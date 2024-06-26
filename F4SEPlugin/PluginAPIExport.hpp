#pragma once

#include <optional>

#include "f4se/PapyrusVM.h"
#include "f4se/PapyrusNativeFunctions.h"
#include "f4se/PapyrusEvents.h"
#include "f4se/PapyrusStruct.h"

#include "DataLoader.h"
#include "SubscriptionsHolder.h"
#include "StringUtils.hpp"
#include <ostream>

#include "F4SELogger.h"
#include "ContactStructBuilder.h"
#include "SceneStruct.h"

namespace PluginAPIExport
{
#define AS_EXPORT_PAPYRUS_SCRIPT "SoAS"
const char* EXPORT_PAPYRUS_SCRIPT = AS_EXPORT_PAPYRUS_SCRIPT;

	constexpr UInt32 pluginUID = 'SoAS';

	const char* pluginName = "SoAS";
	const UInt32 pluginVersionInt = 0x0050;
	const char* pluginVersionString = "0.5.0";
	bool dataInitialized = false;
	DataHolder scenes;
	BSReadWriteLock dataLock;

	inline VMVariable ToVar(const VMValue& val)
	{
		VMVariable r;
		r.GetValue() = val;
		return r;
	}
	inline VMVariable NoneVar()
	{
		VMVariable r;
		return r;
	}

	BSFixedString GetVersionString(StaticFunctionTag* _)
	{
		return pluginVersionString;
	};

	SInt32 GetVersionInt(StaticFunctionTag* _)
	{
		return pluginVersionInt;
	};

	void OnScenesDataLoadFinished(bool result)
	{
		BSWriteLocker lock(&dataLock);
		dataInitialized = DataLoader::GetResult(scenes);
		g_Subscriptions.NotifyDataReloadFinish(result);
	}

	bool StartDataReload(StaticFunctionTag* _)
	{
		const bool retVal = DataLoader::StartLoading(OnScenesDataLoadFinished);
		if (!retVal)
		{
			if (ConsoleManager* mgr = *g_console)
			{
				CALL_MEMBER_FN(mgr, Print)("Failed to start data reloading");
			}
		}
		else
		{
			BSWriteLocker lock(&dataLock);
			dataInitialized = false;
		}
		return retVal;
	}

	bool RegisterForDataReloadFinish(StaticFunctionTag* _, VMObject* script)
	{
		if (!script)
		{
			return FW("Empty script");
		}
		const UInt64 handle = SafeGetHandle(script);
		if (handle == 0)
		{
			return FE("Invalid script");
		}
		const BSFixedString scriptName = script->GetObjectType();
		const bool res = g_Subscriptions.Register(handle, scriptName);
		ILogger::LogInfo("RegisterForDataReloadFinish: [%s](%016X) with result=%s", scriptName.c_str(), handle, (res?"true":"false"));
		return res;
	}

	bool IsDataReloadFinishRegistered(StaticFunctionTag* _, VMObject* script)
	{
		if (!script)
		{
			return FW("Empty script");
		}
		const UInt64 handle = SafeGetHandle(script);
		if (handle == 0)
		{
			return FE("Invalid script");
		}
		const BSFixedString scriptName = script->GetObjectType();
		const bool res = g_Subscriptions.Contains(handle, scriptName);
		ILogger::LogInfo("IsDataReloadFinishRegistered: [%s](%016X) with result=%s", scriptName.c_str(), handle, (res?"true":"false"));
		return res;
	}

	bool UnregsiterForDataReloadFinish(StaticFunctionTag* _, VMObject* script)
	{
		if (!script)
		{
			return FW("Empty script");
		}

		const UInt64 handle = SafeGetHandle(script);
		if (handle == 0)
		{
			return FE("Invalid script");
		}
		const BSFixedString scriptName = script->GetObjectType();
		const bool res = g_Subscriptions.Unregister(handle, scriptName);
		ILogger::Log("UnregsiterForDataReloadFinish: [%s](%016X) with result=%s", scriptName.c_str(), handle, (res?"true":"false"));
		return res;
	}

	DECLARE_STRUCT(ScriptDescriptor, AS_EXPORT_PAPYRUS_SCRIPT);

