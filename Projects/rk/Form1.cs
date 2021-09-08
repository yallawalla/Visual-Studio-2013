using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rk
{
    public struct  atm {
        public double temp;             // izmerjena temperatura
        public double hg;               // izmerjeni pritisk
        public double rv;               // izmerjena rel. vlaga
        public double h;                // višina meritve
        public double tau0;             /* temperatura na povrsini zemlje */

        public double ha0;              /* pritisk na povrsini zemlje v mm Hg */
        public double ro0;
        public double a0;
        public double ro0a0;
        public double y;                /* visina nad povrsino zemlje */
        public double tau;              /* temperatura */
        public double hm;               /* pritisk v mm Hg */
        public double ro;	    	    /* gostota */
        public double a;    	        /* hitrost zvoka */
    };

    public partial class Form1 : Form
    {
        atm atm;
        double[] emji = new double[] {
	        0.77, 0.85, 0.94, 1.03, 1.13, 1.24, 1.36, 1.49,
	        1.63, 1.78, 1.95, 2.13, 2.32, 2.53, 2.76, 3.01,
	        3.29, 3.57, 3.88, 4.22, 4.58, 4.90, 5.30, 5.70,
	        6.10, 6.50, 7.00, 7.50, 8.10, 8.60, 9.20, 9.80,
	        10.5, 11.2, 12.0, 12.8, 13.6, 14.5, 15.5, 16.5,
	        17.5, 18.7, 19.8, 21.1, 22.4, 23.8, 25.2, 26.7,
	        28.4, 30.0, 31.8, 33.7, 35.7, 37.7, 39.9, 42.2,
	        44.6, 47.1, 49.7, 52.4, 55.3, 55.3};


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            atm.rv = 60;
            atm.hg = 750;
            atm.temp = 25;
            atm.h = 0;

            int i = (int)Math.Max(0, Math.Min(60, atm.temp + 20));
            double em = emji[i] + (atm.temp - Math.Floor(atm.temp)) * (emji[i + 1] - emji[i]);
            atm.tau = (273.0 + atm.temp) / (1.0 - (3.0 * atm.rv * em) / (800.0 * atm.hg));
            atm.a = 20.0484413 * Math.Sqrt(atm.tau);
            atm.ro = 0.4643761 * atm.hg / atm.tau;
            for (int k = 0; k < 10000; k += 1000)
            {
                chart1.Series.Add("s" + k.ToString());
                chart1.Series["s" + k.ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                for (int n = 100; n < 1000; n += 10)
                    chart1.Series["s" + k.ToString()].Points.AddXY(n, drag(n, k));
            }
        }

        private double drag(double v, double h)
        {
            double tau = atm.tau - 0.006328 * (h-atm.h);
            double hg = 750.0 * Math.Pow(atm.tau / 288.9, 5.4);
            double a = 20.0484413 * Math.Sqrt(tau);
            double ro = 0.4643761 * hg / tau;
            return ro*CD43(v / a);
        }

        /**************************************************/
        /*   Siacci izracuna CDS(m). m..Machovo stevilo   */
        /**************************************************/
        double Siacci(double m)
        {
            double p, r;

            p = 15.06 * m * (340.8 * m - 300.0);
            p = p / (371.0 + Math.Pow(1.704 * m, 10.0));
            r = 56.16 * m - 47.95;
            p = p + Math.Sqrt(r * r + 9.6);
            p = p + 68.23 * m - 48.05;
            p = 0.018193 * p / (m * m);
            return (p);
        }
        /**************************************************/
        /*   CD43(M) izracuna ruski etalon iz leta 1943   */
        /**************************************************/

        private double CD43(double m)
        {
           double[] a1 = new double[] {-2.083333, 1.180556, -0.3125, 0.038056, -0.001667, 0.157};
           double[] a2 = new double[] {613.333335, -233.33334, 39.0, -2.813333, 0.068667, 0.158};
           double[] a4 = new double[] {-78.411093, 43.245043, 1.803950, -3.74780, 0.88697, 0.325};
           double[] a5 = new double[] {1829.806015, -426.848831, 42.480376, -1.984508, 0.015829, 0.385};
           double[] a6 = new double[] {0.030874, -0.117636, 0.209691, -0.151862, -0.05038, 0.385};
           double cd, sm;
           int i;

           if (m < 0.6) {
              cd = 1.388889;
              sm = m;
              for (i = 0; i < 6; i++)
                cd = cd * sm + a1[i];
           } else if (m < 0.9) {
              cd = -533.333337;
              sm = m - 0.6;
                for (i = 0; i < 6; i++)
	         cd = cd * sm + a2[i];
           } else if (m < 1.0) {
              cd = -1.025 + 1.35 * m;
           } else if (m < 1.2) {
              cd = -445.15561;
              sm = m - 1.0;
              for (i = 0; i < 6; i++)
	         cd = cd * sm + a4[i];
           } else if (m < 1.4) {
              cd = -2831.379493;
              sm = m - 1.2;
                for (i = 0; i < 6; i++)
	         cd = cd * sm + a5[i];
           } else if (m < 3.56) {
              cd = -0.003049;
	          sm = m - 1.2;
                for (i = 0; i < 6; i++)
	         cd = cd * sm + a6[i];
           } else {
	          cd = 0.260;
           }
           return cd;
        }

    }
}
