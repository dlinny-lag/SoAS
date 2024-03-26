#include "F4SELogger.h"
#include "f4se/GameAPI.h"

F4SELogger g_f4seLogger;

void F4SELogger::LogImpl(const char * fmt, va_list args)
{
	gLog.FormattedMessage(fmt, args);
	if (LogToConsole)
	{
		if (ConsoleManager* mgr = *g_console)
		{
			CALL_MEMBER_FN(mgr, VPrint)(fmt, args);
		}
	}
}

F4SELogger::F4SELogger() : LogToConsole(false)
{

}
