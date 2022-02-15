// ConsoleApplication4.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>

int n = 0;
struct C {
	explicit C(int) {}
	C(const C&) { ++n; }  // the copy constructor has a visible side effect
};                      // it modifies an object with static storage duration



int _tmain(int argc, _TCHAR* argv[])
{
	C c1(42);      // direct-initialization, calls C::C(int)
	C c2 = C(42);  // copy-initialization, calls C::C(const C&)

	std::cout << n << std::endl;  // prints 0 if the copy was elided, 1 otherwise

	return 0;
}

