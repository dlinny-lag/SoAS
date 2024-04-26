#include <string>
#include "DataLoader.h"
#include "ExeFileInfo.hpp"
#include "Shlwapi.h"
#include "ShlObj.h"
#include <mutex>

#include "ILogger.h"

volatile bool finished = false;
volatile bool started = false;
volatile bool succeed = false;
DataHolder data;

std::condition_variable completion;
std::mutex completion_mutex;

constexpr const char* const exchangeFile = "__SoAS_DATA.bin";
constexpr const char* const exeName = "ScenesEditor.exe";

const char * LocalRunFlagFile = "ASLocalRun.txt";

std::string collectorExePath;
std::string exchangePath;

int IsFileExists(const char * path)
{
	WIN32_FIND_DATA FindFileData;
	const HANDLE handle = FindFirstFileA(path, &FindFileData);
	const bool retVal = INVALID_HANDLE_VALUE != handle;
	FindClose(handle);
	return retVal;
}

std::string DesktopPath = "";
void EnsureDesktopPath()
{
	if (!DesktopPath.empty())
		return;

	char desktopPath[MAX_PATH];
	if (S_OK != SHGetFolderPathA(HWND_DESKTOP,CSIDL_DESKTOPDIRECTORY, NULL, 0, desktopPath))
	{
		E("Failed to get desktop path");
		return;
	}
	DesktopPath = desktopPath;
}

bool IsLocalRun()
{
	EnsureDesktopPath();
	const std::string file = DesktopPath + "\\" + LocalRunFlagFile;
	return IsFileExists(file.c_str());
}

std::string GetCollectorPath()
{
	std::string retVal;
	if (IsLocalRun())
	{
		retVal = R"(C:\Dev\Fo4\AnimScenes\ScenesEditor\bin\)";
#if _DEBUG
		retVal += "Debug";
#else
		retVal += "Release";
#endif
	}
	else
	{
		char path[MAX_PATH];
		HMODULE hm = NULL;

		if (GetModuleHandleEx(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS |GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT, (LPCSTR)&GetCollectorPath, &hm) == 0)
		{
			DWORD ret = GetLastError();
			return "";
		}
		if (GetModuleFileName(hm, path, sizeof(path)) == 0)
		{
			DWORD ret = GetLastError();
			return "";
		}

		const std::string dllPath(path); // %FALLOUT 4%\Data\F4SE\plugins\SoAS.dll
		// expected path is %FALLOUT 4%\Data\Scenes\tools\
		//
		
		const size_t index = dllPath.find_last_of('\\');
		if (index == std::string::npos)
			return "";
		retVal = dllPath.substr(0, index);
		retVal += "\\..\\..\\Scenes\\tools"; // TODO: reduce path length
	}

	retVal += "\\";
	retVal += exeName;
	return retVal;
}

DWORD ExecuteDataGenerator()
{
	char cmd[MAX_PATH];
	cmd[MAX_PATH - 1] = 0;
	sprintf_s(cmd, MAX_PATH - 1, R"("%s" "%s")", collectorExePath.c_str(), exchangePath.c_str());
	ILogger::Log("Executing %s", cmd);
	STARTUPINFO si;
	PROCESS_INFORMATION pi;
	ZeroMemory(&si, sizeof(si));
	si.cb = sizeof(si);
	ZeroMemory(&pi, sizeof(pi));
	if (!CreateProcess(NULL, cmd, NULL, NULL, FALSE, CREATE_NO_WINDOW, NULL, NULL, &si, &pi))
	{
		E("Failed to execute Scenes Collector with error = %d", GetLastError());
		return -1;
	}

	WaitForSingleObject(pi.hProcess, INFINITE);
	DWORD exitCode = -100;
	GetExitCodeProcess(pi.hProcess, &exitCode);
	CloseHandle(pi.hProcess);
	CloseHandle(pi.hThread);
	if (exitCode != 0)
	{
		E("%s failed with error 0x%X", exchangePath.c_str(), exitCode);
		return exitCode;
	}
	return 0;
}


DWORD WINAPI ThreadFunc(const std::function<void(bool)> OnFinish)
{
	const DWORD result = ExecuteDataGenerator();
	if (result != 0)
	{
		finished = true;
		succeed = false;
		completion.notify_all();
		OnFinish(false);
		return result;
	}
	std::vector<std::string> errors;
	succeed = data.Deserialize(exchangePath, errors);
	finished = true;
	if (succeed)
	{
		DeleteFile(exchangePath.c_str());
		ILogger::Log("Done");
	}
	else
	{
		E("Failed to deserialize from %s", exchangePath.c_str());
		for(const auto& err : errors)
		{
			E(err.c_str());
		}
	}
	started = false;
	completion.notify_all();
	OnFinish(succeed);
	return 0;
}


bool DataLoader::StartLoading(std::function<void(bool)> onFinish)
{
	if (started)
		return FW("Started already");
	ILogger::Log("Starting data loading...");
	started = true;

	if (collectorExePath.empty())
		collectorExePath = GetCollectorPath();
#if _DEBUG
	constexpr char hash[32] = {0x8D_c,0x61_c,0xF3_c,0x5B_c,0x2E_c,0x89_c,0x1B_c,0x55_c,0xCD_c,0xB3_c,0xC7_c,0x99_c,0x15_c,0x1F_c,0x0C_c,0x7D_c,0x7A_c,0xE9_c,0x87_c,0xF1_c,0x91_c,0xFD_c,0xAE_c,0x5E_c,0x77_c,0x81_c,0xA3_c,0xD3_c,0x82_c,0xAF_c,0x2C_c,0xD8_c};
	const FileInfo<32, 141824> fi(hash);
#else
	constexpr char hash[32] = {0x88_c,0x10_c,0x83_c,0x9E_c,0x11_c,0xCE_c,0x76_c,0x15_c,0xA9_c,0x8C_c,0x5D_c,0x5D_c,0xC3_c,0xE7_c,0xD8_c,0x61_c,0xEF_c,0x79_c,0xE8_c,0xF6_c,0xFD_c,0x6D_c,0xBC_c,0x37_c,0xDD_c,0xB8_c,0xF4_c,0x81_c,0x80_c,0xF4_c,0x53_c,0x0D_c};
	const FileInfo<32, 132608> fi(hash);
#endif


	if (!fi.ValidateFile(collectorExePath))
	{
		started = false;
		return false;
	}
	char myDocs[MAX_PATH];

	const HRESULT err = SHGetFolderPath(NULL, CSIDL_MYDOCUMENTS | CSIDL_FLAG_CREATE, NULL, SHGFP_TYPE_CURRENT, myDocs);
	if (!SUCCEEDED(err))
	{
		started = false;
		E("Failed to get exchange file path");
		return false;
	}

	exchangePath = myDocs;
	exchangePath += R"(\My Games\Fallout4\F4SE\)";
	exchangePath += exchangeFile;

	std::thread thread(ThreadFunc, std::move(onFinish));
	thread.detach();
	return true;
}

bool DataLoader::IsInProgress()
{
	return started;
}

bool DataLoader::HasData()
{
	return finished && succeed;
}

bool DataLoader::IsSucceed()
{
	return succeed;
}

bool DataLoader::IsFinished()
{
	return finished;
}

bool DataLoader::GetResult(DataHolder& result)
{
	if (!HasData())
		return false;

	finished = false;
	result = std::move(data);
	return true;
}

void DataLoader::WaitForComplete()
{
	if (!started)
		return;
	std::unique_lock<std::mutex> lock(completion_mutex);
	completion.wait(lock);
}
