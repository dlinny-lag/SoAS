// TestsConsole.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
// I'm too lazy, so just duplicate
typedef unsigned long		UInt32;
typedef signed long			SInt32;

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
    auto scene = content.Find("SE Atomic Blowjob");

    {
		const auto data = std::unique_ptr<Json::JObject>(scene->Custom.GetData());
        
        const Data::CustomAttributesSearch searcher(data.get());
        SInt32 intVal;
        const std::string propName = SU::ToUpper("IntVal");
        const bool result = searcher.TryGetInt(std::vector<std::string_view>{propName}, intVal);
        if (result)
            ILogger::Log("IntValue=%d", intVal);
        else
            ILogger::Log("IntValue failed");
    }
    scene = nullptr;
    DataLoader::StartLoading(OnDataLoad);
    DataLoader::WaitForComplete();
    DataLoader::GetResult(content);
    scene = content.Find("Gray Spanking 02 Staged");
    for(const Data::ActorsContact& contact : scene->ActorsContacts)
    {
        bool isValid;
	    const bool isLeftHand = ContactFlag::Is(contact.From, "Is_Left_Hand", isValid);
        ILogger::Log("IsLeftHand=%s", S(isLeftHand));

    	const bool isLeftArm = ContactFlag::Is(contact.From, "Is_Left_Arm", isValid);
        ILogger::Log("IsLeftArm=%s", S(isLeftArm));
    }
}

// see https://stackoverflow.com/questions/35310117/debug-assertion-failed-expression-acrt-first-block-header

