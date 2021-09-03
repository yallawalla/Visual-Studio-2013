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


struct h2o {				  /* osnovni podatki atmosfere */
	double temp;              /* v stopinjah Celzija */
	double h;                 /* pritisk v mm Hg */
	double rv;                /* procent relativne vlage */
	double tau;               /* fiktivna temperatura */
	double em;                /* pritisk v zasicenem zraku */
	int err;                  /* parameter napake */
} h2o = { 20.0, 750.0, 60, 0, 0 };
struct  atm {          /* normalna atmosfera */
	double tau0;             /* temperatura na povrsini zemlje */
	double ha0;              /* pritisk na povrsini zemlje v mm Hg */
	double ro0;
	double a0;
	double ro0a0;
	double y;                /* visina nad povrsino zemlje */
	double tau;              /* temperatura */
	double hm;               /* pritisk v mm Hg */
	double ro;	    	    /* gostota */
	double a;    	        /* hitrost zvoka */
} atm;

double emji[] ={ 0.77, 0.85, 0.94, 1.03, 1.13, 1.24, 1.36, 1.49,
				1.63, 1.78, 1.95, 2.13, 2.32, 2.53, 2.76, 3.01,
				3.29, 3.57, 3.88, 4.22, 4.58, 4.90, 5.30, 5.70,
				6.10, 6.50, 7.00, 7.50, 8.10, 8.60, 9.20, 9.80,
				10.5, 11.2, 12.0, 12.8, 13.6, 14.5, 15.5, 16.5,
				17.5, 18.7, 19.8, 21.1, 22.4, 23.8, 25.2, 26.7,
				28.4, 30.0, 31.8, 33.7, 35.7, 37.7, 39.9, 42.2,
				44.6, 47.1, 49.7, 52.4, 55.3, 55.3 };

void	FiktT(struct h2o *p) {
	int i, ti;
	double temp;

	ti = (int)floor(p->temp);
	i = ti + 20;
	if (i<0)
		i = 0;
	if (i>60)
		i = 60;
	p->em = emji[i] + (p->temp - ti) * (emji[i + 1] - emji[i]);
	temp = 1.0 - (3.0 * p->rv * p->em) / (800.0 * p->h);
	p->tau = (273.0 + p->temp) / temp;
}
double	CD43(double m)
{
	static double a1[] = { -2.083333, 1.180556, -0.3125,
		0.038056, -0.001667, 0.157 };
	static double a2[] = { 613.333335, -233.33334, 39.0,
		-2.813333, 0.068667, 0.158 };
	static double a4[] = { -78.411093, 43.245043, 1.803950,
		-3.74780, 0.88697, 0.325 };
	static double a5[] = { 1829.806015, -426.848831, 42.480376,
		-1.984508, 0.015829, 0.385 };
	static double a6[] = { 0.030874, -0.117636, 0.209691,
		-0.151862, -0.05038, 0.385 };
	double cd, sm;
	int i;

	if (m < 0.6) {
		cd = 1.388889;
		sm = m;
		for (i = 0; i < 6; i++)
			cd = cd * sm + a1[i];
	}
	else if (m < 0.9) {
		cd = -533.333337;
		sm = m - 0.6;
		for (i = 0; i < 6; i++)
			cd = cd * sm + a2[i];
	}
	else if (m < 1.0) {
		cd = -1.025 + 1.35 * m;
	}
	else if (m < 1.2) {
		cd = -445.15561;
		sm = m - 1.0;
		for (i = 0; i < 6; i++)
			cd = cd * sm + a4[i];
	}
	else if (m < 1.4) {
		cd = -2831.379493;
		sm = m - 1.2;
		for (i = 0; i < 6; i++)
			cd = cd * sm + a5[i];
	}
	else if (m < 3.56) {
		cd = -0.003049;
		sm = m - 1.2;
		for (i = 0; i < 6; i++)
			cd = cd * sm + a6[i];
	}
	else {
		cd = 0.260;
	}
	return cd;
}
double	FunkE(double u, double p, double y, struct Atmosfera *pp)
{
	double x, m, pom, v;
	/* stetje - zacasno		*/
	v = u * sqrt(1 + p * p);
	x = (0.006328 * y) / atm.tau0;   /* pomozna sprem.		*/
	if (x < 0.14) {                  /* (1-x)**0.5			*/
		m = 0.999997 - x * (0.499309 + 0.139021 * x);
	}
	else {
		m = 0.999306 - x * (0.489492 + 0.174513 * x);
	}
	m = v / (atm.a0 * m);            /* Machovo stevilo		*/
	pom = CD43(m);
	pom = pom * m * atm.ro0a0;
	if (x <= 0.05) {                 /* Odvisnost od visine	*/
		m = 1.000017 + x*(-4.891948 + x*8.955130);
	}
	else if (x <= 0.1) {
		m = 0.996803 + x*(-4.762684 + x*7.624088);
	}
	else if (x <= 0.15) {
		m = 0.985486 + x*(-4.536655 + x*6.489576);
	}
	else if (x <= 0.2) {
		m = 0.962657 + x*(-4.232153 + x*5.471807);
	}
	else {
		m = 0.923605 + x*(-3.843824 + x*4.505014);
	}
	return (pom * m);
}
void	AtmVent(double y, struct atm *p)
{
	p->y = y;
	p->tau = p->tau0 - 0.006328 * y;
	p->hm = p->ha0 * pow(p->tau / p->tau0, 5.4);
	p->a = 20.0484413 * sqrt(p->tau);
	p->ro = 0.4643761 * p->hm / p->tau;
}
void	Atm(double y, struct atm *p)
{
	p->y = y;
	p->tau = 288.16 - 0.0065 * y;
	p->hm = 760.0 * pow(1.0 - 0.00002256 * y, 5.2561);
	p->a = 20.0484413 * sqrt(p->tau);
	p->ro = 0.4645673 * p->hm / p->tau;
}
const int	K = 2;
float		ro = 0;											// rotacija
Vector3D	ar = { 0, 0, 0 };								// yaw
Vector3D	*yarr;
Vector3D	g = { 0, -9.8f, 0 };

void df(float t, Vector3D y[], Vector3D dy[]) {
	float yabs = y[1].GetMagnitude();						// |v|
	dy[0] = y[1];
	dy[1] = -y[1] * yabs*0.00019f + ar*0.001f + g;
	ar = y[1].GetCross(y[0]) / yabs / yabs*ro;				// V x dV/dt / |V|*|V|
}

extern "C" {
__declspec(dllexport) void rk4open(int n) {
		yarr = (Vector3D *)malloc(K*n*sizeof(Vector3D));
		Atm(0, &atm);
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
		float		t=0, h=0.2f;
		Vector3D	y[K], dy[K], k1[K], k2[K], k3[K], k4[K];
		Vector3D	*yp = yarr;
		yp[0] = { 0, 0, 0 };
		yp[1] = { vx,vy,vz };

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
			if (yp[0].y > 0 && yp[2].y < 0) {
				float r = yp[0].x;
				float tt = t;
			}
			++yp; ++yp;
			t += h;
		}
		return 0;
	}
}
