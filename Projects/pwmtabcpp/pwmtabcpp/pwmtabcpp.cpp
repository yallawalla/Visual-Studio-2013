// pwmtabcpp.cpp : Defines the exported functions for the DLL application.
//

#include	"stdafx.h"
#include	<pfm.h>
#include	<math.h>

PFM			*pfm;
int			_PWM_RATE_HI=(10*_uS),
			_PWM_RATE_LO=(50*_uS),
			_I1off=0,_I2off=0,
			_U1off=0,_U2off=0;
_TIM18DMA	TIM18_buf[_MAX_BURST/(10*_uS)];


_QSHAPE	shape[8];

#define	_K1											(_STATUS(p, PFM_STAT_SIMM1)/1)
#define	_K2											(_STATUS(p, PFM_STAT_SIMM2)/2)
#define	_minmax(x,x1,x2,y1,y2) __min(__max(((y2-y1)*(x-x1))/(x2-x1)+y1,y1),y2)
/*******************************************************************************
* Function Name : SetPwmTab
* Description   : set the pwm sequence
* Input         : *p, PFM object pointer
* Return        :
*******************************************************************************/
void	SetPwmTab(PFM *p) {
_TIM18DMA	*t=TIM18_buf;
int		i,j,n;
int		to=p->Burst.Time,too=100;
int		Uo=p->Burst.Pmax;
//-------wait for prev to finish ---
#ifndef	WIN32
			while(_MODE(p,_PULSE_INPROC))
				Wait(5,App_Loop);
#endif
//-------DELAY----------------------
			for(n=2*((p->Burst.Delay*_uS)/_PWM_RATE_HI)-1; n>0; n -= 255, ++t) {
				t->T1=t->T2=_K1*p->Burst.Pdelay;
				t->T3=t->T4=_K2*p->Burst.Pdelay;
				if(n > 255)
					t->n=255;
				else
					t->n=n;
			};
//-------preludij-------------------
			if(p->Burst.Ereq ==0x03) {
				int	du=0,u=p->Burst.Pdelay;
							
				for(i=0; i<sizeof(shape)/sizeof(_QSHAPE); ++i)
					if(p->Burst.Time==shape[i].qref && shape[i].q0) {
						to=shape[i].q0;
						Uo=(int)(pow((pow((double)p->Burst.Pmax,3.0)*p->Burst.N*shape[i].qref/to),1.0/3.0)+0.5);
						if(Uo > shape[i].q1)
							Uo = shape[i].q1;
// prePULSE + delay
						for(n=((to*_uS)/_PWM_RATE_HI); n>0; n--,++t) 	{
							du+=(3*Uo-u-2*du)*70/shape[i].q0; 
							u+=du*70/shape[i].q0;						
							t[0].T1= t[0].T2=_K1*(du + u*shape[i].q2/100);
							t[0].T3= t[0].T4=_K2*(du + u*shape[i].q2/100);
							t->n=1;
						}																
// if Uo < q1 finish prePULSE & return
						if(Uo < shape[i].q1) {
							while(du > p->Burst.Pdelay) 	{
								du+=(0-u-2*du)*70/shape[i].q0; 
								u+=du*70/shape[i].q0;
								t[0].T1= t[0].T2=_K1*(du + u*shape[i].q2/100);
								t[0].T3= t[0].T4=_K2*(du + u*shape[i].q2/100);
								t->n=1;
								++t;
								++to;
							}			

							for(n=2*((p->Burst.Length*_uS/p->Burst.N-to*_uS)/_PWM_RATE_HI)-1;n>0;n -= 255,++t)	{
								t->T1=t->T2=_K1*p->Burst.Pdelay;
								t->T3=t->T4=_K2*p->Burst.Pdelay;
								if(n > 255)
									t->n=255;
								else
									t->n=n;
							}
							
							t->T1=t->T2=p->Burst.Psimm[0];
							t->T3=t->T4=p->Burst.Psimm[1];
							t->n=1;
							++t;
							
							t->T1=t->T2=p->Burst.Psimm[0];
							t->T3=t->T4=p->Burst.Psimm[1];
							t->n=0;
							return;
						}
// else change parameters & continue to 1.pulse
						to=shape[i].q3;
//						Uo=pow((pow(p->Burst.Pmax,3)*p->Burst.N*shape[i].qref - pow(shape[i].q1,3)*shape[i].q0)/shape[i].q3/p->Burst.N,1.0/3.0);
						Uo=(int)(pow((pow((double)p->Burst.Pmax,3.0)*p->Burst.N*shape[i].qref - pow((double)shape[i].q1,3)*shape[i].q0)/shape[i].qref/p->Burst.N,1.0/3.0)+0.5);
						too=_minmax(Uo,260,550,30,120);
					}						
				}
//-------PULSE----------------------					
			for(j=0; j<p->Burst.N; ++j) {
//-------PULSE----------------------		
				for(n=2*((to*_uS)/_PWM_RATE_HI)-1; n>0; n -= 255, ++t) {
					t->T1=t->T2=_K1*Uo;
					t->T3=t->T4=_K2*Uo;
					if(n > 255)
						t->n=255;
					else
						t->n=n;
					}
//-------PAUSE----------------------			
				for(n=2*((too*_uS)/_PWM_RATE_HI)-1;n>0;n -= 255,++t)	{
					t->T1=t->T2=_K1*p->Burst.Pdelay;
					t->T3=t->T4=_K2*p->Burst.Pdelay;
					if(n > 255)
						t->n=255;
					else
						t->n=n;
				}
			}
//-------PAUSE----------------------			
			for(n=2*((p->Burst.Length*_uS - p->Burst.N*(to+too)*_uS)/_PWM_RATE_HI)-1;n>0;n -= 255,++t)	{
				t->T1=t->T2=_K1*p->Burst.Pdelay;
				t->T3=t->T4=_K2*p->Burst.Pdelay;
				if(n > 255)
					t->n=255;
				else
					t->n=n;
			}
			t->T1=t->T2=p->Burst.Psimm[0];
			t->T3=t->T4=p->Burst.Psimm[1];
			t->n=1;
			++t;
			
			t->T1=t->T2=p->Burst.Psimm[0];
			t->T3=t->T4=p->Burst.Psimm[1];
			t->n=0;
	}
