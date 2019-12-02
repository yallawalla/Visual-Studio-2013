// v41.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

//int _tmain(int argc, _TCHAR* argv[])
//{
//	return 0;
//}

extern "C" void GetDisplay(char*);
extern "C" void main(void);
extern "C" void IRQservice(void);
extern "C" void ButtonPress(int);