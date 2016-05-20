using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Media;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        const int N=200;
        double[] x = new double[N];
        double[] y = new double[N];
        double[] vx = new double[N];
        double[] vy = new double[N];
        int idx = 0;
        double hh = 0;
        double h = 0;
        double dt = 0.05;
        double g = 9.81;
        SoundPlayer snd = new SoundPlayer("shower.wav");
        Boolean sndflag = false;

        public Form1()
        {
            InitializeComponent();
            button1_Click(null, null);
            Console.WriteLine("here we go: ");
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.Aqua), Width / 20, Height / 20 + Height / 2 - (int)h, Width / 10, (int)h);
            e.Graphics.DrawRectangle(new Pen(Color.White), Width / 20, Height / 20, Width / 10, Height / 2);

            for (int i = 0; i < N; ++i )
            {
              x[i] = x[i] + vx[i] * dt;
              y[i] = y[i] + vy[i] * dt;
              vx[i] = vx[i];
              vy[i] = vy[i] + g * dt;
              e.Graphics.DrawArc(new Pen(Color.Aqua), (int)x[i], (int)y[i], 2, 2, 0, 360);
            }

            if (h <= 0 && y[idx] > Height)
            {
                snd.Stop();
                sndflag = false;
            }     
                

            if (h > 0 && hh == 0)
            {
                if (sndflag==false)
                {
                    snd.Play();
                    sndflag = true;
                }
   
                x[idx] = Width / 20 + Width / 10;
                y[idx] = Height / 2 + Height / 20;
                vx[idx] = Math.Sqrt(2 * g * h);
                vy[idx] = 0;
                h = h - 0.1;

             }

            if(hh > 0)
            {
                h = h + 1;
                hh = hh - 1;
            }

            if(h > Height/2)
            {
                hh = 0;
            }

            idx = (idx + 1) % N;
       }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
            if (Console.KeyAvailable)
            {
                switch(Console.ReadKey().Key)
                {
                    case ConsoleKey.R:
                        hh = 100;
                        break;
                    case ConsoleKey.Escape:
                        Application.Exit();
                        break;
                }

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            hh = 100;
        }

    }
}
