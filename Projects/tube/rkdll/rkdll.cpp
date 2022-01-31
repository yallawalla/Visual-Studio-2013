// rkdll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <conio.h>
#include "mathlib3d-cpp-alpha-0.1/src/Vector3D.h"

#define pi 3.14159265359
const int	K = 2;
float		rot = 200;												// rotacija
Vector3D	*yarr;

float q = 1.60217662e-19;
float m = 9.10938356e-21;
Vector3D B = { 0, 1, 0 };
Vector3D E = { 0, 0, 0 };

void df(float t, Vector3D y[], Vector3D dy[]) {
		dy[1] = q / m*(E + y[1].GetCross(B));
		dy[0] = y[1];
}

extern "C" {
__declspec(dllexport) void rk4open(int n) {
		yarr = (Vector3D *)malloc(K*n*sizeof(Vector3D));
	}

__declspec(dllexport) void rk4close(void) {
		free(yarr);
	}
__declspec(dllexport) float rk4x(int n) {
		return yarr[K*n].x;
	}
__declspec(dllexport) float rk4y(int n) {
		return yarr[K*n].y;
	}
__declspec(dllexport) float rk4z(int n) {
		return yarr[K*n].z;
	}
__declspec(dllexport) int rk4(float vx, float vy, float vz, int n) {
		float		t=0, h=1e-3;
		int			ret = EOF;
		Vector3D	y[K], dy[K], k1[K], k2[K], k3[K], k4[K];
		Vector3D	*yp = yarr;
		yp[0] = { 0, 0, 0 };
		yp[1] = { vx, vy, vz };

		for (int i = 0; i < n - 2; ++i) {
			df(t, yp, dy);						// k1
			for (int j = 0; j < K; ++j)
			{
				k1[j] = h*dy[j];
				y[j] = yp[j] + k1[j] / 2.0;
			}
			df(t+h/2, y, dy);					// k2
			for (int j = 0; j < K; ++j)
			{
				k2[j] = h*dy[j];
				y[j] = yp[j] + k2[j] / 2.0;
			}
			df(t+h/2, y, dy);					// k2
			for (int j = 0; j < K; ++j)			// k3
			{
				k3[j] = h*dy[j];
				y[j] = yp[j] + k3[j];
			}
			df(t + h, y, dy);
			for (int j = 0; j < K; ++j)			// k4 and result      
			{
				k4[j] = h*dy[j];
				yp[j+2] = yp[j] + k1[j] / 6.0 + k2[j] / 3.0 + k3[j] / 3.0 + k4[j] / 6.0;
			}
			if (yp[0].y > 0 && yp[2].y < 0)
				ret = i;
			++yp; ++yp;
			t += h;
		}
		return ret;
	}
}
