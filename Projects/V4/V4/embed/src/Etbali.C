/********************************************************/
#include "remmenu.h"
#include "balis.h"

int	InitABC(gun *);
int	TockaTraj(double, double *, struct Lokalni *);
int	Points(void);
/* ------------------- Konstante -----------------------*/
double accg = 9.81;              /* pospesek gravitacije*/
double cro	= 3.926990817e-4;    /* pi/8 * 10-3			*/
/* ----------------- Spremenljivke ----------------- 	*/
int nE;                 /* racunanje E					*/
int nR;                 /* racunanje Rocni1				*/
double Eps = 0.5;       /* dolocanje kota				*/
/********************************************************/
/*::::::::::::: SPREMENLJIVE TABELE :::::::::::::::::	*/
/********************************************************/
struct		Lokalni	*V;
/********************************************************/
float	MvTab(struct Lokalni *V,lib *bt,gun *g)
{
	int n=(bt->ammpcs[CP->ammo])>>12;
	double d[4],dd=0;
	if(g->dvtemp[V->vm][V->vp])
	{
		d[0]=g->dvtemp[V->vm][V->vp][0];
		d[1]=g->dvtemp[V->vm][V->vp][1];
		d[2]=g->dvtemp[V->vm][V->vp][2];
		d[3]=g->dvtemp[V->vm][V->vp][3];
		dd=polyp(bt->powder,d, 4);
	}
	
	V->km = 1.0 - n * g->dmass[V->vm] / g->mass[V->vm];
	return(V->v0 - n*g->dvmass[V->vm][V->vp] + dd);
}
/********************************************************/
int Ballistic(lib *bt, lib *tg, lib *crest)
	{
	struct	Vlaga 		Vl;
	struct	Atmosfera 	At;
	double	h,dh,y[3];
	int		mass;
	lib		*btt;
	gun 	*g=&Guns[bt->gun];
	Vl.temp = meteo[AIR_T][0];  /* merjena temperatura 	*/
	Vl.h    = meteo[AIR_P][0];  /* merjen pritisk 		*/
	Vl.rv   = meteo[AIR_H][0];  /* merjena vlaga 		*/
	FiktT(&Vl);                 /* izracun fikt. temp. 	*/

	/* Inicializacija vhodne strukture Lokalni V 		*/

	if(!InitABC(g))
		return(eof);

	V->vm = CP->ammo;	        /* vrsta municije		*/
	V->vp = CP->charge & 0x7f;  /* vrsta polnjenja		*/
								/* razlika do cilja; tempiranje !	*/
	dh = g->alt[CP->ammo][CP->fuze];
	
	V->vo = bt->gun; 			/* vrsta orozja			*/
	V->v0 = g->mv[V->vm][V->vp];/* zacetna hitrost		*/
	V->y1 = tg->xc->z + dh;		/* visinska razlika		*/
	V->y0 = bt->xc->z;			/* nadmorska visina 	*/
	V->X  = tg->xc->y;          /* domet				*/
	V->h  = V->X/50.0;
	if(V->h < 200.0)
		V->h = 200;
	At.tau0 = Vl.tau;	        /* temperatura na meteo. visini   	*/
	At.ha0  = Vl.h;             /* pritisk mm Hg 					*/
								/* Izracuna podatke Ventceljeve at.	*/
								/* y visina orozja, &At podatki 	*/
								/*	atmosfere 						*/
	Lla2Utm(bt,btt=copylib(bt,btt=makelib()),NULL);
	AtmVent(btt->xc->z-meteo[HEIGHT][0],&At);
	freelib(btt);
	V->tau0 = At.tau;           /* temperatura pri orozju			*/
	V->ha0 = At.hm;             /* pritisk pri orozju				*/
	V->a0 = At.a; 		        /* hitrost zvoka pri orozju			*/
	V->ro0 = At.ro;             /* gostota zraka pri orozju			*/
	V->ro0a0 = At.a * At.ro;    /* rabim pri nadaljnih izracunih	*/
	V->v0 += V->v0 * bt->xc->muzzle / 1000.0;	/* izracun dV0 		*/
	mass = (bt->ammpcs[CP->ammo])>>12;
/*	V->v0 = SpreMas(V->v0, mass*2.0/3.0,&V->km);
	V->v0 = TempSmod(V->v0, bt->powder);
*/	V->v0 = MvTab(V,bt,g);
	V->Wx = V->Wz = meteo[WIND_S][0];
	V->Wx *= cos(2.0 * M_PI * ((uint)meteo[WIND_D][0]/64000.0 - tg->xc->x/(double)maxmils));
	V->Wz *= sin(2.0 * M_PI * ((uint)meteo[WIND_D][0]/64000.0 - tg->xc->x/(double)maxmils));

	/* Isce kot pri danem dometu ob upostevanju vetra, Ver. 30.9.96 */

	CP->tof = 0;
/*	PIT_disable();
*/	switch(g->gmode & (TRAJ_UPPER | TRAJ_LOWER))
	{
	case TRAJ_LOWER:
		CP->traj=0;
		break;
	case TRAJ_UPPER:
		CP->traj=1;
		break;
	}
	if(CP->traj)
		NaklonVetH(V);
	else
		NaklonVetL(V);
/*	PIT_enable();
*/	Points();
	if(!V->Err) {
		V->fi = atan(V->Z / V->X);
/* preveri ovire; ob napaki Ballistic vrne eof, xc v crest pa kaze	na oviro	*/
		crest->xc = crest->x;					/* pointer na zacetek			*/
		while(crest->xc) {						/* izracun trajektorije	na		*/
			if(TockaTraj(crest->xc->y,&h,V))	/* razdalji tocke v crest		*/
				if(h <= crest->xc->z) {			/* primerjava visin				*/
					CP->tof = 0;
					return(eof);
			}
			crest->xc=crest->xc->next;
		}
		crest->xc = crest->x;					/* ponovna nastavitev xc		*/
		addsecs(&tg->xc->t, -V->T - 0.5);
		if(g->ABC)								/* cas zaokr. na desetinke sec	*/
		{
			y[0]=g->ABC[V->vm][V->vp][3];
 			y[1]=g->ABC[V->vm][V->vp][4];
			y[2]=g->ABC[V->vm][V->vp][5];
			CP->tof = 10*(V->T - polyp(V->T,y,3))+0.5;
		}
		else
			CP->tof = V->T * 10.0 + 0.5;			
												/* korekcija azimuta za rot.	*/
		if(g->drift) {							/* projektil (drift != 0)		*/
			h =  g->drift[V->vm][V->vp][0] * V->theta0;
			h += g->drift[V->vm][V->vp][1] * exp(V->theta0);
			h += g->drift[V->vm][V->vp][2] * exp(V->theta0 * V->theta0);
		}
		else
			h=0;
/* formatiranje azimuta	*/
		tg->xc->y=ScaleAngle((double)g->zeroA/g->mils + g->scaleA * (bt->xc->refp/MR10 - tg->xc->x/(double)maxmils + (h+V->fi)/2.0/M_PI),1.0,g->mils,null);
		if(g->gmode & BEAR_DBL)
			tg->xc->y=fmod(tg->xc->y,g->mils/2);
/* formatiranje elevacije do cilja */
		tg->xc->z  = g->zeroE + g->scaleE*ScaleAngle(atan((V->y1 - dh) / V->X),2.0*M_PI,g->mils,eof);
/* formatiranje elevacije + balisticnega kota */
		tg->xc->x  = g->zeroE + g->scaleE*ScaleAngle(V->theta0,2.0*M_PI,g->mils,eof);
		if(g->gmode & ELEV_DBL)
			tg->xc->x -= tg->xc->z;
	}
	return(V->Err);
}

