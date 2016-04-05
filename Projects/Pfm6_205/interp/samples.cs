using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class samples
    {
		double[]    tp,fp,rp;
		int         n;
        /*..............................................................*/
        /* inicializacija vzorcev pointer p, stopnja n
        */
        samples init_samples(int size)
        {
            int i, j;
            
            tp = new double[4 * n * n];
            fp = new double[2 * n];
            rp = new double[2 * n];
            
            n = size;
            for (i = 0; i < n; ++i)
            {
                fp[i] = 0;
                for (j = 0; j < n; ++j)
                    tp[n * i + j] = 0;
            }
            return (this);
        }
/*..............................................................*/
/*   Izracun determinante matrike reda n x n, p=element (0,0)	*/
/*..............................................................*/
double	det(double[]  p, int x,int y, int n)
{
	int		i,j,sign=1;
	double 	D=0;

	for(i=1,j=0;j<n;++j,i*= 2)
	{
		if((i & y)==0)
		{
			if(x < n-1)
			{
				D  +=  p[n*j+x]*det(p, x+1, y+i, n)*sign;
				sign *= -1;
			}
			else
				D=p[n*j+x];
		}
	}
	return(D);
}
/*..............................................................*/
/* vpis vzorca f(t) v stack p
*/
void	add_sample(samples p, double t, double f)
{
	int	i,j;
	for(i=0; i<p.n; ++i)
	{
		for(j=0; j<p.n; ++j)
			if((i+j) != 0)
				p.tp[i*p.n+j]  +=  Math.Pow(t,i+j);
			else
				p.tp[i*p.n+j]  +=  1.0;
		if(i != 0)
			p.fp[i]  +=  f*Math.Pow(t,i);
		else
			p.fp[i]  +=  f;
	}
}
/*..............................................................*/
/* vpis periodicnega vzorca f(t) v stack p
*/
void	add_sample_t(samples p, double t, double f, double per)
{
	int         i,j;
	double[]	q = new double[2*(p.n)];

	for(i=0; i < (p.n)/2+1; ++i)
		q[i]=Math.Cos(2.0*Math.PI*t/per*i);
	for(i=1; i < (p.n)/2+1; ++i)
        q[i + (p.n) / 2] = Math.Sin(2.0 * Math.PI * t / per * i);
	for(i=0; i < p.n; ++i)
	{
		for(j=0; j < p.n ; ++j)
			p.tp[(p.n)*i+j]  +=  q[j]*q[i];
		p.fp[i]  +=  q[i]*f;
	}
//	free(q);
}
/*..............................................................*/
/* vpis eksponencialnega ènega vzorca f(t) v stack p
*/
void	add_sample_e(samples p, double t, double f)
{
	int	i,j;
    double[] q = new double[2 * (p.n)];
    q[0] = t;
	for(i=1; i < p.n; ++i)
		q[i]=Math.Exp(Math.Pow(t,i));
	for(i=0; i < p.n; ++i)
	{
		for(j=0; j < p.n ; ++j)
			p.tp[(p.n)*i+j]  +=  q[i]*q[j];
		p.fp[i]  +=  q[i]*f;
	}
//	free(q);
}
/*..............................................................*/
/* Resitev lin. enacbe reda n-1, p= matrika koef. reda n x n	*/
/* a[0] + a[1]*p + a[2]*p^2 + ... a[n-1]*p^n-1 = q				*/
/* r = vektor resitev											*/
/* ce je DET(p)  ==  0, ni resitve, funkcija vrne NULL			*/
/*..............................................................*/
double[]        solve(samples p)
{
	int         i,j;
    double[]    c;
	double      d=det(p.tp,0,0,p.n);
	if(d == 0)
		return(null);

	c = new double[p.n * p.n];
	for(i=0; i<p.n; ++i)
	{
		for(j=0;j< p.n * p.n; ++j)
			if(j % p.n  ==  i)
				c[j]=p.fp[j / p.n];
			else
				c[j]=p.tp[j];
		p.rp[i]=det(c,0,0,p.n)/d;
	}
//	free(c);
	return(p.rp);
}
/*..............................................................*/
/* Izracun polinoma a0 + a1*k + a2*k^2 + ...+ an*k^n			*/
/*..............................................................*/
double	polyp(double t, double[] a, int n)
{
	int		i;
	double	ft;
	for(i=1, ft=a[0]; i<n; ++i)
		ft  +=  a[i]*Math.Pow(t,i);
	return(ft);
}
/*..............................................................*/
/* Izracun polinoma a0*k + a1*e^k + a2*e^(k^2) +...+ an*e^k(k^n)*/
/*..............................................................*/
double	polye(double t, double[] a, int n)
{
	int		i;
	double	ft;
	for(i=1, ft=a[0]*t; i<n; ++i)
		ft  +=  a[i]*Math.Exp(Math.Pow(t,i));
	return(ft);
}
/*..............................................................*/
/* Izracun trig. polinoma										*/
/* a0 + a1*cos(k/per) + a2(cos(2*k/per) +...+					*/
/*      a(n/2)*sin(k/per) + a(n/2+1)(sin(2*k/per)				*/
/*..............................................................*/
double	polyt(double t, double[] a, double per,int n)
{
	int		i;
	double	ft;
	for(i=1, ft=a[0]; i<n; ++i)
		ft  +=  a[i]*Math.Cos(i*2.0*Math.PI*t/per) + a[i+n-1]*Math.Sin(i*2.0*Math.PI*t/per);
	return(ft);
}





    }
}
