#include "SubscriptionsHolder.h"
#include "ILogger.h"
#include "f4se/PapyrusEvents.h"
#include "SerializationHelper.h"

constexpr int dataStructure1 = 1;
UInt32 SubscriptionsHolder::currentDataStructureVersion = dataStructure1;
SubscriptionsHolder g_Subscriptions;

bool SubscriptionsHolder::Register(UInt64 handle, BSFixedString scriptName)
{
	const ScriptHandle script{ handle, scriptName };
	BSWriteLocker lock(&handlersLock);
	if (handlers.find(script) != handlers.end())
	{
		return FW("Subscriber with name %s is already registered. Tried %016X", scriptName.c_str(), handle);
	}

	if (!handlers.insert(script).second)
	{
		return FE("Failed to insert Subscriber [%s](%016X)", scriptName.c_str(), handle);
	}

	IObjectHandlePolicy* policy = (*g_gameVM)->m_virtualMachine->GetHandlePolicy();
	policy->AddRef(handle);

	return true;

}
bool SubscriptionsHolder::Contains(UInt64 handle, BSFixedString scriptName)
{
	const ScriptHandle script{ handle, scriptName };
	BSReadLocker lock(&handlersLock);
	return handlers.find(script) != handlers.end();
}

bool SubscriptionsHolder::Unregister(UInt64 handle, BSFixedString scriptName)
{
	const ScriptHandle script{ handle, scriptName };
	BSWriteLocker lock(&handlersLock);
	const auto iter = handlers.find(script);
	if (iter == handlers.end())
	{
		return FW("Subscriber %s not found", scriptName.c_str());
	}
	VirtualMachine* vm = (*g_gameVM)->m_virtualMachine;
	handlers.erase(iter);
	vm->GetHandlePolicy()->Release(handle);
	return true;

}
constexpr UInt32 rootTag = 'ASSH';

bool SubscriptionsHolder::Save(const F4SESerializationInterface* serializer)
{
	if (!serializer->OpenRecord(rootTag, SubscriptionsHolder::currentDataStructureVersion))
		return FE("Failed to open record (%.4s)", &rootTag);

	const UInt32 size = handlers.size();
	if (!serializer->WriteRecordData(&size, sizeof(size)))
	{
		return FE("Failed to write size of subscribers");
	}

	for (auto& script : handlers)
	{
		if (!SerializationHelper::WriteString(serializer, &script.Name))
		{
			return FE("Failed to write subscriber script name");
		}
		if (!Serialization::WriteData(serializer, &script.Handle))
		{
			return FE("Failed to write subscriber script handle");
		}
	}

	return true;

}

bool SubscriptionsHolder::Load(const F4SESerializationInterface* serializer)
{
	UInt32 recType, length, savedVersion;
	if (!serializer->GetNextRecordInfo(&recType, &savedVersion, &length))
		return FW("Can't get saved record");
	if (recType != rootTag)
		return FE("Invalid record type (%.4s). Expected (%.4s)", &recType, &rootTag);

	if (savedVersion != currentDataStructureVersion)
	{
		return FE("Wrong SubscriptionsHolder version %d. Expected %d", savedVersion, currentDataStructureVersion);
	}
	UInt32 size;
	if (!serializer->ReadRecordData(&size, sizeof(size)))
	{
		return FE("Failed to read size of subscribers");
	}
	VirtualMachine * vm = (*g_gameVM)->m_virtualMachine;
	for (UInt32 i = 0; i < size; i++)
	{
		ScriptHandle script;
		if (!SerializationHelper::ReadString(serializer, &script.Name))
		{
			return FE("Failed to read subscriber's script name");
		}
		if (!Serialization::ReadData(serializer, &script.Handle))
		{
			return FE("Failed to read subscriber's script handle");
		}
		bool valid = SerializationHelper::ResolveHandle(serializer, script.Handle, &script.Handle);
		if (valid)
		{
			VMIdentifier * identifier = nullptr;
			if (vm->GetObjectIdentifier(script.Handle, script.Name.c_str(), 0, &identifier, 0))
			{
				// release our reference
				if(identifier)
				{
					valid = true;
					if(!identifier->DecrementLock())
					{
						identifier->Destroy();
						ILogger::Log("Destroyed identifier for script %s (%016X)", script.Name.c_str(), script.Handle);
					}
				}
				else
				{
					valid = false;
				}
			}
			else
			{
				valid = false;
			}
		}
		if (valid)
		{
			if (!handlers.emplace(script).second)
				ILogger::Log("Subscriptions already have record for [%s]", script.Name.c_str());
		}
		else
		{
			ILogger::Log("[%s] subscriber disappeared.", script.Name.c_str());
		}
	}
	return true;
}

void SubscriptionsHolder::Clear()
{
	BSWriteLocker lock(&handlersLock);
	handlers.clear();
}

constexpr char* eventName = "OnScenesReloadFinished";
void SubscriptionsHolder::NotifyDataReloadFinish(bool succeed)
{
	ILogger::Log("Notifying data reload finish");
	BSReadLocker lock(&handlersLock);
	for (auto& script : handlers)
	{
		SendPapyrusEvent1<bool>(script.Handle, script.Name, eventName, succeed);
	}
}

void SubscriptionsHolder::DumpAll()
{
	// TODO: implement
}