	ScriptDescriptor GetSubscribedDescriptor(StaticFunctionTag* _, VMObject* script)
	{
		ScriptDescriptor retVal;
		retVal.SetNone(true);
		if (!script)
		{
			ILogger::LogWarning("Empty script");
			return retVal;
		}
		
		const UInt64 handle = SafeGetHandle(script);
		if (handle == 0)
		{
			ILogger::LogError("Invalid script");
			return retVal;
		}
		const BSFixedString scriptName = script->GetObjectType();
		const bool res = g_Subscriptions.Contains(handle, scriptName);
		if (!res)
			return retVal;

		const UInt32 highWord = handle >> 32;
		const UInt32 formId = handle & 0xFFFFFFFF;
		retVal.SetNone(false);
		retVal.Set<UInt32>("FormId", formId);
		retVal.Set<UInt32>("HighWord", highWord);
		retVal.Set<BSFixedString>("Name", scriptName);

		return retVal;
	}
	bool UnregisterForDataReloadFinishByDescriptor(StaticFunctionTag* _, ScriptDescriptor descriptor)
	{
		if (descriptor.IsNone())
		{
			return FW("None descriptor");
		}
		UInt32 highWord;
		UInt32 formId;
		BSFixedString scriptName;
		if (!descriptor.Get("HighWord", &highWord))
		{
			return FE("Can't get HighWord field");
		}
		if (!descriptor.Get("FormId", &formId))
		{
			return FE("Can't get FormId field");
		}
		if (!descriptor.Get("Name", &scriptName))
		{
			return FE("Can't get Name field");
		}
		UInt64 handle = highWord;
		handle <<= 32;
		handle |= formId;
		const bool res = g_Subscriptions.Unregister(handle, scriptName);
		ILogger::Log("UnregisterForDataReloadFinishByDescriptor: [%s](%016X) with result=%s", scriptName.c_str(), handle, (res?"true":"false"));
		return res;
	}

	template<typename TCopy>
	std::optional<TCopy> GetSceneData(BSFixedString sceneId, const std::function<TCopy(const Data::Scene* scene)>& retrieve)
	{
		if (!dataInitialized)
		{
			W("Data is not ready"); // perhaps, it should be checked separately
			return std::optional<TCopy>(std::nullopt);
		}

		BSReadLocker lock(&dataLock); // lock must remain until scene pointer is in use
		const Data::Scene* scene = scenes.Find(sceneId.c_str());
		if (!scene)
		{
			W("Scene [%s] not found", sceneId.c_str());
			return std::optional<TCopy>(std::nullopt);
		}
		return retrieve(scene);
	}

	DECLARE_STRUCT(Participant, AS_EXPORT_PAPYRUS_SCRIPT);
	VMArray<Participant> GetParticipants(StaticFunctionTag* _, BSFixedString sceneId)
	{
		VMArray<Participant> retVal;
		retVal.SetNone(true);
		const auto participants = GetSceneData<std::vector<Data::Participant>>(sceneId, [](auto scene) {return scene->Participants;});
		if (!participants.has_value())
			return retVal;

		retVal.SetNone(false);
		for (const auto& participant : participants.value())
		{
			Participant p;
			p.Set("Skeleton", BSFixedString(participant.Skeleton.c_str())); // TODO: cache
			p.Set("Sex", static_cast<SInt32>(participant.Sex));
			p.Set("IsAggressor", participant.IsAggressor);
			p.Set("IsVictim", participant.IsVictim);
			retVal.Push(&p, false);
		}
		return retVal;
	}

	std::vector<Actor*> ToVector(VMArray<Actor*>& arr)
	{
		std::vector<Actor*>	retVal;
		if (arr.IsNone())
			return retVal;
		retVal.reserve(arr.Length());
		for(UInt32 i = 0; i < arr.Length(); i++)
		{
			Actor* value;
			arr.Get(&value, i);
			retVal.emplace_back(value);
		}
		return retVal;
	}

