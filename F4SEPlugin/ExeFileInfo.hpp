#pragma once
#include "blake2.h"
#include "handles.hpp"
#include "ILogger.h"

constexpr char operator "" _c(unsigned long long arg) noexcept
{
	return static_cast<char>(arg);
}

template<int HashLength, size_t FileSize>
class FileInfo
{
	static std::string ToArray(std::string_view view)
	{
		std::string retVal = "{";
		for(const unsigned char c : view)
		{
			char buffer[16];
			snprintf(buffer, 16, "0x%02X_c", c);
			if (retVal.size() > 1)
				retVal += ",";
			retVal += buffer;
		}
		retVal += "}";
		return retVal;
	}

	char expectedHash[HashLength];
public:
	FileInfo(const char hash[HashLength])
	{
		memcpy(expectedHash, hash, sizeof(expectedHash));
	}
	[[nodiscard]] bool ValidateFile(const std::string& path) const
	{
		const std::string_view expectedHashView(expectedHash, sizeof(expectedHash));
		const auto_handle exeFile = CreateFile(path.c_str(), GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
		if (exeFile == INVALID_HANDLE_VALUE)
		{
			return FE("Couldn't open %s", path.c_str());
		}
		LARGE_INTEGER size;
		if (!GetFileSizeEx(exeFile, &size))
		{
			return FE("Failed to get %s file size", path.c_str());
		}

		if (size.HighPart != 0)
		{
			return FE("Invalid %s file size", path.c_str());
		}
		if (size.LowPart != FileSize)
		{
			return FE("%s size mismatch. Expected %d, but got %d", path.c_str(), FileSize, size.LowPart);
		}

		const auto_handle fmh = CreateFileMapping(exeFile, NULL, PAGE_READONLY, 0, FileSize, NULL);
		if (fmh == NULL)
		{
			return FE("Failed to map %s", path.c_str());
		}

		const auto_map fileMap = MapViewOfFile(fmh, FILE_MAP_READ, 0, 0, FileSize);
		if (fileMap == nullptr)
		{
			return FE("Failed to read %s", path.c_str());
		}

		blake2b_state bState;
		char hashData[HashLength];
		if (blake2b_init(&bState, HashLength) < 0)
		{
			return FE("Failed to init blake2");
		}
		if (blake2b_update(&bState, fileMap, FileSize) < 0)
		{
			return FE("Failed to update blake2");
		}
		if (blake2b_final(&bState, &hashData[0], HashLength) < 0)
		{
			return FE("Failed to finalize blake2");
		}

		const std::string_view hash(&hashData[0], HashLength);
		if (hash != expectedHashView)
		{
			return FE("Invalid hash. Expected %s", ToArray(hash).c_str());
		}
		return true;
	}

};