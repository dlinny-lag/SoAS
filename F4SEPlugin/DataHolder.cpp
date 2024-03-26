
#include "DataHolder.h"
#include "handles.hpp"
#include "Json.h"
#include "StringUtils.hpp"

template<typename IntType>
bool ReadInt(HANDLE theFile, IntType& result)
{
	DWORD bytesRead;
	const bool retVal = ReadFile(theFile, &result, sizeof(result), &bytesRead, NULL);
	if (!retVal)
		return retVal;
	return sizeof(result) == bytesRead;
}

bool ReadString(HANDLE theFile, std::string& result)
{
	SInt32 len = 0;
	DWORD bytesRead;
	if (!ReadInt(theFile, len))
		return false;

	if (len > 0)
	{
		char* data = new char[len];
		if (!ReadFile(theFile, data, len, &bytesRead, NULL))
		{
			delete[] data;
			return false;
		}
		result.assign(data, len);
		delete[] data;
	}
	return true;
}
bool ReadStrings(HANDLE theFile, std::vector<std::string>& result)
{
	UInt32 count;
	if (!ReadInt(theFile, count))
		return false;
	result.reserve(count);
	for(UInt32 i = 0; i < count; i++)
	{
		std::string val;
		if (!ReadString(theFile, val))
			return false;
		result.emplace_back(val);
	}
	return true;
}
bool ReadParticipant(HANDLE theFile, Data::Participant& result)
{
	if (!ReadString(theFile, result.Skeleton))
		return false;
	UInt32 readSex;
	if (!ReadInt(theFile, readSex))
		return false;
	switch (readSex)
	{
		case 0: result.Sex = Data::Sex::Any; break;
		case 1: result.Sex = Data::Sex::Male; break;
		case 2: result.Sex = Data::Sex::Female; break;
		default: return false;
	}
	if (!ReadInt(theFile, result.IsAggressor))
		return false;
	if (!ReadInt(theFile, result.IsVictim))
		return false;

	return true;
}
bool ReadParticipants(HANDLE theFile, std::vector<Data::Participant>& result)
{
	UInt32 count;
	if (!ReadInt(theFile, count))
		return false;
	for (UInt32 i = 0; i < count; i++)
	{
		Data::Participant participant;
		if (!ReadParticipant(theFile, participant))
			return false;
		result.emplace_back(participant);
	}
	return true;
}

bool ReadContactArea(HANDLE theFile, Data::ContactArea& result)
{
	return ReadString(theFile, result.Id);
}

bool ReadContact(HANDLE theFile, Data::Contact& result)
{
	if (!ReadInt(theFile, result.ParticipantIndex))
		return false;
	if (!ReadContactArea(theFile, result.Area))
		return false;
	if (!ReadString(theFile, result.ReversePath))
		return false;
	if (!ReadInt(theFile, result.Stimulation))
		return false;
	if (!ReadInt(theFile, result.Hold))
		return false;
	if (!ReadInt(theFile, result.Pain))
		return false;
	if (!ReadInt(theFile, result.Comfort))
		return false;
	if (!ReadInt(theFile, result.Tickle))
		return false;
	if (!ReadInt(theFile, result.PainType))
		return false;
	return true;
}

bool ReadActorsContact(HANDLE theFile, Data::ActorsContact& result)
{
	if (!ReadContact(theFile, result.From))
		return false;
	if (!ReadContact(theFile, result.To))
		return false;
	return true;
}

bool ReadActorsContacts(HANDLE theFile, std::vector<Data::ActorsContact>& result)
{
	UInt32 count;
	if (!ReadInt(theFile, count))
		return false;
	result.reserve(count);
	for(UInt32 i = 0; i < count; i++)
	{
		Data::ActorsContact contact;
		if (!ReadActorsContact(theFile, contact))
			return false;
		result.emplace_back(contact);
	}
	return true;
}

bool ReadEnvironmentContact(HANDLE theFile, Data::EnvironmentContact& contact)
{
	UInt32 dir;
	if (!ReadInt(theFile, dir))
		return false;
	switch (dir)
	{
		case 0: contact.Direction = Data::Directions::FromEnvironment; break;
		case 1: contact.Direction = Data::Directions::FromActor; break;
		default: return false;
	}
	if (!ReadContact(theFile, contact.Contact))
		return false;

	return true;
}

bool ReadEnvironmentContacts(HANDLE theFile, std::vector<Data::EnvironmentContact>& result)
{
	UInt32 count;
	if (!ReadInt(theFile, count))
		return false;
	result.reserve(count);
	for(UInt32 i = 0; i < count; i++)
	{
		Data::EnvironmentContact contact;
		if (!ReadEnvironmentContact(theFile, contact))
			return false;
		result.emplace_back(contact);
	}
	return true;
}

Json::JObject* ReadJObject(HANDLE theFile);
Json::JArray* ReadJArray(HANDLE theFile);

