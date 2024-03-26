#include "ILogger.h"
#include <array>
#include <mutex>

std::mutex logImplCall;

ILogger::LogLevel ILogger::Level = ILogger::LogLevel::Warning;

ILogger* instance = nullptr;
void ILogger::Init(ILogger* logger)
{
	instance = logger;
}
void ILogger::Log(const char * fmt, ...)
{
	if (!instance)
		return;
	va_list args;
	va_start(args, fmt);
	std::lock_guard lock(logImplCall);
	instance->LogImpl(fmt, args);
	va_end(args);
}

ILogger::LogLevel ILogger::GetLevel()
{
	return Level;
}

void ILogger::SetLevel(LogLevel level)
{
	Level = level;
}


const std::array<std::string, 5> levelsMapping = 
{
	"",
	"ERROR:",
	"WARNING:",
	"INFO:",
	"DEBUG:"
};
std::string ILogger::LevelToString(LogLevel level)
{
	return levelsMapping[level];
}

#define TESTLEVEL(LogLevelValue)\
	if ((LogLevelValue) > ILogger::Level)\
		return

void ILogger::Log(LogLevel level, const char* fmt, ...)
{
	if (!instance)
		return;
	TESTLEVEL(level);
	const std::string format = instance->LevelToString(level) + fmt;
	va_list args;
	va_start(args, fmt); // fmt causes same args as format.c_str()
	std::lock_guard lock(logImplCall);
	instance->LogImpl(format.c_str(), args);
	va_end(args);
}
void ILogger::Log(LogLevel level, const char* fmt, va_list args)
{
	if (!instance)
		return;
	TESTLEVEL(level);
	const std::string format = instance->LevelToString(level) + fmt;
	std::lock_guard lock(logImplCall);
	instance->LogImpl(format.c_str(), args);
}


#define LOGVARIADIC(LogFunctionName)\
	va_list args;\
	va_start(args, fmt);\
	LogFunctionName(fmt, args);\
	va_end(args)

void ILogger::LogDebug(const char* fmt, ...)
{
	TESTLEVEL(Debug);
	LOGVARIADIC(LogDebug);
}

void ILogger::LogInfo(const char* fmt, ...)
{
	TESTLEVEL(Info);
	LOGVARIADIC(LogInfo);
}

void ILogger::LogWarning(const char* fmt, ...)
{
	TESTLEVEL(Warning);
	LOGVARIADIC(LogWarning);
}

void ILogger::LogError(const char* fmt, ...)
{
	TESTLEVEL(Error);
	LOGVARIADIC(LogError);
}


void ILogger::LogDebug(const char* fmt, va_list args)
{
	Log(Debug, fmt, args);
}

void ILogger::LogInfo(const char* fmt, va_list args)
{
	Log(Info, fmt, args);
}

void ILogger::LogWarning(const char* fmt, va_list args)
{
	Log(Warning, fmt, args);
}

void ILogger::LogError(const char* fmt, va_list args)
{
	Log(Error, fmt, args);
}


void D(const char* fmt, ...)
{
	TESTLEVEL(ILogger::LogLevel::Debug);
	LOGVARIADIC(ILogger::LogDebug);
}

void I(const char* fmt, ...)
{
	TESTLEVEL(ILogger::LogLevel::Info);
	LOGVARIADIC(ILogger::LogInfo);
}

void W(const char* fmt, ...)
{
	TESTLEVEL(ILogger::LogLevel::Warning);
	LOGVARIADIC(ILogger::LogWarning);
}

void E(const char* fmt, ...)
{
	TESTLEVEL(ILogger::LogLevel::Error);
	LOGVARIADIC(ILogger::LogError);
}

bool FE(const char* fmt, ...)
{
	TESTLEVEL(ILogger::LogLevel::Error) false;
	LOGVARIADIC(ILogger::LogError);
	return false;
}

bool FW(const char* fmt, ...)
{
	TESTLEVEL(ILogger::LogLevel::Warning) false;
	LOGVARIADIC(ILogger::LogWarning);
	return false;
}

bool FD(const char* fmt, ...)
{
	TESTLEVEL(ILogger::LogLevel::Debug) false;
	LOGVARIADIC(ILogger::LogDebug);
	return false;
}

bool FI(const char* fmt, ...)
{
	TESTLEVEL(ILogger::LogLevel::Info) false;
	LOGVARIADIC(ILogger::LogInfo);
	return false;
}

bool TW(const char* fmt, ...)
{
	TESTLEVEL(ILogger::LogLevel::Warning) true;
	LOGVARIADIC(ILogger::LogWarning);
	return true;
}

bool TE(const char* fmt, ...)
{
	TESTLEVEL(ILogger::LogLevel::Error) true;
	LOGVARIADIC(ILogger::LogError);
	return true;
}

bool TD(const char* fmt, ...)
{
	TESTLEVEL(ILogger::LogLevel::Debug) true;
	LOGVARIADIC(ILogger::LogDebug);
	return true;
}
