#pragma once
#include "ILogger.h"

struct F4SELogger final : ILogger
{
	F4SELogger();
	void LogImpl(const char * fmt, va_list args) override;
	bool LogToConsole;
};

extern F4SELogger g_f4seLogger;

