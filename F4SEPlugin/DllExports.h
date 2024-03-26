#pragma once

#ifdef _DEBUG // methods exported for TestsConsole project
	#define DLLEXPORT __declspec( dllexport )
#else // nothing to export. TestsConsole project is disabled in release mode
	#define DLLEXPORT
#endif