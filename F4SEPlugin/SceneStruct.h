#pragma once
#include "Scene.h"
#include "PapyrusStructHelper.h"

struct SceneStruct
{
	static constexpr const char* const Prefix = "CUSTOM";
	static constexpr size_t PrefixLength = std::string::traits_type::length(Prefix);
	static VMValue Create(const Json::JObject* data, BSFixedString structName);
};