/*
		tg->xc->z = ELAZ_defr;
		tg->xc->y = (tg->xc->x/(double)maxmils - (h+V->fi)/2.0/M_PI) * g->mils;
		tg->xc->x  = g->zeroE + g->scaleE*ScaleAngle(V->theta0,2.0*M_PI,g->mils,eof);
		tg->xc->y += g->zeroA + g->scaleA*ScaleAngle(bt->xc->refp,MR10,g->mils,eof);
		while(tg->xc->y >= g->mils)
			tg->xc->y -= g->mils;
		while(tg->xc->y < 0)
			tg->xc->y += g->mils;
*/



/************************************************/
/* ----------- Balisticni programi ------------ */
/************************************************/
/************************************************/
/* RocniVet izracuna domet ob prisotnosti vetra */
/************************************************/
void RocniVet(struct Lokalni *I)
{
   double v00, theta00, c00;
   double vr0, thr0, psir0;
   double p0, p1, p2;
   double *cd=Guns[I->vo].coeff[I->vm][I->vp];

   v00 = I->v0;							/* ohrani vrednosti */
   theta00 = I->theta0;			
   c00 = I->c;	
   p0 = cos(theta00);
   p1 = v00 * p0 - I->Wx;
   psir0 = atan(I->Wz / p1);
   p2 = v00 * sin(theta00);	
   thr0 = atan(p2/ sqrt(I->Wz * I->Wz + p1 * p1));
   vr0 = I->Wx * I->Wx + I->Wz * I->Wz + v00 * v00;
   vr0 = sqrt(vr0 - 2.0 * v00 * I->Wx * p0);
   I->v0 = vr0;        /* priprava */
   I->theta0 = thr0;
   I->c =  BalKoef3(cd,thr0);
   I->c *= I->km;
   Rocni1(I);
   if (I->Err) return;
   p0 = sin(psir0);
   p1 = cos(psir0);
   I->Z = I->Wz * I->T - I->X * p0;
   I->X = I->Wx * I->T + I->X * p1;
   I->Zs = I->Wz * I->Ts - I->Xs * p0;
   I->Xs = I->Wx * I->Ts + I->Xs * p1;
   p0 = I->vc * cos(I->thc);
   p1 = I->vc * sin(I->thc);
   p2 = I->Wx * I->Wx + I->Wz * I->Wz + I->vc * I->vc;
   I->vc = sqrt(p2 + 2.0 * I->Wx * p0);
   p0 = p0 + I->Wx;
   I->thc = atan(p1 / sqrt(I->Wz * I->Wz + p0 * p0));
   I->v0 = v00;      /* Obnovi */
   I->theta0 = theta00;
   I->c = c00;
}

/************************************************/
/* Rocni1A pripravi za Rocni1 velikost integra  */
/* cijskega koraka, in balisticnega koeficienta */
/************************************************/
/*
void Rocni1A(struct Lokalni *I)
{
   int vm, vp;

   vm = I->vm;
   vp = I->vp;
   I->h = KorakH(I->theta0, Bali[vm].thm, Bali[vm].nk,
		Bali[vm].dth, Hji[vm][vp]);
   I->c = I->km * BalKoef1(I->vm, I->v0, I->theta0);
   Rocni1(I);
}
*/
/************************************************/
/* RocniVetA pripravi za RocniVet velikost */
/* integracijskega koraka                  */
/*******************************************/
/*
void RocniVetA(struct Lokalni *I)
{
   int vm, vp;
   vm = I->vm;
   vp = I->vp;
   I->h = KorakH(I->theta0, Bali[vm].thm, Bali[vm].nk,
		Bali[vm].dth, Hji[vm][vp]);
   RocniVet(I);
}
*/
/************************************************/
/* NaklonVetH izracuna naklon in odklon orozja  */
/* pri konstantnem vetru za zgornjo trajektorijo*/
/************************************************/
void NaklonVetH(struct Lokalni *I)
	{
	double f[3],e[3];
	double Xc, A, B, C;
	double thi, tlo, tmax;
	int i;
	double h1, h2, d1, d2, d, b, D, E, Ep, Em, h;
	gun	*g=&Guns[I->vo];

	Xc = I->X;               /* zahtevani domet */

	A=g->ABC[I->vm][I->vp][2];
	B=g->ABC[I->vm][I->vp][1];
	C=g->ABC[I->vm][I->vp][0];

	thi = 2.0 * M_PI * g->hi / g->mils;
	tlo = 2.0 * M_PI * g->lo / g->mils;
	tmax= -B/2.0/A;
	if(tlo > tmax)
		tmax=tlo;

	h = B*B - 4.0*A*(C - I->X);
	if(h < 0)
		h=tmax;
	else
		h = (-B - sqrt(h))/(2.0 * A);
	I->theta0 = 0;

	for(i=0; i<10; ++i) {
		I->theta0 += h;
		if(I->theta0 > thi)
			I->theta0 = thi;
		if(I->theta0 < tmax)
			I->theta0 = tmax;
		I->X = Xc;
		RocniVet(I);
		if(!I->Err) {
			if(fabs(Xc - I->X) < Eps)
				return;
			if((I->X < Xc) && I->theta0 == tmax) {
				I->Err = 2;
				return;
				}
			if((I->X > Xc) && I->theta0 == thi) {
				I->Err = 1;
				return;
				}
			switch(i){
				case 0:	f[0] = I->theta0;
						e[0] = I->X;
						C = - A*f[0]*f[0] - B*f[0] + I->X - Xc;
						h = B*B - 4.0*A*C;
						if(h < 0)
							h=tmax;
						else
							h = (-B - sqrt(h))/(2.0 * A);
						h -= I->theta0;
						break;
				case 1:	f[1] = I->theta0;
						e[1] = I->X;
						h = (f[1]-f[0])/(e[1]-e[0])*(Xc-e[1]);
						break;
				default:f[2] = I->theta0;
						e[2] = I->X;
						h1 = f[1] - f[0];
						h2 = f[2] - f[1];
						d1 = (e[1] - e[0]) / h1;
						d2 = (e[2] - e[1]) / h2;
						d  = (d2 - d1) / (h2 + h1);
						b  = d2 + d * h2;
						D  = b * b - 4.0 * (e[2] - Xc) * d;
						if (D < 0.0) {
							I->Err = 5;
							return;
							}
						D = sqrt(D);
						Ep = b + D;
						Em = b - D;
						if (fabs(Em) < fabs(Ep))
							E = Ep;
						else
							E = Em;
						h = -2.0 * (e[2] - Xc)/ E;
						e[0] = e[1];
						e[1] = e[2];
						f[0] = f[1];
						f[1] = f[2];
				}
			}
		else{
			if(I->Xs < I->X)		/* ce je teme pred in pod ciljem	*/
				return;             /* je predalec	!!!					*/
			if(I->theta0 == thi)    /* ce pri max.elev. ne doseze visine*/
				return;             /* je previsoko						*/
			switch(i) {
			case 0:	I->theta0 = thi;/* 1. strel -> isci doment pri max.	*/
					h=0;            /* elevaciji						*/
					break;
			default:I->theta0 -= h; /* sicer zmanjsaj korak za polovico */
					h /= -2;
			}
			--i;
			}
		}
	}
