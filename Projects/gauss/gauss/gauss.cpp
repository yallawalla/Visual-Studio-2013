// gauss.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <stdlib.h>
#include <math.h>
#include <cstdlib>
#include <ctime>
#define PI 3.141592654

#define NSUM 25

double gaussrand1()
{
	double x = 0;
	int i;
	for (i = 0; i < NSUM; i++)
		x += (double)rand() / RAND_MAX;

	x -= NSUM / 2.0;
	x /= sqrt(NSUM / 12.0);

	return x;
}


double gaussrand2()
{
	static double U, V;
	static int phase = 0;
	double Z;

	if (phase == 0) {
		U = (rand() + 1.) / (RAND_MAX + 2.);
		V = rand() / (RAND_MAX + 1.);
		Z = sqrt(-2 * log(U)) * sin(2 * PI * V);
	}
	else
		Z = sqrt(-2 * log(U)) * cos(2 * PI * V);

	phase = 1 - phase;

	return Z;
}

double gaussrand3()
{
	static double V1, V2, S;
	static int phase = 0;
	double X;

	if (phase == 0) {
		do {
			double U1 = (double)rand() / RAND_MAX;
			double U2 = (double)rand() / RAND_MAX;

			V1 = 2 * U1 - 1;
			V2 = 2 * U2 - 1;
			S = V1 * V1 + V2 * V2;
		} while (S >= 1 || S == 0);

		X = V1 * sqrt(-2 * log(S) / S);
	}
	else
		X = V2 * sqrt(-2 * log(S) / S);

	phase = 1 - phase;

	return X;
}
#define M 30
int _tmain(int argc, _TCHAR* argv[])
{
	
	srand(time(0));
	while (1) {
		int	n[M];
		int i=0,j=0;
		system("cls");
		for (i = 0; i < M; ++i)
			n[i] = 0;
		while (i++ < 100) {
			//printf("%3d ", 10*((int)(gaussrand1()*25.0+5.0)/10));
			//if (!(++i % 10))
			//	printf("\r\n");
			j = ((int)(gaussrand1()*25.0 + 5.0) / 10) + M / 2;
			++n[j];
		}
		for (i = 0; i < M; ++i, printf("\r\n%2d ", i * 10 - 10 * M / 2))
			for (j = 0; j < n[i]; ++j)
				printf("-");
		Sleep(100);

	}

	//printf("\r\n\r\n");
	//i = 0;
	//while (i < 100) {
	//	printf("%3d ", 10 * ((int)(gaussrand2()*25.0 + 5.0) / 10));
	//	if (!(++i % 10))
	//		printf("\r\n");
	//}

	//printf("\r\n\r\n");
	//i = 0;
	//while (i < 100) {
	//	printf("%3d ", 10 * ((int)(gaussrand3()*25.0 + 5.0) / 10));
	//	if (!(++i % 10))
	//		printf("\r\n");
	//}

	return 0;

}

