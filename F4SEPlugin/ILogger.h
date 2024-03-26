#pragma once
#include "DllExports.h"

struct ILogger
{
	enum LogLevel
	{
		None = 0,
		Error = 1,
		Warning = 2,
		Info = 3,
		Debug = 4,
	};
	ILogger() = default;
	virtual ~ILogger() = default;

	DLLEXPORT static void Init(ILogger* logger);
	DLLEXPORT static void Log(const char * fmt, ...);

	DLLEXPORT static LogLevel Level;
	DLLEXPORT static LogLevel GetLevel();
	DLLEXPORT static void SetLevel(LogLevel level);

	DLLEXPORT static void Log(LogLevel level, const char * fmt, ...);

	DLLEXPORT static void LogDebug(const char* fmt, ...);
	DLLEXPORT static void LogInfo(const char* fmt, ...);
	DLLEXPORT static void LogWarning(const char* fmt, ...);
	DLLEXPORT static void LogError(const char* fmt, ...);

	DLLEXPORT static void Log(LogLevel level, const char * fmt, va_list args);

	DLLEXPORT static void LogDebug(const char* fmt, va_list args);
	DLLEXPORT static void LogInfo(const char* fmt, va_list args);
	DLLEXPORT static void LogWarning(const char* fmt, va_list args);
	DLLEXPORT static void LogError(const char* fmt, va_list args);

protected:
	virtual void LogImpl(const char * fmt, va_list args) = 0;
	DLLEXPORT virtual std::string LevelToString(LogLevel level);
};

// log/debug
void D(const char* fmt, ...);
// log/info
void I(const char* fmt, ...);
// log/warning
void W(const char* fmt, ...);
// log/error
void E(const char* fmt, ...);
// false/error
bool FE(const char* fmt, ...);
// false/warning
bool FW(const char* fmt, ...);
// false/debug
bool FD(const char* fmt, ...);
// false/info
bool FI(const char* fmt, ...);
// true/warning
bool TW(const char* fmt, ...);
// true/error
bool TE(const char* fmt, ...);
// true/debug
bool TD(const char* fmt, ...);

inline const char* S(bool val) {return val ? "true" : "false";}