/************************************************/
/* NaklonVetL izracuna naklon in odklon orozja  */
/* pri konstantnem vetru za spodnjo trajektorijo*/
/************************************************/
void NaklonVetL(struct Lokalni *I)
	{
	double f[3],e[3];
	double Xc, A, B, C;
	double thi, tlo, tmax, tlos;
	int i;
	double h1, h2, d1, d2, d, b, D, E, Ep, Em, h;
	gun	*g=&Guns[I->vo];

	Xc = I->X;               /* zahtevani domet */

	A=g->ABC[I->vm][I->vp][2];
	B=g->ABC[I->vm][I->vp][1];
	C=g->ABC[I->vm][I->vp][0];

	thi = 2.0 * M_PI * g->hi / g->mils;
	tlo = 2.0 * M_PI * g->lo / g->mils;
	tmax= -B/2.0/A;
	tlos = atan2(I->y1,I->X);

	if(thi < tmax)
		tmax = thi;
	h = B*B - 4.0*A*(C - sqrt(I->X * I->X + I->y1 * I->y1));
	if(h < 0)
		h=tmax;
	else
		h = (-B + sqrt(h))/(2.0 * A);
	I->theta0 = tlos;

	for(i=0; i<10; ++i) {
		I->theta0 += h;
		if(I->theta0 > tmax)
			I->theta0 = tmax;
		if(I->theta0 < tlo)
			I->theta0 = tlo;
		if(I->theta0 < tlos)
			I->theta0 = tlos;
		I->X = Xc;
		RocniVet(I);
		if(!I->Err) {
			if(fabs(Xc - I->X) < Eps)
				return;
			if((I->X < Xc) && I->theta0 == tmax) {
				I->Err = 2;
				return;
				}
			if(tlos > atan2(I->Ys,I->Xs)) {
				I->Err = 1;
				return;
				}
			switch(i){
				case 0:	f[0] = I->theta0;
						e[0] = I->X;
						C = - A*f[0]*f[0] - B*f[0] + I->X - Xc;
						h = B*B - 4.0*A*C;
						if(h < 0)
							h=tmax;
						else
							h = (-B + sqrt(h))/(2.0 * A);
						h -= I->theta0;
						break;
				case 1:	f[1] = I->theta0;
						e[1] = I->X;
						h = (f[1]-f[0])/(e[1]-e[0])*(Xc-e[1]);
						break;
				default:f[2] = I->theta0;
						e[2] = I->X;
						h1 = f[1] - f[0];
						h2 = f[2] - f[1];
						d1 = (e[1] - e[0]) / h1;
						d2 = (e[2] - e[1]) / h2;
						d  = (d2 - d1) / (h2 + h1);
						b  = d2 + d * h2;
						D  = b * b - 4.0 * (e[2] - Xc) * d;
						if (D < 0.0) {
							I->Err = 5;
							return;
							}
						D = sqrt(D);
						Ep = b + D;
						Em = b - D;
						if (fabs(Em) < fabs(Ep))
							E = Ep;
						else
							E = Em;
						h = -2.0 * (e[2] - Xc)/ E;
						e[0] = e[1];
						e[1] = e[2];
						f[0] = f[1];
						f[1] = f[2];
				}
			}
		else{
			if(I->theta0 == tmax)   /* ce pri max.elev. ne doseze visine*/
				return;             /* je previsoko						*/
			if(I->Xs > Xc) {		/* ce je pri prenizki trajekt. teme	*/
				I->Err = 1;         /* ze predalec, je cilj preblizu	*/
				return;
				}
			switch(i) {
/* 1. strel -> povecaj kot za razliko temena do visine cilja			*/
			case 0:	h = 2 * (atan2(I->y1,I->Xs) - atan2(I->Ys,I->Xs));
					break;
			default:I->theta0 -= h;
					h /= 2;		/* sicer zmanjsaj korak za polovico */
			}
			--i;
			}
		}
	}
