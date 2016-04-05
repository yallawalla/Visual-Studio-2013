using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        Form1 f = null;
        List<double []> pts = new List<double[]>();
        double tau1 = 6.4e-3, tau2 = 1e-4, om11 = 1.3e-15, om22 = 3.7e-15, sig = 2.6e-20, c = 3e10, b2 = 0.2, a7 = 0.04, fact = 0.31, tc = 9e-9;

        public Form2(Form1 fparent)
        {
            InitializeComponent();
            f = fparent;
        }


        public double[] fdrv(double dt, double x, double[] y)
        {
            double[] dy = new double[3];

            double k = sig * c * (b2 * y[1] - a7 * y[0]);

            dy[0] = dt * (x / 1.7 - y[0] / tau1 + y[1] / tau2 - 2 * om11 * y[0] * y[0] + om22 * y[1] * y[1] + k * y[2]);
            dy[1] = dt * (x / 1.0 - y[1] / tau2 + om11 * y[0] * y[0] - om22 * y[1] * y[1] - k * y[2]);
            dy[2] = dt * (y[2] * fact * k - y[2] / tc);

            return dy;
        }

        public void display()
        {
            int t = 0;
            double dx1 = 0, dx2 = 0, x1 = 0, x2 = 0, e1 = 0, e2 = 0;

            double[] y = new double[] { 1, 1.8e9, 100 };
            double[] emax = new double[] { 0,0,0};
            Runge rk4 = new Runge();
//            double dt = 1e-9;
            chart1.Series["p1"].Points.Clear();
            chart1.Series["p2"].Points.Clear();

            foreach (double[] x in pts)
            {
                while (t < x[0])
                {
                    x1 += dx1 / bar.Value;
                    x2 += dx2 / bar.Value;

                    dx1 += (7.0 / 6.0 * x[1] - x1 - 2 * dx1) / bar.Value;
                    dx2 += (7.0 / 6.0 * x[2] - x2 - 2 * dx2) / bar.Value;

                    e1 += Math.Pow(x1, 3) / 400 * 1e-5;
                    e2 += Math.Pow(x2, 3) / 400 * 1e-5;

                    //for (n = 0; n < 50; ++n)
                    //{
                    //    y = rk4.solve(x1 * x1 * x1 * 1e18, y, dt, new Runge.Function(fdrv), emax);
                    //    chart1.Series["p1"].Points.AddXY(50 * t + n, x1);
                    //    chart1.Series["p2"].Points.AddXY(50 * t + n, y[2] * 1e-15);
                    //}
                    //e1 += Math.Pow(x1, 3) / 400 * 1e-6;
                    //e2 += Math.Pow(y[2] * 1e-15, 3) / 400 * 1e-6;

                    chart1.Series["p1"].Points.AddXY(t, x1);
                    chart1.Series["p2"].Points.AddXY(t, x2);
                    ++t;
                }
            }
            this.Text = "E1=" + String.Format("{0,0:0.0}", e1) + "J, E2=" + String.Format("{0,0:0.0}", 10*e2) + "mJ";
        }


        public void parse(string s)
        {
            if (s != "")
            {
                if (s.Substring(0, 1) == "t")
                {
                    this.clear();
                }
                else
                {
                    try
                    {
                        double[] d = Array.ConvertAll(s.Split(','), Double.Parse);
                        pts.Add(d);
                        this.Refresh();
                    }
                    catch { }
                }
            }
            else
                if (pts.Count > 0)
                    this.display();
        }

        public void clear()
        {
            chart1.Series["p1"].Points.Clear();
            chart1.Series["p2"].Points.Clear();
            chart1.Series["p3"].Points.Clear();
            pts.Clear();
        }

        public void show(double t, double a, double b)
        {
            chart1.Series["p1"].Points.AddXY(t, a);
            chart1.Series["p2"].Points.AddXY(t, b);
        }

 
        private void Form2_Resize(object sender, EventArgs e)
        {
            bar.Left = Width - bar.Width-10;
            bar.Top = 0;
            bar.Height = Height;
            chart1.Left = 0;
            chart1.Top = 0;
            chart1.Width = Width - bar.Width - 10;
            chart1.Height = Height;
        }


        private void bar_Scroll(object sender, ScrollEventArgs e)
        {
            display();
        }

    }
}