	VMArray<VMVariable> GetParticipantsContacts(StaticFunctionTag* _, BSFixedString sceneId, BSFixedString structName, VMArray<Actor*> participants)
	{
		VMArray<VMVariable> retVal;
		retVal.SetNone(true);
		const auto contacts = GetSceneData<std::vector<Data::ActorsContact>>(sceneId, [](auto scene){return scene->ActorsContacts;});
		if (!contacts.has_value())
			return retVal;

		const std::vector<Actor*> actors = ToVector(participants);
		retVal.SetNone(false);

		for (const auto& contact : contacts.value())
		{
			const VMValue val = ContactStruct::Create(contact, structName, actors);
			VMVariable var = ToVar(val);
			retVal.Push(&var);
		}
		D("Returning %d actors contacts for scene [%s]", retVal.Length(), sceneId.c_str());
		return retVal;
	}

	VMArray<VMVariable> GetEnvironmentContacts(StaticFunctionTag* _, BSFixedString sceneId, BSFixedString structName, VMArray<Actor*> participants)
	{
		VMArray<VMVariable> retVal;
		retVal.SetNone(true);
		const auto contacts = GetSceneData<std::vector<Data::EnvironmentContact>>(sceneId, [](const auto scene){return scene->EnvironmentContacts;});
		if (!contacts.has_value())
			return retVal;

		const std::vector<Actor*> actors = ToVector(participants);
		for(const auto& contact : contacts.value())
		{
			const VMValue val = ContactStruct::Create(contact, structName, actors);
			VMVariable var = ToVar(val);
			retVal.Push(&var);
		}
		return retVal;
	}

	DECLARE_STRUCT(SceneStringAttributes, AS_EXPORT_PAPYRUS_SCRIPT);
	SceneStringAttributes GetStringAttributes(StaticFunctionTag* _, BSFixedString sceneId)
	{
		SceneStringAttributes retVal;
		retVal.SetNone(true);

		struct StringsData
		{
			std::vector<std::string> authors;
			std::vector<std::string> narrative;
			std::vector<std::string> feeling;
			std::vector<std::string> service;
			std::vector<std::string> attribute;
			std::vector<std::string> other;
		};

		const auto data = GetSceneData<StringsData>(sceneId, [](const auto scene)
		{
			return StringsData{scene->Authors, scene->Narrative, scene->Feeling, scene->Service, scene->Attribute, scene->Other};
		});
		if (!data.has_value())
			return retVal;

		retVal.SetNone(false);
		retVal.Set("Authors", BSFixedString(SU::Join(data.value().authors, ",").c_str()));
		retVal.Set("Narrative", BSFixedString(SU::Join(data.value().narrative, ",").c_str()));
		retVal.Set("Feeling", BSFixedString(SU::Join(data.value().feeling, ",").c_str()));
		retVal.Set("Service", BSFixedString(SU::Join(data.value().service, ",").c_str()));
		retVal.Set("Attribute", BSFixedString(SU::Join(data.value().attribute, ",").c_str()));
		retVal.Set("Other", BSFixedString(SU::Join(data.value().other, ",").c_str()));

		return retVal;
	}

	VMArray<BSFixedString> GetTags(StaticFunctionTag* _, BSFixedString sceneId)
	{
		VMArray<BSFixedString> retVal;
		retVal.SetNone(true);
		const auto tags = GetSceneData<std::vector<std::string>>(sceneId, [](auto scene){return scene->Tags;});
		if (!tags.has_value())
			return retVal;

		retVal.SetNone(false);
		for(const auto& furn : tags.value())
		{
			BSFixedString f(furn.c_str());
			retVal.Push(&f, false);
		}
		return retVal;
	}

	VMArray<BSFixedString> GetFurnitureTags(StaticFunctionTag* _, BSFixedString sceneId)
	{
		VMArray<BSFixedString> retVal;
		retVal.SetNone(true);

		const auto furniture = GetSceneData<std::vector<std::string>>(sceneId, [](auto scene){return scene->Furniture;});
		if (!furniture.has_value())
			return retVal;

		retVal.SetNone(false);
		for(const auto& furn : furniture.value())
		{
			BSFixedString f(furn.c_str());
			retVal.Push(&f, false);
		}
		return retVal;
	}


