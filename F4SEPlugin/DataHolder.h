#pragma once
#include "DllExports.h"
#include <string>
#include <unordered_map>
#include <ostream>
#include "Scene.h"

class DataHolder final
{
	std::unordered_map<std::string, Data::Scene> scenes;
public:
	DataHolder() = default;
	~DataHolder() = default;
	DataHolder(DataHolder&& other) = default;
	DataHolder(const DataHolder& other) = delete;
	DataHolder& operator=(DataHolder&& other) = default;
	DataHolder& operator=(const DataHolder& other) = delete;

	bool Deserialize(const std::string& filePath, std::vector<std::string>& errors);
	[[nodiscard]] size_t Size() const {return scenes.size();}
	DLLEXPORT void Dump(std::ostream& stream) const;

	DLLEXPORT const Data::Scene* Find(const char* sceneId) const;
};

