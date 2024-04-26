// TestsConsole.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
#include <iostream>
#include "ContactFlag.hpp"
#include "StringUtils.hpp"
#include "DataLoader.h"
#include "ILogger.h"
#include "CustomAttributesSearch.h"

struct ConsoleLogger : ILogger
{
	void LogImpl(const char * fmt, va_list args) override
	{
		vprintf(fmt, args);
        printf("\n");
	}
};

void OnDataLoad(bool result)
{
	ILogger::Log("Data loaded with result %s", S(result));
}

void PrintStringViews(const std::vector<std::string_view>& list)
{
	ILogger::Log(SU::Join(list, ",").c_str());
}

int main()
{
    ConsoleLogger logger;
    ILogger::Init(&logger);
    ILogger::SetLevel(ILogger::LogLevel::Debug);

    const bool result = DataLoader::StartLoading(OnDataLoad);
    if (!result)
    {
        ILogger::Log("Failed to start");
        return 0;
    }

//   PrintStringViews(SU::Split("Some_Value_", "_"));
//   PrintStringViews(SU::Split("Some_Value_", "_", false));
//   PrintStringViews(SU::Split("Some_Value", "_"));
//   PrintStringViews(SU::Split("Some_Value", "_", false));
//   PrintStringViews(SU::Split("_Some_Value", "_"));
//   PrintStringViews(SU::Split("_Some_Value", "_", false));
//   PrintStringViews(SU::Split("SomeValue", "_"));
//   PrintStringViews(SU::Split("SomeValue", "_", false));
//   PrintStringViews(SU::Split("SomeValue_", "_"));
//   PrintStringViews(SU::Split("SomeValue_", "_", false));
//   PrintStringViews(SU::Split("_SomeValue_", "_"));
//   PrintStringViews(SU::Split("_SomeValue_", "_", false));
//   PrintStringViews(SU::Split("_SomeValue", "_"));
//   PrintStringViews(SU::Split("_SomeValue", "_", false));

    DataLoader::WaitForComplete();
    if (!DataLoader::IsSucceed())
    {
        ILogger::Log("Result is not succeed");
        return 0;
    }
    DataHolder content;
    DataLoader::GetResult(content);
    content.Dump(std::cout);
    auto scene = content.Find("MyTestsScene2");

    {
		const auto data = std::unique_ptr<Json::JObject>(scene->Custom.GetData());
        
        const Data::CustomAttributesSearch searcher(data.get());
        SInt32 intVal;
        std::string propName = SU::ToUpper("IntVal");
        bool result = searcher.TryGetInt(std::vector<std::string_view>{propName}, intVal);
        if (result)
            ILogger::Log("IntValue=%d", intVal);
        else
            ILogger::Log("IntValue failed");
        propName = SU::ToUpper("ArrayVal");
        std::unique_ptr<Json::JArray> arr;
        result = searcher.TryGetArray(std::vector<std::string_view>{propName}, arr);
        if (result)
            ILogger::Log("Array %s", arr->to_string().c_str());
        else
            ILogger::Log("Array failed");
    }
    scene = nullptr;
    DataLoader::StartLoading(OnDataLoad);
    DataLoader::WaitForComplete();
    DataLoader::GetResult(content);
    scene = content.Find("MyTestsScene");
    if (!scene)
        return 0;
    for(const Data::ActorsContact& contact : scene->ActorsContacts)
    {
        bool isValid;
	    const bool isLeftHand = ContactFlag::Is(contact.From, "Is_Left_Hand", isValid);
        ILogger::Log("IsLeftHand=%s", S(isLeftHand));

    	const bool isLeftArm = ContactFlag::Is(contact.From, "Is_Left_Arm", isValid);
        ILogger::Log("IsLeftArm=%s", S(isLeftArm));
    }

    const auto sceneData = scene->Custom.GetData();
    const Data::CustomAttributesSearch searcher(sceneData.get());

    const std::string StringVal1 = SU::ToUpper("StringVal1");
    std::vector<std::string_view> splitted = SU::Split(StringVal1.c_str(), "_");
    std::string str;
    if (searcher.TryGetString(splitted, str))
		ILogger::Log("%s = %s", StringVal1.c_str(), str.c_str());
    else
        ILogger::Log("Failed to get string value of %s", StringVal1.c_str());

    const std::string ObjVal1_IntVal2 = SU::ToUpper("ObjVal1_IntVal2");
    splitted = SU::Split(ObjVal1_IntVal2.c_str(), "_");
    SInt32 intVal;
    if (searcher.TryGetInt(splitted, intVal))
        ILogger::Log("%s = %d", ObjVal1_IntVal2.c_str(), intVal);
    else
        ILogger::Log("Failed to get int value of %s", ObjVal1_IntVal2.c_str());


    const std::string ObjVal1_ArrayVal2_0 = SU::ToUpper("ObjVal1_ArrayVal2_0");
    splitted = SU::Split(ObjVal1_ArrayVal2_0.c_str(), "_");
    if (searcher.TryGetString(splitted, str))
		ILogger::Log("%s = %s", ObjVal1_ArrayVal2_0.c_str(), str.c_str());
    else
        ILogger::Log("Failed to get string value of %s", ObjVal1_ArrayVal2_0.c_str());

    const std::string ObjVal1_ArrayVal2_1 = SU::ToUpper("ObjVal1_ArrayVal2_1");
    splitted = SU::Split(ObjVal1_ArrayVal2_1.c_str(), "_");
    if (searcher.TryGetString(splitted, str))
		ILogger::Log("%s = %s", ObjVal1_ArrayVal2_1.c_str(), str.c_str());
    else
        ILogger::Log("Failed to get string value of %s", ObjVal1_ArrayVal2_1.c_str());

    return 0;
}

// see https://stackoverflow.com/questions/35310117/debug-assertion-failed-expression-acrt-first-block-header

