#include <functional>
#include <unordered_map>
#include <tuple>
#include "StringUtils.hpp"
#include "ContactFlag.hpp"
#include "ContactStructBuilder.h"

#include "ILogger.h"
#include "f4se/PapyrusArgs.h"

struct my_tuple_hashier
{
	std::size_t operator()(const std::tuple<std::string_view, UInt64>& key) const
	{
		const size_t hash1 = std::hash<std::string_view>()(std::get<0>(key));
		const size_t hash2 = std::hash<UInt64>()(std::get<1>(key));
		return ((hash1 << 5) + hash1) ^ hash2;
	}
};


std::unordered_map<std::tuple<std::string_view, UInt64>, std::function<void(const Data::Contact& source, VMValue& toInit)>,my_tuple_hashier> staticAssigners = 
{
	{{"PARTICIPANTINDEX", VMValue::kType_Int}, [](const Data::Contact& source, VMValue& toInit){toInit.SetInt(source.ParticipantIndex);}},
	{{"AREA", VMValue::kType_String}, [](const Data::Contact& source, VMValue& toInit){toInit.SetString(source.Area.Id.c_str());}},
	{{"STIMULATION", VMValue::kType_Int}, [](const Data::Contact& source, VMValue& toInit){toInit.SetInt(source.Stimulation);}},
	{{"HOLD", VMValue::kType_Int}, [](const Data::Contact& source, VMValue& toInit){toInit.SetInt(source.Hold);}},
	{{"PAIN", VMValue::kType_Int}, [](const Data::Contact& source, VMValue& toInit){toInit.SetInt(source.Pain);}},
	{{"COMFORT", VMValue::kType_Int}, [](const Data::Contact& source, VMValue& toInit){toInit.SetInt(source.Comfort);}},
	{{"TICKLE", VMValue::kType_Int}, [](const Data::Contact& source, VMValue& toInit){toInit.SetInt(source.Tickle);}},
	{{"PAINTYPE", VMValue::kType_Int}, [](const Data::Contact& source, VMValue& toInit){toInit.SetInt(static_cast<SInt32>(source.PainType));}},
};

inline bool TrySetFlag(VMValue& toInit, const Data::Contact& source, const std::string_view pattern)
{
	bool isValid;
	const bool flag = ContactFlag::Is(source, pattern.data(), isValid);
	if (!isValid)
		return false;
	toInit.SetBool(flag);
	return true;
}

VMValue GetContactValue(const Data::Contact& source, UInt64 type, std::string_view pattern)
{
	const auto iter = staticAssigners.find(std::make_tuple(pattern, type));
	if (iter != staticAssigners.end())
	{
		VMValue r;
		iter->second(source, r);
		return r;
	}
	if (type == VMValue::kType_Bool)
	{
		VMValue r;
		if (TrySetFlag(r, source, pattern))
			return r;
	}
	return PapyrusStructHelper::DefaultFieldValue(type);
}
VMValue TryGetActor(const Data::Contact& source, const std::vector<Actor*>& actors)
{
	VirtualMachine* vm = (*g_gameVM)->m_virtualMachine;
	const UInt32 index = source.ParticipantIndex;
	if (index + 1 > actors.size()) // empty list, most likely
		return VMValue(); // None
	VMValue retVal;
	Actor* actor = actors[index];
	PackValue(&retVal, &actor, vm);
	return retVal;
}

VMObjectTypeInfo* actorType;
bool CanHoldActor(UInt64 fieldType)
{
	if (!actorType)
	{
		VirtualMachine* vm = (*g_gameVM)->m_virtualMachine;
		if (vm->GetObjectTypeInfo(Actor::kTypeID, &actorType))
			if (actorType)
				actorType->Release();
	}
	if (!actorType)
		return FE("Failed to get Actor's type info");
	IComplexType* curr = actorType;
	while(curr)
	{
		if ( (UInt64)curr == fieldType)
			return true;
		curr = curr->m_parent;
	}
	return false;
}

void ContactStruct::ResetCache()
{
	actorType = nullptr;
}

VMValue GetFieldValue(const std::string_view pattern, UInt64 fieldType, const Data::Contact& contact, const std::vector<Actor*>& actors)
{
	if (pattern == "PARTICIPANT" && CanHoldActor(fieldType))
		return TryGetActor(contact, actors);
	return GetContactValue(contact, fieldType, pattern);
}


VMValue ContactStruct::Create(const Data::ActorsContact& actorsContact, BSFixedString structName, const std::vector<Actor*>& actors)
{
	const auto initializer = [&actorsContact, &actors](UInt64 fieldType, BSFixedString fieldName)
	{
		const auto normalizedName = SU::ToUpper(fieldName.c_str());
		if (SU::StartsFrom(normalizedName.c_str(), "FROM_"))
		{
			const std::string_view pattern = std::string_view(normalizedName.c_str() + 5);
			return GetFieldValue(pattern, fieldType, actorsContact.From, actors);
		}
		if (SU::StartsFrom(normalizedName.c_str(), "TO_"))
		{
			const std::string_view pattern = std::string_view(normalizedName.c_str() + 3);
			return GetFieldValue(pattern, fieldType, actorsContact.To, actors);
		}
		return PapyrusStructHelper::DefaultFieldValue(fieldType);
	};
	return PapyrusStructHelper::CreateAndInitializeInstance(structName, initializer);
}

VMValue ContactStruct::Create(const Data::EnvironmentContact& envContact, BSFixedString structName, const std::vector<Actor*>& actors)
{
	const auto initializer = [&envContact, &actors](UInt64 fieldType, BSFixedString fieldName)
	{
		const auto normalizedName = SU::ToUpper(fieldName.c_str());
		if (normalizedName == "Direction" && fieldType == VMValue::kType_Int)
		{
			VMValue r;
			r.SetInt(static_cast<SInt32>(envContact.Direction));
			return r;
		}
		return GetFieldValue(std::string_view(normalizedName.c_str()), fieldType, envContact.Contact, actors);
	};
	return PapyrusStructHelper::CreateAndInitializeInstance(structName, initializer);
}
