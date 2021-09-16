// emul.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <conio.h>
#include <stdlib.h>
#include <time.h> 

#define nBits	10
#define nStop	2
#define nBaud	1000

int _tmain(int argc, _TCHAR* argv[])
{
	char c[32];
	int	t, n = 0, tn[200], data, co, cnt;
	gets_s(c);

	srand(time(NULL));
	t = rand() % 0xffff;

	int	stopb = 0;
	for (int i = 0; i < nStop; ++i)
		stopb = (stopb << 1) | 1;

	co = 1;
	for (int i = 0; c[i]; ++i) {
		data = c[i] * 2 + (stopb * 2 << nBits);
		for (int j = 0; j < nBits + nStop+1; ++j) {
			if ((data & 1) != co) {
				co ^= 1;
				tn[n++] = t;
			}
			data /= 2;
			t += nBaud + rand() % nBaud/10 - nBaud / 10 / 2;
		}
	}



	co = ~((1 << nBits)-1);
	data = cnt = 0;
	for (int i = 1; i < n; ++i) {
		cnt += ((tn[i] - tn[i - 1] + nBaud / 2) / nBaud);
		if (cnt % (nBits + nStop + 1) == 0)
			printf("%c", data % ((1 << nBits) - 1));
		data = data >> ((tn[i] - tn[i - 1] + nBaud / 2) / nBaud);
		data ^= co;
	}
	printf("%c", data % ((1 << nBits) - 1));
	printf("\r\n");
	while (!_kbhit());
	return 0;
}

