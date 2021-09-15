// emul.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <conio.h>

#define nBytes	8
#define nStop	2
#define nBaud	100

int _tmain(int argc, _TCHAR* argv[])
{
	char c[32],co;
	int	t = 58917, n = 0, tn[200],data;
	gets_s(c);

	int	stopb = 0;
	for (int i = 0; i < nStop; ++i)
		stopb = (stopb << 1) | 1;

	co = 1;
	for (int i = 0; c[i]; ++i) {
		data = c[i] * 2 + (stopb * 2 << nBytes);
		for (int j = 0; j < nBytes + nStop+1; ++j) {
			if ((data & 1) != co) {
				co ^= 1;
				tn[n++] = t;
			}
			data /= 2;
			t += nBaud;
		}
	}


	co = data = 0;
	for (int i = 1; i < n; ++i) {
		int dt = (tn[i] - tn[i - 1] + nBaud / 2) / nBaud;
		for (int j = 0; j < dt; ++j)
			data = 2 * data + co;
		co ^= 1;
	}







	for (int i = 0; i < n; ++i)
		printf("%5d\r\n", tn[i]);

	printf("\r\n");
	return 0;
}

