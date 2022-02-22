// ConsoleApplication5.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"


int _tmain(int argc, _TCHAR* argv[])
{
	int n, i;
	float x, y, m, c, d;
	float sumx = 0, sumxsq = 0, sumy = 0, sumxy = 0;
	printf("enter the number of values for n:");
	scanf_s("%d", &n);
	for (i = 0; i<n; i++){
		printf("enter values of x and y");
		scanf_s("%f%f", &x, &y);
		sumx = sumx + x;
		sumxsq = sumxsq + (x*x);
		sumy = sumy + y;
		sumxy = sumxy + (x*y);
	}
	d = n*sumxsq - sumx*sumx;
	if (!d)
		d = 1;
	m = (n*sumxy - sumx*sumy) / d;
	c = (sumy*sumxsq - sumx*sumxy) / d;
	printf("M=%f\tC=%f\n", m, c);
	return 0;
}

