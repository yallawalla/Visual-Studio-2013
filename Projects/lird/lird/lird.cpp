// lird.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

void printb(int b, int n) {
	while (n--) {
		if (b & (1 << n))
			printf("1");
		else
			printf("0");
	}
}

char	tab[4096];

int _tmain(int argc, _TCHAR* argv[])
{
	int ii = 0, odd = 1,even=3;
	for	(int k = 0; k < 12; ++k) {
		printb(1<<ii, 24); printf("   "); printb(odd, 12); printf("%5d,%3d\r\n", odd, ii);
		printb(1 << (ii+1), 24); printf("   "); printb(even, 12); printf("%5d,%3d\r\n", even, ii+1);
		tab[odd] = ii++;
		tab[even] = ii++;
		odd *= 2;
		if (odd > 0xfff) odd = (odd & 0xfff) + (odd >> 12);
		even *= 2;
		if (even > 0xfff) even = (even & 0xfff) + (even >> 12);
	}
	return 0;
}

