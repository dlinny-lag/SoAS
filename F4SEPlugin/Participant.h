#pragma once
#include <string>
#include "Sex.h"

namespace Data
{
	struct Participant
	{
		std::string Skeleton;
		Sex Sex;
		bool IsAggressor;
		bool IsVictim;
		// TODO: custom attributes
	};
}
