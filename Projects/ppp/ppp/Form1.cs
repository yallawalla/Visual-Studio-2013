using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ppp
{
    public partial class Form1 : Form
    {
        int qt=300,qu1=250,qu2=370;
        int u=450,t=105;

        public Form1()
        {
            InitializeComponent();
            ubar.Value=ubar.Maximum-u;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            chart1.Left = 0;
            chart1.Top = 0;
            chart1.Width = 2*Width/3;
            chart1.Height = Height;

            ubar.Top = 0;
            ubar.Height = Height;
            ubar.Left = Width - ubar.Width;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void chart1_Paint(object sender, PaintEventArgs e)
        {
            int i,j,k,E;
            int t0 = (t - (qu2 * qu2 * t) / (u * u))/10;
            int qt0 = qt/10;

            if (t0 < 0)
            {
                qt0 += t0;
                t0 = 0;
            }

            chart1.Series["p1"].Points.Clear();
            chart1.Series["p2"].Points.Clear();
            chart1.Series["E"].Points.Clear();

            for (i = j = 0; j < qt0; ++j, ++i)
            {
                chart1.Series["p1"].Points.AddXY(10 * i,0);
            }
            for (j = 0; j < t/10; ++j, ++i)
            {
                chart1.Series["p1"].Points.AddXY(10 * i, u);
            }
            for (j = 0; j < 20; ++i, ++j)
            {
                chart1.Series["p1"].Points.AddXY(10 * i, 0);
            }
            
            
            for (i = j = E = 0; j < qt0; ++j, ++i)
            {
                k = qu1 + ((qu2 - qu1) * j) * 10 / qt;
                E += k * k;
                chart1.Series["p2"].Points.AddXY(10 * i, k);
            }

            for (j = 0; j < t0; ++j, ++i)
            {
                k = u;
                E += k * k;
                chart1.Series["p2"].Points.AddXY(10 * i, u);
            }

            for (j = 0; j < 20; ++i, ++j)
            {
                chart1.Series["p2"].Points.AddXY(10 * i, 0);
            }


        }

        private void ubar_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void ubar_ValueChanged(object sender, EventArgs e)
        {
            u = ubar.Maximum-ubar.Value;
            this.Refresh();
        }

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            int n=Convert.ToInt32(chart1.ChartAreas[0].AxisX2.PixelPositionToValue(e.X));
            chart1.Series["E"].Points.AddXY(0,n);
            chart1.Series["E"].Points.AddXY(chart1.Width,n);
        }
    }
}

            //chart1.Series["p1"].Points.AddXY(Convert.ToDouble(i), 7.0/6.0*Convert.ToDouble(j));
            //chart1.Series["p2"].Points.AddXY(Convert.ToDouble(i), 7.0/6.0*Convert.ToDouble(k));

            //chart1.Series["p1"].Points.Clear();
            //chart1.Series["p2"].Points.Clear();
																					// dU = u*dt/2/t
                //dq=ptime= (ptime -  (p->Burst.qN * p->Burst.qN * ptime)/(p->Burst.Pmax *p->Burst.Pmax));
                //dp=(p->Burst.Pmax*(ptime%10))/(ptime+ptime/2);
	
                //if(ptime>9) {
                //    dq=0;
                //}
                //else {
                //    ptime=0;
                //}

                //n=(qtime*_uS)/_PWM_RATE_HI;															//	cas rampe
                //n+=(dq*_uS)/_PWM_RATE_HI;																//	skrajsava iz pulza
                //for(j=0; j<n; ++j) {
                //    i =  p->Burst.qPmax + ((p->Burst.qN - p->Burst.qPmax)*j + n/2)/n;				
                //    if(j==n-1)																						// ce je zadnji kvant, priatej ostanek
                //        i+=(p->Burst.qN*(dq%10) + 10)/(2*10);
                //    if(_STATUS(p, PFM_STAT_SIMM1))
                //        t->T1=t->T2=i;
                //    else
                //        t->T1=t->T2=Pdelay[0];
                //    if(_STATUS(p, PFM_STAT_SIMM2))
                //        t->T3=t->T4=i;
                //    else
                //        t->T3=t->T4=Pdelay[1];
                //    t->n=1;
                //    ++t;



//              E = u*u*t + Eo

    
//              du = 2*u*t*dE

