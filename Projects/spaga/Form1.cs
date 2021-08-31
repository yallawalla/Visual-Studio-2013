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
        int[]   u = new int[30];
        int[]   du = new int[30];
        int     k1 = 10, k2 = 10, count = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if(count++ < 100)
                u[0] = 10000;
            else
                u[0] = 0;
            u[u.Length-1] = 0;

 
            for (int i = 0; i < u.Length-1; ++i)
            {
                du[i] += (u[i] - u[i+1] - 0*du[i]/3) / k1;
            }
            for (int i = 0; i < u.Length-1; ++i)
            {
                u[i] -= du[i] / k2;
                u[i+1] += du[i] / k2;
            }
            for (int i = 0; i < u.Length; ++i)
            {
                int rc = Math.Max(Math.Min(255, u[i] / 30), 0);
                int gc = Math.Max(Math.Min(255, du[i] / 10), 0);
                g.DrawRectangle(new Pen(Color.FromArgb(rc, gc, 100), 3), 5 * i, Height / 2 - u[i] / 100, 5, 5);
//                g.DrawRectangle(new Pen(Color.FromArgb(rc, gc, 100), 3), 5 * i, Height / 2 , 5, 5);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
