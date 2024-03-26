#pragma once
#include "CustomAttributes.h"
#include "f4se/GameTypes.h"
#include "f4se/PapyrusValue.h"

namespace Data
{
	struct CustomAttributesSearch : public CustomAttributes
	{
		bool TryGetInt(const BSFixedString& path, SInt32& out);
		bool TryGetFloat(const BSFixedString& path, float& out);
		bool TryGetString(const BSFixedString& path, BSFixedString& out);
		bool TryGet(const BSFixedString& path, VMValue& out);
		bool TryGetStruct(const BSFixedString& path, VMStructTypeInfo* type, VMValue& out);
	};
}