/************************************************/
/* NaklonMuzz izracuna spremenjeno izh. hitrost */
/* pri izracunanem I za dano razdaljo           */
/************************************************/
double	NaklonMuzz(double r)
	{
struct Lokalni	*I=V;

	double	f[3],e[3];
	double	Xc,m;
	int		i;
	double	h1, h2, d1, d2, d, b, D, E, Ep, Em, h;

	m = I->v0;
	h=0;
	Xc=r;

	for(i=0; i<10; ++i) {
		I->v0 += h;
		RocniVet(I);
		if(!I->Err) {
			if(fabs(Xc - I->X) < Eps)
				break;
			switch(i){
				case 0:	f[0] = I->v0;
						e[0] = I->X;
						h = I->v0 * (Xc - I->X)/Xc;
						break;
				case 1:	f[1] = I->v0;
						e[1] = I->X;
						h = (f[1]-f[0])/(e[1]-e[0])*(Xc-e[1]);
						break;
				default:f[2] = I->v0;
						e[2] = I->X;
						h1 = f[1] - f[0];
						h2 = f[2] - f[1];
						d1 = (e[1] - e[0]) / h1;
						d2 = (e[2] - e[1]) / h2;
						d  = (d2 - d1) / (h2 + h1);
						b  = d2 + d * h2;
						D  = b * b - 4.0 * (e[2] - Xc) * d;
						if (D < 0.0) {
							I->Err = 5;
							return(1);
							}
						D = sqrt(D);
						Ep = b + D;
						Em = b - D;
						if (fabs(Em) < fabs(Ep))
							E = Ep;
						else
							E = Em;
						h = -2.0 * (e[2] - Xc)/ E;
						e[0] = e[1];
						e[1] = e[2];
						f[0] = f[1];
						f[1] = f[2];
				}
			}
		else{
			switch(i) {
			case 0:	h = I->v0 * (I->y1 - I->Ys)/I->y1;
					break;
			default:I->v0 -= h;
					h /= 2;
					}
			--i;
			}
		}
	return(I->v0/m);
	}
/*----------------------------------------------*/
/***************** K O N E C ********************/
/*----------------------------------------------*/

/*------------------------------------------------*/
/*::::::::::::: DESNE STRANI ENACB :::::::::::::::*/
/*------------------------------------------------*/
/************************************************/
/* FunkE izracuna du/dx za sistem enacb z argu- */
/* mentom x. Uporabljena je atmosfera Ventcelja */
/* Zakon zracnega upora je CD43. Vrednosti at-  */
/* mosfere v startni tocki so dane v *p. y je   */
/* relativna visina glede na startno tocko      */
/* Pritisk in temperatura v startni tocki sta   */
/* lahko poljubna.                              */
/************************************************/
double FunkE(double u, double p, double y, struct Atmosfera *pp)
{
   double x, m, pom, v;

   nE++;							/* stetje - zacasno		*/
   v = u * sqrt(1 + p * p);
   x = (0.006328 * y) / pp->tau0;   /* pomozna sprem.		*/
   if (x < 0.14) {                  /* (1-x)**0.5			*/
	 m = 0.999997 - x * (0.499309 + 0.139021 * x);
   } else {
	 m = 0.999306 - x * (0.489492 + 0.174513 * x);
   }
   m = v / (pp->a0 * m);            /* Machovo stevilo		*/
   pom = CD43(m);
   pom = pom * m * pp->ro0a0;
   if (x <= 0.05) {                 /* Odvisnost od visine	*/
	 m=1.000017+x*(-4.891948+x*8.955130);
   } else if (x <=0.1) {
	 m=0.996803+x*(-4.762684+x*7.624088);
   } else if (x <=0.15) {
	 m=0.985486+x*(-4.536655+x*6.489576);
   } else if (x <=0.2) {
	 m=0.962657+x*(-4.232153+x*5.471807);
   } else {
	 m=0.923605+x*(-3.843824+x*4.505014);
   }
   return (pom * m);
}
/************************************************/
/* Rocni1 izracuna pot na podlagi podatkov iz   */
/* strukture I. Rezultate pusti v strukturi I   */
/* Err = 0..vse v redu, sicer je napaka         */
/* Metoda: Adams-Bashforth-Moulton              */
/************************************************/
void Rocni1(struct Lokalni *I)
{
	struct Atmosfera A1;
	double p11, p12, p13, p14;
	double p21, p22, p23, p24;
	double p31, p32, p33, p34;
	double up, pp, yp;
	double Dt, Du, Dp, Dy;
	int i, jj;
/*     double R, R1, R2;  */

	watchdog();
	nR++;              /* stevec izracunov */
	I->Err = 0;        /* ni napake */
	A1.tau0 = I->tau0;
	A1.ha0 = I->ha0;
	A1.ro0 = I->ro0;
	A1.a0 = I->a0;
	A1.ro0a0 = I->ro0a0;
	I->C1 = -I->c * cro;
	I->x = 0.0;
	if (I->Pad) I->St = 0; else I->St = 2;
	I->u[0] = I->v0 * cos(I->theta0);        /* zacetne vrednosti */
    I->p[0] = tan(I->theta0);
    I->y[0] = 0.0;
    I->t[0] = 0.0;
    I->tc[0] = I->h / I->u[0];
    I->j = 0;             /* stevec korakov */

    while (1) {
      jj = I->j;
      if (I->j <= 2) {
	I->uc[jj] = I->h * I->C1 * FunkE(I->u[jj], I->p[jj], I->y[jj], &A1);
	I->pc[jj] = -I->h * accg / (I->u[jj] * I->u[jj]);
	p11 = I->uc[jj];
	p21 = I->pc[jj];
	p31 = I->h * I->p[jj];
	up = I->u[jj] + 0.5 * p11;
	pp = I->p[jj] + 0.5 * p21;
	yp = I->y[jj] + 0.5 * p31;
	p12 = I->h * I->C1 * FunkE(up, pp, yp, &A1);
	p22 = -I->h * accg / (up * up);
	p32 = I->h * pp;
	up = I->u[jj] + 0.5 * p12;
	pp = I->p[jj] + 0.5 * p22;
	yp = I->y[jj] + 0.5 * p32;
	p13 = I->h * I->C1 * FunkE(up, pp, yp, &A1);
	p23 = -I->h * accg / (up * up);
	p33 = I->h * pp;
	up = I->u[jj] + p13;
	pp = I->p[jj] + p23;
	yp = I->y[jj] + p33;
	p14 = I->h * I->C1 * FunkE(up, pp, yp, &A1);
	p24 = -I->h * accg / (up * up);
	p34 = I->h * pp;
	I->u[jj+1] = I->u[jj] + (p11 + 2.0 * p12 + 2.0 * p13 + p14)/ 6.0;
	I->p[jj+1] = I->p[jj] + (p21 + 2.0 * p22 + 2.0 * p23 + p24)/ 6.0;
	I->y[jj+1] = I->y[jj] + (p31 + 2.0 * p32 + 2.0 * p33 + p34)/ 6.0;
	I->tc[jj+1] = I->h / I->u[jj+1];
	switch (jj) {
	  case 1:Dt = 0.41666667 * I->tc[0] + 0.666666667 * I->tc[1] -
			  0.0833333333 * I->tc[2];
		 I->t[1] = I->t[0] + Dt;
		 Dt = 0.41666667 * I->tc[2] + 0.666666667 * I->tc[1] -
		      0.0833333333 * I->tc[0];
		 I->t[2] = I->t[1] + Dt;
		 break;
	  case 2:jj++;
		 I->uc[jj]=I->h * I->C1*FunkE(I->u[jj],I->p[jj],I->y[jj], &A1);
		 I->pc[jj] = -I->h * accg / (I->u[jj] * I->u[jj]);
		 /* se casi */
		 Dt = 0.375 * I->tc[3] + 0.79166667 * I->tc[2] -
		      0.20833333333 * I->tc[1] + 0.0416666667 * I->tc[0];
		 I->t[3] = I->t[2] + Dt;
		 break;
	}
      } else {
	Du = (2.29166667*I->uc[jj]-2.45833333*I->uc[jj-1]+
		1.54166667*I->uc[jj-2]-0.375*I->uc[jj-3]);
	for (i = 0; i < 2; i++) {
	  I->u[jj+1] = I->u[jj] + Du;
	  I->pc[jj+1] = - I->h * accg / (I->u[jj+1] * I->u[jj+1]);
	  Dp = (0.375*I->pc[jj+1] + 0.79166667*I->pc[jj] -
		  0.20833333*I->pc[jj-1]+0.04166667*I->pc[jj-2]);
	  I->p[jj+1] = I->p[jj] + Dp;
	  Dy = I->h*(0.375*I->p[jj+1] + 0.79166667*I->p[jj] -
		  0.20833333*I->p[jj-1]+0.04166667*I->p[jj-2]);
	  I->y[jj+1] = I->y[jj] + Dy;
	  I->uc[jj+1]=I->h * I->C1 * FunkE(I->u[jj+1],I->p[jj+1],I->y[jj+1], &A1);
	  if (i == 0) {
	     Du = (0.375*I->uc[jj+1]+0.79166667*I->uc[jj]-
		     0.20833333*I->uc[jj-1]+0.04166667*I->uc[jj-2]);
	  }
	}
	I->tc[jj+1] = I->h / I->u[jj+1];
	Dt = (0.375*I->tc[jj+1]+0.79166667*I->tc[jj]-
		     0.20833333*I->tc[jj-1]+0.04166667*I->tc[jj-2]);
	I->t[jj+1] = I->t[jj] + Dt;
	}

      jj = I->j + 1;
      switch (I->St) {
	case 0:if (I->p[jj] < 0.0) { /* ne narasca vec ? */
		 I->St = 3;
		 Teme(I);
		 if (I->St != 1) return;
	       }
	       break;
	case 1:if (I->y[jj] < I->y1) {  /* pod ciljem */
		 I->St = 4;
		 CiljnaTocka(I);
		 return;
	       }
	       break;
	case 2:if (I->p[jj] < 0.0) {   /* pada */
		  I->St = 3;          /* doloci teme */
		  Teme(I);
		  return;
	       } else {  /* se narasca */
		 if (I->y[jj] > I->y1) {  /* preko cilja */
		   I->St = 4;
		   CiljnaTocka(I);
		   return;
		 }
		   }
	       break;
      }
      I->x += I->h;
	  I->j++;        /* !!! */
	}
}