	VMVariable GetSceneAttributes(StaticFunctionTag* _, BSFixedString sceneId, BSFixedString structName)
	{
		// TODO: update when predefined scene attributes introduced
		const auto custom = GetSceneData<std::unique_ptr<Json::JObject>>(sceneId, [](auto scene){return scene->Custom.GetData();});
		if (!custom.has_value())
			return NoneVar();

		const VMValue retVal = SceneStruct::Create(custom.value().get(), structName);
		return ToVar(retVal);
	}

	bool Register(VirtualMachine* vm)
	{
		vm->RegisterFunction(new NativeFunction0("GetVersionString", EXPORT_PAPYRUS_SCRIPT, GetVersionString, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "GetVersionString", IFunction::kFunctionFlag_NoWait);

		vm->RegisterFunction(new NativeFunction0("GetVersionInt", EXPORT_PAPYRUS_SCRIPT, GetVersionInt, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "GetVersionInt", IFunction::kFunctionFlag_NoWait);
		
		vm->RegisterFunction(new NativeFunction0("StartDataReload", EXPORT_PAPYRUS_SCRIPT, StartDataReload, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "StartDataReload", IFunction::kFunctionFlag_NoWait);

		// notifications subscribers registering

		vm->RegisterFunction(new NativeFunction1("RegisterForDataReloadFinish", EXPORT_PAPYRUS_SCRIPT, RegisterForDataReloadFinish, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "RegisterForDataReloadFinish", IFunction::kFunctionFlag_NoWait);

		vm->RegisterFunction(new NativeFunction1("IsDataReloadFinishRegistered", EXPORT_PAPYRUS_SCRIPT, IsDataReloadFinishRegistered, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "IsDataReloadFinishRegistered", IFunction::kFunctionFlag_NoWait);

		vm->RegisterFunction(new NativeFunction1("UnregsiterForDataReloadFinish", EXPORT_PAPYRUS_SCRIPT, UnregsiterForDataReloadFinish, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "UnregsiterForDataReloadFinish", IFunction::kFunctionFlag_NoWait);

		vm->RegisterFunction(new NativeFunction1("GetSubscribedDescriptor", EXPORT_PAPYRUS_SCRIPT, GetSubscribedDescriptor, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "GetSubscribedDescriptor", IFunction::kFunctionFlag_NoWait);

		vm->RegisterFunction(new NativeFunction1("UnregisterForDataReloadFinishByDescriptor", EXPORT_PAPYRUS_SCRIPT, UnregisterForDataReloadFinishByDescriptor, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "UnregisterForDataReloadFinishByDescriptor", IFunction::kFunctionFlag_NoWait);

		// Scene attributes API

		vm->RegisterFunction(new NativeFunction1("GetParticipants", EXPORT_PAPYRUS_SCRIPT, GetParticipants, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "GetParticipants", IFunction::kFunctionFlag_NoWait);

		vm->RegisterFunction(new NativeFunction3("GetParticipantsContacts", EXPORT_PAPYRUS_SCRIPT, GetParticipantsContacts, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "GetParticipantsContacts", IFunction::kFunctionFlag_NoWait);

		vm->RegisterFunction(new NativeFunction3("GetEnvironmentContacts", EXPORT_PAPYRUS_SCRIPT, GetEnvironmentContacts, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "GetEnvironmentContacts", IFunction::kFunctionFlag_NoWait);

		vm->RegisterFunction(new NativeFunction1("GetStringAttributes", EXPORT_PAPYRUS_SCRIPT, GetStringAttributes, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "GetStringAttributes", IFunction::kFunctionFlag_NoWait);

		vm->RegisterFunction(new NativeFunction1("GetTags", EXPORT_PAPYRUS_SCRIPT, GetTags, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "GetTags", IFunction::kFunctionFlag_NoWait);

		vm->RegisterFunction(new NativeFunction1("GetFurnitureTags", EXPORT_PAPYRUS_SCRIPT, GetFurnitureTags, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "GetFurnitureTags", IFunction::kFunctionFlag_NoWait);

		vm->RegisterFunction(new NativeFunction2("GetSceneAttributes", EXPORT_PAPYRUS_SCRIPT, GetSceneAttributes, vm));
		vm->SetFunctionFlags(EXPORT_PAPYRUS_SCRIPT, "GetSceneAttributes", IFunction::kFunctionFlag_NoWait);

		return true;
	}
}
