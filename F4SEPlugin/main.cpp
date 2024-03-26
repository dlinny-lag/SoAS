#include <shlobj.h>
#include "Shlwapi.h"
#include "f4se/PluginAPI.h"
#include "f4se_common/f4se_version.h"
#include "f4se/PapyrusVM.h"
#include "f4se/GameForms.h"
#include "PluginAPIExport.hpp"
#include "F4SELogger.h"

#define REQUIRED_RUNTIME RUNTIME_VERSION_1_10_163

F4SEPapyrusInterface* g_papyrus = nullptr;
F4SEMessagingInterface* g_messaging = nullptr;
F4SESerializationInterface* g_serialization = nullptr;
PluginHandle	g_pluginHandle = kPluginHandle_Invalid;


bool RegisterExportingFunctions(VirtualMachine* vm)
{
	PluginAPIExport::Register(vm);
	return true;
}
void Serialization_Revert(const F4SESerializationInterface* intfc)
{
	g_Subscriptions.Clear();
}
void Serialization_Save(const F4SESerializationInterface* intfc)
{
	g_Subscriptions.Save(intfc);
}
void Serialization_Load(const F4SESerializationInterface* intfc)
{
	g_Subscriptions.Load(intfc);
}



void MessageHandler(F4SEMessagingInterface::Message* msg)
{
	if (F4SEMessagingInterface::kMessage_GameLoaded == msg->type || F4SEMessagingInterface::kMessage_NewGame == msg->type)
	{
		ContactStruct::ResetCache();
	}
}
#include <cinttypes>
extern "C"
{
	bool F4SEPlugin_Query(const F4SEInterface * f4se, PluginInfo * info)
	{
		gLog.OpenRelative(CSIDL_MYDOCUMENTS, R"(\My Games\Fallout4\F4SE\SoAS.log)");
		ILogger::Init(&g_f4seLogger);
		g_f4seLogger.LogToConsole = true;// TODO: add Papyrus API for this setting
		ILogger::Level = ILogger::Debug;

		g_pluginHandle = f4se->GetPluginHandle();
		info->infoVersion = PluginInfo::kInfoVersion;
		info->name = PluginAPIExport::pluginName;
		info->version = PluginAPIExport::pluginVersionInt;

		if(f4se->runtimeVersion < REQUIRED_RUNTIME)
		{
			_ERROR("Unsupported runtime version %08X (expected %08X or higher)", f4se->runtimeVersion, REQUIRED_RUNTIME);
			return false;
		}

		g_papyrus = static_cast<F4SEPapyrusInterface*>(f4se->QueryInterface(kInterface_Papyrus));
		if(!g_papyrus)
		{
			_ERROR("Failed to get F4SEPapyrusInterface");
			return false;
		}
		g_messaging = static_cast<F4SEMessagingInterface*>(f4se->QueryInterface(kInterface_Messaging));
		if (!g_messaging)
		{
			_ERROR("Failed to get F4SEMessagingInterface");
			return false;
		}
		g_serialization = (F4SESerializationInterface*)f4se->QueryInterface(kInterface_Serialization);
		if (!g_serialization)
		{
			_ERROR("Failed to get F4SESerializationInterface");
			return false;
		}
		return true;
	}

	bool F4SEPlugin_Load(const F4SEInterface * f4se)
	{
		if (g_messaging)
			g_messaging->RegisterListener(g_pluginHandle, "F4SE", MessageHandler);

		g_serialization->SetUniqueID(g_pluginHandle, PluginAPIExport::pluginUID);
		g_serialization->SetRevertCallback(g_pluginHandle, Serialization_Revert);
		g_serialization->SetSaveCallback(g_pluginHandle, Serialization_Save);
		g_serialization->SetLoadCallback(g_pluginHandle, Serialization_Load);

		g_papyrus->Register(RegisterExportingFunctions);

		ILogger::Log("Started Semantics of Animation Scenes. Version %s", PluginAPIExport::pluginVersionString);
		ILogger::Log("LogLevel=%d", ILogger::Level);
		ILogger::Log("LogToConsole=%s", S(g_f4seLogger.LogToConsole));
		ContactStruct::ResetCache();
		if (!DataLoader::StartLoading(PluginAPIExport::OnScenesDataLoadFinished))
			E("Failed to initiate data loading");

		return true;
	}

};
