#pragma once
#include "CustomAttributes.h"

namespace Data
{
	struct CustomAttributesSearch final
	{
		CustomAttributesSearch(const Json::JObject* data): data(data){}
		~CustomAttributesSearch(){data = nullptr;}
		bool TryGetInt(const std::vector<std::string_view>& path, SInt32& out) const;
		bool TryGetFloat(const std::vector<std::string_view>& path, float& out) const;
		bool TryGetString(const std::vector<std::string_view>& path, std::string& out) const;
		bool TryGetBool(const std::vector<std::string_view>& path, bool& out) const;
		bool TryGet(const std::vector<std::string_view>& path, std::unique_ptr<Json::JObject>& out) const;
		bool TryGetArray(const std::vector<std::string_view>& path, std::unique_ptr<Json::JArray>& out) const;
	private:
		const Json::JObject* data;
	};
}