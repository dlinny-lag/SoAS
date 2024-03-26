#pragma once
#include "PapyrusStructHelper.h"
#include "Contact.h"
#include "f4se/GameReferences.h"


struct ContactStruct
{
	static void ResetCache();
	static VMValue Create(const Data::ActorsContact& actorsContact, BSFixedString structName, const std::vector<Actor*>& actors);
	static VMValue Create(const Data::EnvironmentContact& envContact, BSFixedString structName, const std::vector<Actor*>& actors);
};