/**********************************************/
/* Teme doloci elemente temena - na podlagi   */
/* podatkov, ki jih dobi v strukturi Lokalni  */
/**********************************************/
void Teme(struct Lokalni *I)
{
  int jj, n, j, mit;
  double a[5], dpx, d1, d2, d3, dd2, dd3, ddd3;
  double th, zz, yy, hycx1, dt;

  jj = I->j;
  /* stopnja polinoma za p(x) */
  n = 3;
  if (jj <= 2) {   /* po metodi RK */
	/* izracun manjkajocih podatkov */
    dpx = I->u[jj+1];                      /* u(x1) */
    dpx = -accg * I->h / (dpx * dpx);         /* odvod dp(x1) */
    /* koeficienti polinoma za p(x) */
    a[0] = I->p[jj];
    a[1] = I->pc[jj];
    a[2] = 3.0 * (I->p[jj+1] - I->p[jj]) - (2.0*I->pc[jj] + dpx);
    a[3] = I->pc[jj] + dpx - 2.0 * (I->p[jj+1]-I->p[jj]);
  } else {    /* metoda ABM */
    d1 = I->p[jj+1] - I->p[jj];
    d2 = I->p[jj] - I->p[jj-1];
    d3 = I->p[jj-1] - I->p[jj-2];
    dd2 = d1 - d2; dd3 = d2 - d3; ddd3 = dd2 - dd3;
    ddd3 = ddd3 / 6.0;
    dd2 = dd2 / 2.0;
    a[0] = I->p[jj];
    a[1] = d1 - dd2 - ddd3;
    a[2] = dd2;
    a[3] = ddd3;
  }

  /* doloci tocko temena */
  th = -a[0] / a[1];         /* zacetni priblizek iteracije */
  if (th > 1.0) th = 1.0;
  mit = 0;                   /* stevec iteracij */
  while (1) {                /* iterira */
    zz = yy = a[n];
    for (j = n-1; j>0; j--) {
      yy = th * yy + a[j];
      zz = th * zz + yy;
    }
    yy = th * yy + a[0];
    th = th - yy / zz;        /* novi priblizek */
    if (th < 0.0) th = 0.0;   /* omeji iteracije */
    if (th > 1.0) th = 1.0;
		mit++;
    if (mit > 10) I->Err = 1; /* prevec iteracij */
    if ((fabs(yy) < 0.001) || (I->Err == 1)) {
      I->Xs=I->x+th*I->h; break;
    }
  }

  /* stopnja polinoma za y(x) */
  n = 3;
  if (jj <= 2) {     /* metoda RK */
    /* izracun manjkajocih podatkov */
    hycx1 = I->h * I->p[jj+1];    /* odvod yc(x1) */
    /* koeficienti polinoma za y(x) */
    a[0] = I->y[jj];        /* y(x0) */
    dd2 = I->h * I->p[jj];
    a[1] = dd2;             /* dy/dx(x0) */
    d1 = I->y[jj+1] - I->y[jj];
    a[2] = 3.0 * d1 - (2.0*dd2 + hycx1);
    a[3] = dd2 + hycx1 - 2.0 * d1;
  } else {    /* metoda ABM */
    d1 = I->y[jj+1] - I->y[jj];
    d2 = I->y[jj] - I->y[jj-1];
    d3 = I->y[jj-1] - I->y[jj-2];
    dd2 = d1 - d2; dd3 = d2 - d3; ddd3 = dd2 - dd3;
    ddd3 = ddd3 / 6.0;
    dd2 = dd2 / 2.0;
    a[0] = I->y[jj];
    a[1] = d1 - dd2 - ddd3;
    a[2] = dd2;
    a[3] = ddd3;
  }

  yy = a[n];     /* visina temena */
  for (j = n-1; j>= 0; j--) {
      yy = th * yy + a[j];
  }
  I->Ys = yy;

  /* stopnja polinoma za t(x) */
  n = 3;
  if (jj <= 2) {    /* metoda RK */
    /* izracun manjkajocih podatkov */
    dt = I->h / I->u[jj+1];  /* odvod tc(x1) */
    /* koeficienti polinoma za t(x) */
    a[0] = I->t[jj];
    a[1] = I->tc[jj];
    dpx = I->t[jj+1] - I->t[jj];
    a[2] = 3.0 * dpx - (2.0*I->tc[jj] + dt);
    a[3] = I->tc[jj] + dt - 2.0 * dpx;
  } else {     /* metoda ABM */
    d1 = I->t[jj+1] - I->t[jj];
    d2 = I->t[jj] - I->t[jj-1];
    d3 = I->t[jj-1] - I->t[jj-2];
    dd2 = d1 - d2; dd3 = d2 - d3; ddd3 = dd2 - dd3;
    ddd3 = ddd3 / 6.0;
    dd2 = dd2 / 2.0;
    a[0] = I->t[jj];
    a[1] = d1 - dd2 - ddd3;
    a[2] = dd2;
    a[3] = ddd3;
  }
  yy = a[n];     /* cas leta */
  for (j = n-1; j>= 0; j--) {
      yy = th * yy + a[j];
  }
  I->Ts = yy;


  if (I->Ys < I->y1) {I->Err = 2; return;} /* previsoko */

  if (!I->Pad) {   /* doloci cilj na rastocem delu */
    I->St = 5;
	CiljnaTocka(I);
    return;
  }

  if (I->y[jj+1] < I->y1) {  /* pod ciljem */
    I->St = 5;
	CiljnaTocka(I);
  } else {
    I->St = 1;                     /* nadaljuje */
  }
}

