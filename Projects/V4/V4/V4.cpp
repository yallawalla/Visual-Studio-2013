// V4.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <Windows.h>

extern "C"	void main(void);
extern "C"	void IRQservice(void);
extern "C"	void ButtonPress(int);

extern "C"	void (__stdcall *fcallback)();

void CALLBACK WaitOrTimerCallback(PVOID lpParameter, BOOLEAN TimerOrWaitFired)
{
	IRQservice();
}

extern "C" __declspec(dllexport) void _dllmain(void)
{
	HANDLE hNewTimer;
	CreateTimerQueueTimer(&hNewTimer, NULL, WaitOrTimerCallback, NULL, 20, 20, WT_EXECUTELONGFUNCTION);
	main();
}

extern "C" __declspec(dllexport) void _dllButtonPress(int n)
{
	ButtonPress(n);
}

extern "C" __declspec(dllexport) void _dllUseCallback(void (__stdcall *f)())
{
	fcallback=f;
}


