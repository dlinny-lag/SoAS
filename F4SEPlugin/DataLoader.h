#pragma once
#include "DataHolder.h"
#include <functional>

struct DataLoader final
{
	static bool StartLoading(std::function<void(bool)> onFinish);
	static bool IsInProgress();
	static bool HasData();
	static bool IsSucceed();
	static bool IsFinished();
	static bool GetResult(DataHolder& result);
	static void WaitForComplete();
};

