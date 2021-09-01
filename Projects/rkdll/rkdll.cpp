// rkdll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"


// test.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <conio.h>
#include "mathlib3d-cpp-alpha-0.1/src/Vector3D.h"

const int	 K = 2;
float		ro = 100;				// rotacija
Vector3D	ar = { 0, 0, 0 };		// yaw
Vector3D	*yarr;
Vector3D	g = { 0, -9.8f, 0 };

void df(float t, Vector3D y[], Vector3D dy[]) {
	float yabs = y[1].GetMagnitude();						// |v|
	dy[0] = y[1];
	dy[1] = -y[1] * yabs*0.001 + ar*0.1 + g;
	ar = y[1].GetCross(y[0]) / yabs / yabs*ro;				// V x dV/dt / |V|*|V|
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
		float		t, h;
		Vector3D	y[K], dy[K], k1[K], k2[K], k3[K], k4[K];
		Vector3D	*yp = yarr;
		yp[0] = { 0, 0, 0 };
		yp[1] = { vx,vy,vz };
		h = 0.2f;
		t = 0;

		for (int i = 0; i < n - 2; ++i) {
			df(t, yp, dy);						//k1
			for (int j = 0; j < K; ++j)
			{
				k1[j] = h*dy[j];
				y[j] = yp[j] + k1[j] / 2.0;
			}
			df(t, yp, dy);						//k2
			for (int j = 0; j < K; ++j)
			{
				k2[j] = h*dy[j];
				y[j] = yp[j] + k2[j] / 2.0;
			}
			for (int j = 0; j < K; ++j)			//k3
			{
				k3[j] = h*dy[j];
				y[j] = yp[j] + k3[j];
			}
			df(t + h, y, dy);
			for (int j = 0; j < K; ++j)			//k4 and result      
			{
				k4[j] = h*dy[j];
				yp[j+2] = yp[j] + k1[j] / 6.0 + k2[j] / 3.0 + k3[j] / 3.0 + k4[j] / 6.0;
			}
			++yp; ++yp;
		}
		return 0;
	}
}
