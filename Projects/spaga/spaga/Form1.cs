using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spaga
{
    public partial class Form1 : Form
    {
        const double Pi = 3.14159;
        const int N = 24;
        int[] y = new int[N];
        int[] dy = new int[N];

        public Form1()
        {
            InitializeComponent();
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new System.Drawing.Pen(System.Drawing.Color.Cyan);
            y[0] = 100;
            for (int i = 1; i < N; ++i)
                if (y[i] > y[i - 1]) {
                   y[i] -= (y[i] - y[i - 1]) / 4 + 1;
                }
                 else if (y[i] < y[i - 1]) {
                    y[i] += (y[i-1] - y[i]) / 4 + 1;
                }

            for (int i = 0; i < N-1; ++i)
                e.Graphics.DrawLine(p,Width * i / N, Height / 2 - (int)y[i] ,Width * (i+1) / N, Height / 2 - (int)y[i+1]);
            p.Dispose();
        }




        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
