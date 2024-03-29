#pragma once
#include <string>
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

	static void Init(ILogger* logger);
	static void Log(const char * fmt, ...);

	static LogLevel Level;
	static LogLevel GetLevel();
	static void SetLevel(LogLevel level);

	static void Log(LogLevel level, const char * fmt, ...);

	static void LogDebug(const char* fmt, ...);
	static void LogInfo(const char* fmt, ...);
	static void LogWarning(const char* fmt, ...);
	static void LogError(const char* fmt, ...);

	static void Log(LogLevel level, const char * fmt, va_list args);

	static void LogDebug(const char* fmt, va_list args);
	static void LogInfo(const char* fmt, va_list args);
	static void LogWarning(const char* fmt, va_list args);
	static void LogError(const char* fmt, va_list args);

protected:
	virtual void LogImpl(const char * fmt, va_list args) = 0;
	virtual std::string LevelToString(LogLevel level);
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
