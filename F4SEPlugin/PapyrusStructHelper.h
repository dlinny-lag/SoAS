#pragma once
#include "f4se/PapyrusValue.h"
#include <functional>

struct PapyrusStructHelper
{
	static VMValue CreateAndInitializeInstance(BSFixedString structName, const std::function<VMValue(UInt64 fieldType, BSFixedString fieldName)>& initializer);
	static VMValue DefaultFieldValue(UInt64 type);
};

