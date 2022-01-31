using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slam
{
    public partial class Form1 : Form
    {
        public struct Vehicle
        {
            public double X, Y, fi, V, theta;
        }

        List<Point> Points = new List<Point>();
        Vehicle Dron;

        public Form1()
        {
            InitializeComponent();
            Cursor = Cursors.Hand;
            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            timer1.Enabled = true;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var itemToRemove = Points.SingleOrDefault(r => Math.Abs(r.X - e.X) < 10 && Math.Abs(r.Y - e.Y) < 10);
                if (itemToRemove.IsEmpty)
                    Points.Add(new Point(e.X, e.Y));
                else
                    Points.Remove(itemToRemove);
            }

            if (e.Button == MouseButtons.Right)
            {
                Dron = new Vehicle
                {
                    X = e.X,
                    Y = e.Y,
                    fi = 0,
                    V = 0,
                    theta = 0
                };
            }
        }

        //private void label1_Paint(object sender, PaintEventArgs e)
        //{
        //    GraphicsPath gp = new GraphicsPath();
        //    gp.AddEllipse(label1.ClientRectangle);

        //    PathGradientBrush pgb = new PathGradientBrush(gp);

        //    pgb.CenterPoint = new PointF(label1.ClientRectangle.Width / 2,
        //                                 label1.ClientRectangle.Height / 2);
        //    pgb.CenterColor = Color.White;
        //    pgb.SurroundColors = new Color[] { Color.Red };

        //    e.Graphics.FillPath(pgb, gp);

        //    pgb.Dispose();
        //    gp.Dispose();
        //}

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Pen semiTransPen = new Pen(Color.FromArgb(10, 0, 0, 255), 10);
            //for (int i = 20; i < 200; i += 10)
            //{
            //    semiTransPen.Color = Color.FromArgb(200-i, 0, 0, 255);
            //    e.Graphics.DrawEllipse(semiTransPen, 200 - i/2, 200 - i/2, i, i);
            //}
            foreach (Point p in Points)
            {
                e.Graphics.DrawEllipse(Pens.Yellow, p.X-5, p.Y-5, 10, 10);
            }
            if (Dron.X != 0 && Dron.Y != 0)
            {
                e.Graphics.DrawEllipse(Pens.Green, (int)Dron.X - 5, (int)Dron.Y - 5, 10, 10);
                e.Graphics.DrawLine(Pens.Green, (float)Dron.X, (float)Dron.Y, (float)(Dron.X - 50 * Math.Cos(Dron.fi)), (float)(Dron.Y - 50 * Math.Sin(Dron.fi)));
            }
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            Dron.V = Dron.V - e.Delta*0.01;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            trackBar1.Value = Cursor.Position.X - Left - Width / 2;
            Dron.theta = -trackBar1.Value / 10000.0;

       }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Dron.X += Dron.V * Math.Cos(Dron.fi);
            Dron.Y += Dron.V * Math.Sin(Dron.fi);
            Dron.fi += Dron.V * Dron.theta;
            Refresh();
        }
    }
}