/****************************************************/
/* CiljnaTocka doloci elemente cilja - na podlagi   */
/* podatkov, ki jih dobi v strukturi Lokalni        */
/****************************************************/
void CiljnaTocka(struct Lokalni *I)
{
  double thmin, thmax, hycx1, a[5], th, zz, yy;
  double d1, d2, d3, dd2, dd3, ddd3;
  int jj, n, j, mit;

  jj = I->j;
  if (I->St == 5) {    /* meje cilja */
    if (I->Pad) {
      thmin = (I->Xs - I->x) / I->h ; thmax = 1.0;
    } else {
      thmin = 0.0; thmax = (I->Xs - I->x) / I->h;
    }
  } else {
    thmin = 0.0; thmax = 1.0;
  }

  /* stopnja polinoma za y(x) */
  n = 3;
  if (jj <= 2) {     /* metoda RK */
    /* izracun manjkajocih podatkov */
    hycx1 = I->h * I->p[jj+1];    /* odvod yc(x1) */
    /* koeficienti polinoma za y(x) */
    a[0] = I->y[jj];        /* y(x0) */
    dd2 = I->h * I->p[jj];
    a[1] = dd2;             /* dy/dx(x0) */
    d1 = I->y[jj+1] - I->y[jj];
    a[2] = 3.0 * d1 - (2.0*dd2 + hycx1);
    a[3] = dd2 + hycx1 - 2.0 * d1;
  } else {    /* metoda ABM */
    d1 = I->y[jj+1] - I->y[jj];
    d2 = I->y[jj] - I->y[jj-1];
    d3 = I->y[jj-1] - I->y[jj-2];
    dd2 = d1 - d2; dd3 = d2 - d3; ddd3 = dd2 - dd3;
    ddd3 = ddd3 / 6.0;
    dd2 = dd2 / 2.0;
    a[0] = I->y[jj];
    a[1] = d1 - dd2 - ddd3;
    a[2] = dd2;
    a[3] = ddd3;
  }
  a[0] = a[0] - I->y1;

  /* doloci ciljno tocko */
  if (I->St == 4 || I->St == 5) {
    if (I->Pad) th = 1.0; else th = 0.0;  /* zacetni priblizek */
    mit = 0;                   /* stevec iteracij */
    while (1) {                /* iterira */
      zz = yy = a[n];
      for (j = n-1; j>0; j--) {
	yy = th * yy + a[j];
	zz = th * zz + yy;
      }
      yy = th * yy + a[0];
      th = th - yy / zz;            /* novi priblizek */
      if (th < thmin) th = thmin;   /* omeji iteracije */
      if (th > thmax) th = thmax;
      mit++;
      if (mit > 10) I->Err = 3; /* prevec iteracij */
      if ((fabs(yy) < 0.001) || (I->Err == 3)) {
	 I->X=I->x+th*I->h; break;
      }
    }
  }
  if (I->Vse)
	  ElCilja(I);  /* se ostali podatki */
}

