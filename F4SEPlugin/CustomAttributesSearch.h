#pragma once
#include "CustomAttributes.h"
#include "DllExports.h"

namespace Data
{
	struct CustomAttributesSearch final
	{
		DLLEXPORT CustomAttributesSearch(const Json::JObject* data): data(data){}
		DLLEXPORT ~CustomAttributesSearch(){data = nullptr;}
		DLLEXPORT bool TryGetInt(const std::vector<std::string_view>& path, SInt32& out) const;
		DLLEXPORT bool TryGetFloat(const std::vector<std::string_view>& path, float& out) const;
		DLLEXPORT bool TryGetString(const std::vector<std::string_view>& path, std::string& out) const;
		DLLEXPORT bool TryGetBool(const std::vector<std::string_view>& path, bool& out) const;
		DLLEXPORT bool TryGet(const std::vector<std::string_view>& path, std::unique_ptr<Json::JObject>& out) const;
		DLLEXPORT bool TryGetArray(const std::vector<std::string_view>& path, std::unique_ptr<Json::JArray>& out) const;
	private:
		const Json::JObject* data;
	};
}