bool ReadJValue(HANDLE theFile, Json::JToken& result)
{
	Json::TokenType type;
	if (!ReadInt(theFile, type))
		return false;
	switch (type)
	{
		case Json::Null:
			{
				result = Json::JToken();
				return true;
			}
		case Json::Array:
			{
				Json::JArray* arr = ReadJArray(theFile);
				if (!arr)
					return false;
				result = std::unique_ptr<Json::JArray>(arr);
				return true;
			}
		case Json::Object:
			{
				Json::JObject* obj = ReadJObject(theFile);
				if (!obj)
					return false;
				result = std::unique_ptr<Json::JObject>(obj);
				return true;
			}
		case Json::Integer:
			{
				SInt32 val;
				if (!ReadInt(theFile, val))
					return false;
				result = val;
				return true;
			}
		case Json::Float:
			{
				float val;
				if (!ReadInt(theFile, val))
					return false;
				result = val;
				return true;
			}
		case Json::Boolean:
			{
				bool val;
				if (!ReadInt(theFile, val))
					return false;
				result = val;
				return true;
			}
		case Json::String:
			{
				std::string val;
				if (!ReadString(theFile, val))
					return false;
				result = val;
				return true;
			}
		case Json::Property:
			return false;
	}
	return false;
}

bool ReadJProperty(HANDLE theFile, std::string& name, Json::JToken& value)
{
	Json::TokenType type;
	if (!ReadInt(theFile, type))
		return false;
	if (type != Json::Property)
		return false;
	if (!ReadString(theFile, name))
		return false;
	return ReadJValue(theFile, value);
}

Json::JObject* ReadJObject(HANDLE theFile)
{
	UInt32 count;
	if (!ReadInt(theFile, count))
		return nullptr;
	std::unordered_map<std::string, Json::JToken> properties;
	properties.reserve(count);
	for(UInt32 i = 0; i < count; i++)
	{
		std::string name;
		Json::JToken value;
		if (!ReadJProperty(theFile, name, value))
			return nullptr;
		properties.emplace(name, std::move(value));
	}
	return new Json::JObject{std::move(properties)};
}
Json::JArray* ReadJArray(HANDLE theFile)
{
	UInt32 count;
	if (!ReadInt(theFile, count))
		return nullptr;
	std::vector<Json::JToken> elements;
	elements.reserve(count);
	for(UInt32 i = 0; i < count; i++)
	{
		Json::JToken value;
		if (!ReadJValue(theFile, value))
			return nullptr;
		elements.emplace_back(std::move(value));
	}
	return new Json::JArray{std::move(elements)};
}

bool ReadCustomAttributes(HANDLE theFile, Data::CustomAttributes& result)
{
	Json::TokenType type;
	if (!ReadInt(theFile, type))
		return false;
	if (type != Json::Object) // no null expected
		return false;
	Json::JObject* obj = ReadJObject(theFile);
	if (!obj)
		return false;
	result = *obj;
	delete obj;
	return true;
}


bool ReadScene(HANDLE theFile, Data::Scene& result)
{
	if (!ReadString(theFile, result.Id))
		return false;
	if (!ReadInt(theFile, result.Type))
		return false;
	if (!ReadStrings(theFile, result.Furniture))
		return false;
	if (!ReadParticipants(theFile, result.Participants))
		return false;
	if (!ReadActorsContacts(theFile, result.ActorsContacts))
		return false;
	if (!ReadEnvironmentContacts(theFile, result.EnvironmentContacts))
		return false;
	if (!ReadStrings(theFile, result.Tags))
		return false;
	if (!ReadStrings(theFile, result.Authors))
		return false;
	if (!ReadStrings(theFile, result.Narrative))
		return false;
	if (!ReadStrings(theFile, result.Feeling))
		return false;
	if (!ReadStrings(theFile, result.Service))
		return false;
	if (!ReadStrings(theFile, result.Attribute))
		return false;
	if (!ReadStrings(theFile, result.Other))
		return false;
	if (!ReadCustomAttributes(theFile, result.Custom))
		return false;

	return true;
}

bool DataHolder::Deserialize(const std::string& filePath, std::vector<std::string>& errors)
{
	const auto_handle theFile = CreateFile(filePath.c_str(), GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
	if (theFile == INVALID_HANDLE_VALUE)
		return false;
	UInt32 scenesCount;
	if (!ReadInt(theFile, scenesCount))
		return false;
	scenes.reserve(scenesCount);
	for(UInt32 i = 0; i < scenesCount; i++)
	{
		Data::Scene scene;
		if (!ReadScene(theFile, scene))
			return false;
		scenes.emplace(SU::ToUpper(scene.Id), std::move(scene));
	}
	// read errors
	UInt32 errorsCount;
	if (!ReadInt(theFile, errorsCount))
		return false;

	if (errorsCount == 0)
		return true; // ok

	errors.reserve(errorsCount);
	for(UInt32 i = 0; i < errorsCount; i++)
	{
		std::string err;
		if (!ReadString(theFile, err))
		{
			errors.emplace_back("Failed to read error string");
			return false;
		}
		errors.emplace_back(std::move(err));
	}
	return false;
}

void DataHolder::Dump(std::ostream& stream) const
{
	for(const auto& pair : scenes)
	{
		pair.second.Dump(stream);
	}
}

const Data::Scene* DataHolder::Find(const char* sceneId) const
{
	const auto pair = scenes.find(SU::ToUpper(sceneId));
	if (pair == scenes.end())
		return nullptr;
	return &pair->second;
}