/********************************************/
/* ElCilja doloci naslednje podatke cilja   */
/* T..cas leta, vc..hitrost padca, thc..kot */
/* padca.                                   */
/********************************************/
void ElCilja(struct Lokalni *I)
{
  int j, n, jj;
  double a[5], yy, th, dt, dpx, ppom, du;
  double d1, d2, d3, dd2, dd3, ddd3;
  struct Atmosfera A1;

  jj = I->j;
  th = (I->X - I->x)/ I->h;
  if ((th > 1.0) || (th < 0.0)) {I->Err = 9; return;}

  /* stopnja polinoma za t(x) */
  n = 3;
  if (jj <= 2) {    /* metoda RK */
    /* izracun manjkajocih podatkov */
    dt = I->h / I->u[jj+1];  /* odvod tc(x1) */
    /* koeficienti polinoma za t(x) */
    a[0] = I->t[jj];
    a[1] = I->tc[jj];
    dpx = I->t[jj+1] - I->t[jj];
    a[2] = 3.0 * dpx - (2.0*I->tc[jj] + dt);
    a[3] = I->tc[jj] + dt - 2.0 * dpx;
  } else {     /* metoda ABM */
    d1 = I->t[jj+1] - I->t[jj];
    d2 = I->t[jj] - I->t[jj-1];
    d3 = I->t[jj-1] - I->t[jj-2];
    dd2 = d1 - d2; dd3 = d2 - d3; ddd3 = dd2 - dd3;
    ddd3 = ddd3 / 6.0;
    dd2 = dd2 / 2.0;
    a[0] = I->t[jj];
    a[1] = d1 - dd2 - ddd3;
    a[2] = dd2;
    a[3] = ddd3;
  }
  yy = a[n];     /* cas leta */
  for (j = n-1; j>= 0; j--) {
      yy = th * yy + a[j];
  }
  I->T = yy;

  /* stopnja polinoma za p(x) */
  n = 3;
  if (jj <= 2) {   /* po metodi RK */
    /* izracun manjkajocih podatkov */
    dpx = I->u[jj+1];                      /* u(x1) */
    dpx = -accg * I->h / (dpx * dpx);         /* odvod dp(x1) */
    /* koeficienti polinoma za p(x) */
    a[0] = I->p[jj];
    a[1] = I->pc[jj];
    a[2] = 3.0 * (I->p[jj+1] - I->p[jj]) - (2.0*I->pc[jj] + dpx);
    a[3] = I->pc[jj] + dpx - 2.0 * (I->p[jj+1]-I->p[jj]);
  } else {    /* metoda ABM */
    d1 = I->p[jj+1] - I->p[jj];
    d2 = I->p[jj] - I->p[jj-1];
    d3 = I->p[jj-1] - I->p[jj-2];
    dd2 = d1 - d2; dd3 = d2 - d3; ddd3 = dd2 - dd3;
    ddd3 = ddd3 / 6.0;
    dd2 = dd2 / 2.0;
    a[0] = I->p[jj];
    a[1] = d1 - dd2 - ddd3;
    a[2] = dd2;
    a[3] = ddd3;
  }

  yy = a[n];     /* tangens kota padca */
  for (j = n-1; j>= 0; j--) {
      yy = th * yy + a[j];
  }
  I->thc = atan(yy);
  ppom = yy;

  /* stopnja polinoma za u(x) */
  n = 3;
  if (jj <= 2) {
    /* izracun manjkajocih podatkov */
/* ~~~ popravek 16.12.85 MM, zelo nizki vrednost Zpr < 0.1 vrze ven */
	A1.tau0 = I->tau0;
	A1.ha0 = I->ha0;
	A1.ro0 = I->ro0;
	A1.a0 = I->a0;
	A1.ro0a0 = I->ro0a0;
/* ~~~ */
	du = I->h * I->C1 * FunkE(I->u[jj+1],
		 I->p[jj+1], I->y[jj+1], &A1);
	/* koeficienti polinoma za u(x) */
    a[0] = I->u[jj];
    a[1] = I->uc[jj];
    d1 = I->u[jj+1] - I->u[jj];
    a[2] = 3.0 * d1 - (2.0*I->uc[jj] + du);
    a[3] = I->uc[jj] + du - 2.0 * d1;
  } else {     /* metoda ABM */
    d1 = I->u[jj+1] - I->u[jj];
    d2 = I->u[jj] - I->u[jj-1];
    d3 = I->u[jj-1] - I->u[jj-2];
    dd2 = d1 - d2; dd3 = d2 - d3; ddd3 = dd2 - dd3;
    ddd3 = ddd3 / 6.0;
    dd2 = dd2 / 2.0;
    a[0] = I->u[jj];
    a[1] = d1 - dd2 - ddd3;
    a[2] = dd2;
    a[3] = ddd3;
  }

  yy = a[n];     /* vodoravna hitrost */
  for (j = n-1; j>= 0; j--) {
      yy = th * yy + a[j];
  }
  I->vc = yy * sqrt(1.0 + ppom * ppom);
}
/**********************************************/
/* KorakH doloci integracijski korak h za     */
/* ekvidistancno podane kote. th..startni kot */
/* thz..zacetni kot tabele, n..podatkov v     */
/* tabeli, dth..korak tabele, p..kazalec na   */
/* tabelo. Ce je kot premajhen,vzame vrednost */
/* ki pripada najmanjsemu kotu.Podobno naredi */
/* pri prevelikem kotu                        */
/**********************************************/
/*
double KorakH(double th, double thz, int n, double dth, double *p)
{
   double ifg, ksi1, h, *p1, k1, k2;
   int i, n1;

   n1 = n - 1;
   ifg = (th - thz) / dth;
   i = ifg;
   if (i < 0) return (*p);
   if (i >= n1) return (*(p+n1));
   ksi1 = ifg - i;
   p1 = p + i;
   k1 = *p1; p1++;
   k2 = *p1;
   h = (k1 + ksi1*(k2 - k1));
   return (h);
}
*/
/***************************************/
/* TockaTraj doloci Y koordinato tocke */
/* na trajektoriji. Podana je X koor-  */
/* dinata in podatki trajektorije.     */
/* 1..vse v redu, 0..prevelik x        */
/***************************************/
int TockaTraj(double X, double *Y, struct Lokalni *I)
{
   double cel, frac, a[5], hycx1, dd2, d1, yy;
   int j, n;

   if (X > I->X) return (0);
   cel = X / I->h;                    /* h-jev */
   j = cel;
   frac = cel - (double)j;

   /* stopnja polinoma za y(x) */
   n = 3;
	/* izracun manjkajocih podatkov */
   hycx1 = I->h * I->p[j+1];    /* odvod yc(x1) */
	/* koeficienti polinoma za y(x) */
   a[0] = I->y[j];        /* y(x0) */
   dd2 = I->h * I->p[j];
   a[1] = dd2;             /* dy/dx(x0) */
   d1 = I->y[j+1] - I->y[j];
   a[2] = 3.0 * d1 - (2.0*dd2 + hycx1);
   a[3] = dd2 + hycx1 - 2.0 * d1;

   yy = a[n];     /* y koordinata */
   for (j = n-1; j>= 0; j--) {
      yy = frac * yy + a[j];
   }
   *Y = yy;
   return (1);
}



