#include "CustomAttributesSearch.h"
#include <charconv>

using namespace Data;

Json::JToken None;

const Json::JToken* FromObject(const Json::JObject* parent, const std::string_view segment)
{
	const auto& props = parent->Properties;
	const auto iter = props.find(std::string(segment));
	if (iter == props.end())
		return &None;
	return &iter->second;
}

const Json::JToken* Step(const Json::JToken* parent, std::string_view segment)
{
	switch (parent->GetType())
	{
		case Json::TokenType::Object:
		{
			return FromObject(parent->AsObject(), segment);
		}
		case Json::TokenType::Array:
		{
			size_t index;
			const auto res = std::from_chars(segment.data(), segment.data() + segment.length(), index);
			if (res.ec == std::errc::invalid_argument)
				return &None;
			const auto& arr = parent->AsArray()->Elements;
			if (index + 1 > arr.size())
				return &None;
			return &arr[index];
		}
		default: return &None;
	}
}

const Json::JToken* Find(const Json::JObject* root, const std::vector<std::string_view>& path)
{
	if (path.empty())
		return &None;
	auto current = FromObject(root, path[0]);
	if (current->IsNull())
		return &None;
	for(size_t i = 1; i < path.size(); i++)
	{
		current = Step(current, path[i]);
	}
	return current;
}

bool CustomAttributesSearch::TryGetInt(const std::vector<std::string_view>& path, SInt32& out) const
{
	const auto found = Find(data, path);
	if (!found->IsInt()) // TODO: string value support??
		return false; 
	out = found->AsInt();
	return true;
}

bool CustomAttributesSearch::TryGetFloat(const std::vector<std::string_view>& path, float& out) const
{
	const auto found = Find(data, path);
	if (!found->IsFloat()) // TODO: string value support??
		return false;
	out = found->AsFloat();
	return true;
}

bool CustomAttributesSearch::TryGetString(const std::vector<std::string_view>& path, std::string& out) const
{
	const auto found = Find(data, path);
	if (!found->IsString())
		return false;
	out = found->AsString();
	return true;
}

bool CustomAttributesSearch::TryGetBool(const std::vector<std::string_view>& path, bool& out) const
{
	const auto found = Find(data, path);
	if (!found->IsBool()) // TODO: string value support??
		return false;
	out = found->AsBool();
	return true;
}

bool CustomAttributesSearch::TryGet(const std::vector<std::string_view>& path, std::unique_ptr<Json::JObject>& out) const
{
	const auto found = Find(data, path);
	if (!found->IsObject())
		return false;
	out = std::unique_ptr<Json::JObject>(found->AsObject()->Clone());
	return true;
}

bool CustomAttributesSearch::TryGetArray(const std::vector<std::string_view>& path, std::unique_ptr<Json::JArray>& out) const
{
	const auto found = Find(data, path);
	if (!found->IsArray())
		return false;
	out = std::unique_ptr<Json::JArray>(found->AsArray()->Clone());
	return true;
}

