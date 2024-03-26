#include "PapyrusStructHelper.h"
#include "f4se/PapyrusVM.h"
#include "ILogger.h"

VMValue PapyrusStructHelper::CreateAndInitializeInstance(BSFixedString structName,
	const std::function<VMValue(UInt64 fieldType, BSFixedString fieldName)>& initializer)
{
	VMStructTypeInfo* structTypeInfo = nullptr;
	VirtualMachine* vm = (*g_gameVM)->m_virtualMachine;
	if (vm->GetStructTypeInfo(&structName, &structTypeInfo))
	{
		if (structTypeInfo)
			structTypeInfo->Release();
		else
		{
			W("Returned no information for struct %s", structName.c_str());
			return VMValue();
		}
	}
	else
	{
		W("Failed to get information for struct %s", structName.c_str());
		return VMValue();
	}

	VMValue retVal;
	
	VMValue::StructData* structObject = nullptr;
	if (vm->CreateStruct(&structName, &structObject))
	{
		structTypeInfo->m_members.ForEach([&structObject, &structTypeInfo, &initializer](const VMStructTypeInfo::MemberItem* fInfo)
		{
			structObject->GetStruct()[fInfo->index] = initializer(structTypeInfo->m_data.entries[fInfo->index].m_type, fInfo->name);
			return true;
		});
	}
	else
	{
		E("Failed to create structure %s object", structName.c_str());
		return VMValue();
	}

	retVal.SetComplexType(structTypeInfo);
	retVal.data.strct = structObject;

	return retVal;
}

VMValue PapyrusStructHelper::DefaultFieldValue(UInt64 type)
{
	switch (type)
	{
		case VMValue::kType_None: return VMValue(); // None
		case VMValue::kType_String: {VMValue r; r.SetString(""); return r;}
		case VMValue::kType_Int: {VMValue r; r.SetInt(0); return r;}
		case VMValue::kType_Float: {VMValue r; r.SetFloat(0); return r;}
		case VMValue::kType_Bool: {VMValue r; r.SetBool(false); return r;}
	}
	return VMValue(); // None
}
