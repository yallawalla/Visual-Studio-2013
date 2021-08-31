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

float		ro = 100;				// rotacija
Vector3D	ar = { 0, 0, 0 };		// yaw
Vector3D	*yarr;

Vector3D df(float t, Vector3D y) {
	float yabs = y.GetMagnitude();
	Vector3D g = { 0, -9.8f, 0 };
	Vector3D dv = -y*y.GetMagnitude()*0.001 + ar*0.1 + g;
	ar = y.GetCross(dv) / yabs / yabs*ro;
	return dv;
}

extern "C"
{
	__declspec(dllexport) void rk4open(int n) {
		yarr = (Vector3D *)malloc(n*sizeof(Vector3D));
	}
	__declspec(dllexport) void rk4close(void) {
		free(yarr);
	}
	__declspec(dllexport) float rk4x(int n) {
		return yarr[n].x;
	}
	__declspec(dllexport) float rk4y(int n) {
		return yarr[n].y;
	}
	__declspec(dllexport) float rk4z(int n) {
		return yarr[n].z;
	}
	__declspec(dllexport) int rk4(float xo, float yo, float zo, int n) {
			float		t, h;
			Vector3D	*y, k1, k2, k3, k4;

			y = yarr;
			y->x = xo;
			y->y = yo;
			y->z = zo;
			h = 0.2f;
			t = 0;

			for (int i = 0; i < n - 1; ++i) {
				k1 = h * df(t, y);
				k2 = h * df(t + h / 2, *y + k1 / 2);
				k3 = h * df(t + h / 2, *y + k2 / 2);
				k4 = h * df(t + h / 2, *y + k2 / 2);

				*(y+1) = *y + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
				t += h;
				++y;
			}
			return 0;
	}
}
