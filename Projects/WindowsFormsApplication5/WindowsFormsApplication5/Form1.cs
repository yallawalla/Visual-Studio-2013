using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        PointF p, v, a, ep, ev, gps;
        Random rnd = new Random();
        int count;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Point q = new Point(Cursor.Position.X, Cursor.Position.Y);
            float dx = 0, dy = 0;

            if (p.X != 0 && p.Y != 0)
            {
                a.X = 0;// q.X - p.X - v.X + (float)(rnd.NextDouble() * 0.1 - 0.05);
                a.Y = 0;// q.Y - p.Y - v.Y + (float)(rnd.NextDouble() * 0.1 - 0.05);
                v.X = q.X - p.X;
                v.Y = q.Y - p.Y;
                p.X = q.X;
                p.Y = q.Y;
            }

            if (gps.X != 0 && gps.Y != 0)
            {
                dx = gps.X - ep.X;
                dy = gps.Y - ep.Y;
                if (count % (1000 / timer1.Interval) == 0)
                {
//                    gps.X += (float)(rnd.NextDouble() * 20 - 10);
//                    gps.Y += (float)(rnd.NextDouble() * 20 - 10);
                }
            }

            ep.X += ev.X + dx * 0.05f;
            ep.Y += ev.Y + dy * 0.05f;
            ev.X += a.X + dx * 0.01f;
            ev.Y += a.Y + dy * 0.01f;

            Refresh();
            ++count;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (p.X != 0 && p.Y != 0)
            {
                g.DrawEllipse(Pens.Yellow, ep.X - 5, ep.Y - 5, 10, 10);
                g.DrawLine(Pens.Yellow, ep.X - 10, ep.Y, ep.X + 10, ep.Y);
                g.DrawLine(Pens.Yellow, ep.X, ep.Y - 10, ep.X, ep.Y + 10);
            }
            if (gps.X != 0 && gps.Y != 0)
            {
                g.DrawEllipse(Pens.Green, ep.X - 2, ep.Y - 2, 4, 4);
                g.DrawLine(Pens.Green, gps.X - 20, gps.Y, gps.X + 20, gps.Y);
                g.DrawLine(Pens.Green, gps.X, gps.Y - 20, gps.X, gps.Y + 20);
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                p.X = Cursor.Position.X;
                p.Y = Cursor.Position.Y;
                v.X = v.Y = a.X = a.Y = 0;

                ev.X = ev.Y = 0;
                ep.X = e.X;
                ep.Y = e.Y;
            }
            if (e.Button == MouseButtons.Right)
            {
                gps.X = e.X;
                gps.Y = e.Y;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (gps.X != 0 && gps.Y != 0 && count % (1000 / timer1.Interval) == 0)
            //{
            //    gps.X = e.X;
            //    gps.Y = e.Y;
            //}
        }
    }
}
