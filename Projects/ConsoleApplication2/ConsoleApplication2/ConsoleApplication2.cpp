// ConsoleApplication2.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <string.h>
#include <stdio.h>
#include <conio.h>

char	*x(char *p) {
	*p = '-';
	if (p[2] != '_') {
		p[0]=p[2] = '.';
		*x(p + 1) = '-';
	}
	if (p[-2] != '_') {
		p[0] = p[-2] = '.';
		*x(p - 1) = '-';
	}
	return p;
}

char c[] = "_____________________________________________________________________________________________________________";

int _tmain(int argc, _TCHAR* argv[])
{
	int i;
	do {
		i = _getch();
		if (i >= '0' && i <= '9') {
			i -= 0x30;
			x(c + strlen(c) / 2 + i);
			printf("\r%s", c);
		}
	} while (i  != ' ');
	return 0;
}
,�
