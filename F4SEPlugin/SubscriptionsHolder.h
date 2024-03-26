#pragma once
#include <unordered_set>
#include <f4se/PluginAPI.h>

#include "ScriptHandle.hpp"

class SubscriptionsHolder
{
	std::unordered_set<ScriptHandle> handlers;
	BSReadWriteLock handlersLock;
public:
	bool Register(UInt64 handle, BSFixedString scriptName);
	bool Unregister(UInt64 handle, BSFixedString scriptName);
	bool Contains(UInt64 handle, BSFixedString scriptName);
	void NotifyDataReloadFinish(bool succeed);

	static UInt32 currentDataStructureVersion;
	bool Save(const F4SESerializationInterface* serializer);
	bool Load(const F4SESerializationInterface* serializer);
	void Clear();

	void DumpAll();
};

extern SubscriptionsHolder g_Subscriptions;

