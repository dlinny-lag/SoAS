#include "SceneStruct.h"

#include "CustomAttributesSearch.h"
#include "StringUtils.hpp"

inline VMValue FromInt(SInt32 val)
{
	VMValue retVal;
	retVal.SetInt(val);
	return retVal;
}
inline VMValue FromFloat(float val)
{
	VMValue retVal;
	retVal.SetFloat(val);
	return retVal;
}
inline VMValue FromString(const std::string& val)
{
	VMValue retVal;
	retVal.SetString(val.c_str());
	return retVal;
}
inline VMValue FromBool(bool val)
{
	VMValue retVal;
	retVal.SetBool(val);
	return retVal;
}
VMValue SceneStruct::Create(const Json::JObject* data, BSFixedString structName)
{
	const auto initializer = [data](UInt64 fieldType, BSFixedString fieldName)
	{
		const auto normalizedName = SU::ToUpper(fieldName.c_str());
		if (SU::StartsFrom(normalizedName.c_str(), Prefix))
		{
			const std::vector<std::string_view> splitted = SU::Split(normalizedName.c_str() + PrefixLength, "_");
			const Data::CustomAttributesSearch searcher(data);
			switch (fieldType)
			{
				case VMValue::kType_Int:
				{
					SInt32 retVal;
					if (searcher.TryGetInt(splitted, retVal))
						return FromInt(retVal);
				}
				break;
				case VMValue::kType_Float:
				{
					float retVal;
					if (searcher.TryGetFloat(splitted, retVal))
						return FromFloat(retVal);
				}
				break;
				case VMValue::kType_String:
				{
					std::string retVal;
					if (searcher.TryGetString(splitted, retVal))
						return FromString(retVal);
					std::unique_ptr<Json::JObject> obj;
					if (searcher.TryGet(splitted, obj))
						return FromString(obj->to_string());
					std::unique_ptr<Json::JArray> arr;
					if (searcher.TryGetArray(splitted, arr))
						return FromString(arr->to_string());
				}
				break;
				case VMValue::kType_Bool:
				{
					bool retVal;
					if (searcher.TryGetBool(splitted, retVal))
						return FromBool(retVal);
				}
				break;
			}
		}
		return PapyrusStructHelper::DefaultFieldValue(fieldType);
	};
	return PapyrusStructHelper::CreateAndInitializeInstance(structName, initializer);
}