/*------------------------------------------------*/
/*::::::::: KOREKCIJE VHODNIH PODATKOV :::::::::::*/
/*------------------------------------------------*/
/************************************************/
/* TempSmod vrne korigirano hitrost v0 zaradi   */
/* spremembe temperature smodnika dts glede na  */
/* normalno vrednost                            */
/************************************************/
double TempSmod(double v, double dts)
{
   return (v * (1.0 + 0.0004 * dts));
}

/************************************************/
/* SpreMas izracuna korigirano hitrost v0 in    */
/* korekcijski faktor kc balisticnega koefici-  */
/* enta zaradi spremembe mase za sprm %         */
/************************************************/
double SpreMas(double v, double sprm, double *kc)
{
   *kc = 1.0 - sprm / 100.0;
   return (v * (1.0 - 0.004 * sprm));
}
/************************************************/
int		InitABC(gun *g) 
		{
		int		j,k;
		double	*dd;
		double	*abc[16],**ABC[16];
		samples	*s=NULL;
		if(!V)
			V=memalloc(sizeof(struct Lokalni));
		V->km = 1.0;
		V->Pad = 1;
		V->Vse = 1;
		AtmNaZac(V);
		if(g->ABC)
			return(eof);
		clrlcd();
		for(j=0; g->ammo[j]; ++j) {           /* j=municija */
			for(k=0; g->mv[j][k]; ++k) {    /* k=polnenje */
				V->vm=j;
				V->vp=k;
				V->v0 = g->mv[j][k];
				V->h = 200;
				s=init_samples(s,3);
				V->theta0 = 2.0 * M_PI * g->hi / g->mils;
				RocniVet(V);
				add_sample(s,V->theta0,V->X);
				V->theta0 = 2.0 * M_PI * g->lo / g->mils;
				RocniVet(V);
				add_sample(s,V->theta0,V->X);
				V->theta0 = 2.0*M_PI*((g->hi-g->lo)/2+g->lo)/g->mils;
				RocniVet(V);
				add_sample(s,V->theta0,V->X);
				dd=solve(s);

				xyLCD(k+10*(j/4),j%4);
				Putch('.',CHLCD);
				abc[k]=memalloc(3 * sizeof(float));
				abc[k][0]=dd[0];
				abc[k][1]=dd[1];
				abc[k][2]=dd[2];
				}
			ABC[j]=memalloc(k * sizeof(float *));
			while(k--)
				ABC[j][k]=abc[k];
			}
		g->ABC = memalloc(j * sizeof(float **));
		while(j--)
			g->ABC[j] = ABC[j];
		freesamples(s);
		return(eof);
		}
/************************************************/
int		Points(void)
		{
		int		i;
		char	c[64];

		if(LPP)
			if(V->Err)
				puts_pc("-R\r\n>");
			else {
				sprintf(c,"=R %.0lf %.0lf\r\n",V->Xs,V->Ys);
				puts_pc(c);
				for(i=1; i <= V->j; ++i) {
					sprintf(c,"%.0lf %.0lf\r\n",i * V->h ,V->y[i]);
					puts_pc(c);
					}
				sprintf(c,"%.0lf %.0lf\r\n=R\r\n>",V->X ,V->y1);
				puts_pc(c);
				}
		return(null);
		}

/*------------------------------------------------------
double	Ventzel(double h, double v)
{
	double x,m,pom;
	x = (0.006328 * h) / Tau0;
	if(x < 0.14)
		m = 0.999997 - x * (0.499309 + 0.139021 * x);
	else
		m = 0.999306 - x * (0.489492 + 0.174513 * x);
	m = v / (a0 * m);
	pom = CD43(m);
	pom = pom * m * ro0 * a0;
	if(x <= 0.05)
		m = 1.000017 + x * (-4.891948 + x * 8.95513);
	else
		if(x <= 0.1)
			m = 0.996803 + x * (-4.762684 + x * 7.624088);
		else
		
			if(x <= 0.15)
				m = 0.985486 + x * (-4.536655 + x * 6.489576);
			else
				if(x <= 0.2)
					m = 0.962657 + x * (-4.232153 + x * 5.471807);
				else
					m = 0.923605 + x * (-3.843824 + x * 4.505014);
	return(pom * m);
}

void	Integrate(struct Lokalni *b)
{
	enum	{VX,TANTH,ALT,TIM};

	int		r,mi,ni;
	double	h,v;
	double	yp[4],p[5][4];
	double	ro,y[4];

	y[VX] = b->vo * cos(b->theta0);
	y[TANTH] = tan(b->theta0);
	y[ALT] = b->y0;
	y[TIM] = 0;

	for(r = 0; r < b->X; r = r + b->h)
	{
		if(r + b->h > b->X)
			b->h = b->X - r;
  
		for(mi = 0; mi<5; ++mi)
		{
			switch(mi)
			{
			case 0:
				for(ni = 0; ni < 4; ++ni)
					yp[ni] = y[ni];
			case 1:
			case 2:
				for(ni = 0; ni < 4; ++ni)
					yp[ni] = y[ni] + p[mi - 1][ni] / 2.0;
			case 3:
				for(ni = 0; ni < 4; ++ni)
		            yp[ni] = y[ni] + p[mi - 1][ni];
			case 4:
				for(ni = 0; ni < 4; ++ni)
					yp[ni] = y[ni] + 2.0 * p[1][ni] + p[0][ni];
			}
			h = yp[ALT];
			v = yp[VX] * sqrt(1 + yp[TANTH] * yp[TANTH]);
			ro = Ventzel(h, v);
			p[mi][TIM] = b->h / yp[VX];
			p[mi][VX] = -cro * ro * b->h * b->c * b->km;
			p[mi][ALT] = b->h * yp[TANTH];
			p[mi][TANTH] = -b->h * accg / yp[VX] / yp[VX];
		}
		for (ni = 0; ni < 4; ++ni)
			y[ni] = y[ni] + (p[0][ni] + 2.0 * (p[1][ni] + p[2][ni]) + p[3][ni]) / 6.0;
 	}
	b->y1 = y[ALT];
	b->theta0 = atan(y[TANTH]);
	b->vo = y[VX] / cos(b->theta0);
	b->T = y[TIM];
}
*/
