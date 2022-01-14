// emul1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <conio.h>
#include <stdio.h>



#define	nBits	9
#define	nStop	2
#define	nBaud	10


/*

-----_-____--_-------------_-_-__--__---------_-____--__-----------
---------------------_---------------------------------------

*/

void tim9Callb1(int ic) {

	static int to, bit, dat, cnt;
	static int rxtout;
	
	if (to) {
		int n = ((ic - to) % 0x10000 + nBaud/2) / nBaud;
		while (n-- && cnt <= nBits + nStop) {
			dat = (dat | bit) >> 1;
			++cnt;
		}
		bit ^= 1 << (nBits + nStop);
		if (cnt > nBits + nStop) {
			printf("%04X ", dat & 0xfff);
			dat = cnt = 0;
		}
	}
	else 
		bit = dat = 0;
	to = ic;
	}



int buff[128] = { 0 }, *p = buff, ch;
size_t	cread;
char c[128];

int _tmain(int argc, _TCHAR* argv[])
{
	static int to, bit, dat, cnt;
	static int ic = 2000;

	while(1) {
		_cgets_s(c,sizeof(c),&cread);
		for (char *p = c; p[1]; ++p) {
			if (p[0] != p[1]) 
				tim9Callb1(ic);
			ic += nBaud;
			}
		}
	return(0);
}

