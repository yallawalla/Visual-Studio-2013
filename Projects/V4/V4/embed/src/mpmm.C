/*______________________________________________________________________________________________________________*/
#include "remmenu.h"
/*______________________________________________________________________________________________________________*/
#define		pi 3.14159265358979323846
enum		Pstates	{X,Y,Z,V,T,Th,Om,Qfi,Nstates,Xs,Ys,Zs,Ths,Maxstates};
enum		Pcoeff	{Cd,Cla,Cspin,Mass,Cal,Wx,Wz,Ehigh,Elow,Ncoeff};
/*______________________________________________________________________________________________________________*/
void		Emax(float *);
void		Coeff(float *);
void		Meteo(int *);
int			Trajectory(float *, float, int, double *, double *);
int			TrajectoryW(float *, float, int, double *, double *);
int			Angle(float *, float, float, double *, double *, double *, int);
samples		*sss;
/*______________________________________________________________________________________________________________*/
int Ballistic(lib *bt, lib *tg, lib *crest)
	{
	extern	float	cff[];
	char	cc[128];
	int		i,k;
	float	y[Maxstates];
	float	tgh,dh,fi;
	float	c[18];

	gun		*g		=&Guns[bt->gun];
	int		nmass	=(bt->ammpcs[CP->ammo])>>12;
	float	mv		=g->mv[CP->ammo][CP->charge & 0x7f];
	float	dvmass	=g->dvmass[CP->ammo][CP->charge & 0x7f];
	double	*dvtemp	=g->dvtemp[CP->ammo][CP->charge & 0x7f];
	double	*coeff	=g->coeff[CP->ammo][CP->charge & 0x7f];
	double	*cla	=g->drift[CP->ammo][CP->charge & 0x7f];
	double	*abc	=g->ABC[CP->ammo][CP->charge & 0x7f];
/*__zacetna pozicija ___________________________________________________________________________________________*/
	y[X] =0;
	y[Y] =bt->xc->z;
	y[Z] =0;
/*__izhodna hitrost ____________________________________________________________________________________________*/
	y[V]   = mv - dvmass * nmass;				/* vpliv spremembe mase											*/
	if(dvtemp)
		y[V] += polyp(bt->powder,dvtemp, 4);	/* vpliv temperature polnenja									*/
	y[V]  += y[V] * bt->xc->muzzle / 1000.0;	/* vpliv iztrosenosti cevi	?									*/

	y[T] =CP->tof = 0;
	y[Qfi] =y[V]/g->mcal/g->rifle*2*pi;
	y[Th] =y[Om]=0;

	dh=g->alt[CP->ammo][CP->fuze];				/* visina tempiranja											*/
	tgh= tg->xc->z + dh;						/* visinska razlika,+ tempiranje								*/
/*__vpis koeficientov___________________________________________________________________________________________*/
	c[Wx] = c[Wz] = meteo[WIND_S][0];
	c[Wx] *= cos(2.0 * M_PI * ((uint)meteo[WIND_D][0]/MR10 - tg->xc->x/(double)maxmils));
	c[Wz] *= sin(2.0 * M_PI * ((uint)meteo[WIND_D][0]/MR10 - tg->xc->x/(double)maxmils));

	c[Cspin]=0.01;
	c[Mass] =g->mass[CP->ammo];
	c[Mass] += g->dmass[CP->ammo]/c[Mass]*nmass;/* sprememba mase projektila									*/
	c[Cal]=g->mcal;
	c[Ehigh]=(float)g->hi/g->mils*pi*2.0f;
	c[Elow]=(float)g->lo/g->mils*pi*2.0f;
	Coeff(c);
/*______________________________________________________________________________________________________________*/
	if(!(k=Angle(y, tg->xc->y, tgh, coeff, cla, abc,CP->traj)))
	{
		fi = atan(y[Z] / y[X]);
		CP->tof = y[T] * 10.0 + 0.5;			
/* formatiranje azimuta	*/
		tg->xc->y=ScaleAngle((double)g->zeroA/g->mils + g->scaleA * (bt->xc->refp/MR10 - tg->xc->x/(double)maxmils + (fi)/2.0/M_PI),1.0,g->mils,null);
		if(g->gmode & BEAR_DBL)
			tg->xc->y=fmod(tg->xc->y,g->mils/2);
/* formatiranje elevacije do cilja */
		tg->xc->z  = g->zeroE + g->scaleE*ScaleAngle(atan(tgh - dh) / y[X],2.0*M_PI,g->mils,eof);
/* formatiranje elevacije + balisticnega kota */
		tg->xc->x  = g->zeroE + g->scaleE*ScaleAngle(y[Ths],2.0*M_PI,g->mils,eof);
		if(g->gmode & ELEV_DBL)
			tg->xc->x -= tg->xc->z;

		sss=init_samples(sss,3);
		add_sample(sss,0,0);
		add_sample(sss,cff[Xs]/y[X],(cff[Ys]-bt->xc->z)/cff[Ys]);
		add_sample(sss,1,(y[Y]-bt->xc->z)/cff[Ys]);
		solve(sss);

		sprintf(cc,"=R %.0lf %.0lf\r\n",cff[Xs],cff[Ys]-bt->xc->z);
		puts_pc(cc);
		for(i=1;i<50;++i)
		{
			sprintf(cc,"%.0lf %.0lf\r\n",y[X]/50*i,cff[Ys]*polyp((double)i/50,sss->rp,3));
			puts_pc(cc);
		}
		sprintf(cc,"%.0lf %.0lf\r\n=R\r\n>",y[X],cff[Ys]*polyp(1,sss->rp,3));
		puts_pc(cc);
		}
	return(k);
}
/*______________________________________________________________________________________________________________*/
/* NaklonMuzz izracuna spremenjeno izh. hitrost */
/* pri izracunanem I za dano razdaljo           */
/*______________________________________________________________________________________________________________*/
double	NaklonMuzz(double r)
{
	return(0);
}
