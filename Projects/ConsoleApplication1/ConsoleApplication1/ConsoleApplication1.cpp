// ConsoleApplication1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdlib.h>


// https://www.geeksforgeeks.org/count-of-numbers-from-range-l-r-that-end-with-any-of-the-given-digits/
void gfk(int l, int r) {
	int n;
	if (l > r)
		return;
	for (n = 0; l <= r; ++l) {
		switch (l % 10) {
		case 2: case 3: case 9:
			++n;
		}
	}

	printf("%d, %d >> %d", l,r,n);

}

// https://www.geeksforgeeks.org/reverse-substrings-between-each-pair-of-parenthesis/
void revrs(char *c) {
	char *p, *q;
	if (!p) return;

	for (p = c; *p; ++p) {
		if (*p == '(')
			revrs(++p);
		if (*p == ')')
			revrs(++p);


	}

}

int _tmain(int argc, _TCHAR* argv[])
{
	gfk(11, 33);
	return 0;
}
