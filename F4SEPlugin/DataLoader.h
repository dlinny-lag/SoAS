#pragma once
#include "DllExports.h"
#include "DataHolder.h"
#include <functional>

struct DataLoader final
{
	DLLEXPORT static bool StartLoading(std::function<void(bool)> onFinish);
	static bool IsInProgress();
	static bool HasData();
	DLLEXPORT static bool IsSucceed();
	DLLEXPORT static bool IsFinished();
	DLLEXPORT static bool GetResult(DataHolder& result);
	DLLEXPORT static void WaitForComplete();
};

