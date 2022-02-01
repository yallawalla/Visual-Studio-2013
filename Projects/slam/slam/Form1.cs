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
    public struct Vehicle
    {
        public double X, Y, Vx, Vy, fi, V, theta;
    }

    public partial class Form1 : Form
    {

        List<Point> Pos = new List<Point>();
        List<Point> Echo = new List<Point>();
        Vehicle Dron;
        Form Form2=null;
        Point cp;

        public Form1()
        {
            InitializeComponent();
            Cursor = Cursors.Hand;
            timer1.Enabled = true;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var itemToRemove = Pos.SingleOrDefault(r => Math.Abs(r.X - e.X) < 10 && Math.Abs(r.Y - e.Y) < 10);
                if (itemToRemove.IsEmpty)
                    Pos.Add(new Point(e.X, e.Y));
                else
                    Pos.Remove(itemToRemove);
            }

            if (e.Button == MouseButtons.Right)
            {
                Dron = new Vehicle();
                Dron.X = e.X;
                Dron.Y = e.Y;

                Form2=new Form1();
                Form2.Show();
                Form2.Width = Width;
                Form2.Height = Height;
                Form2.Left = Left + Width;
                Form2.Top = Top;
                Form2.Paint += new PaintEventHandler(Form2_Paint);
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
            foreach (Point p in Pos)
            {
                e.Graphics.DrawEllipse(Pens.Yellow, p.X - 5, p.Y - 5, 10, 10);
            }
            if (Form2 != null)
            {
                e.Graphics.DrawEllipse(Pens.Green, (int)Dron.X - 5, (int)Dron.Y - 5, 10, 10);
                e.Graphics.DrawLine(Pens.Green, (float)(Dron.X + 10 * Math.Cos(Dron.fi)), (float)(Dron.Y + 10 * Math.Sin(Dron.fi)), (float)(Dron.X - 30 * Math.Cos(Dron.fi)), (float)(Dron.Y - 30 * Math.Sin(Dron.fi)));
                e.Graphics.DrawLine(Pens.Red, (float)(Dron.X + 1 * Math.Cos(Dron.theta)), (float)(Dron.Y + 1 * Math.Sin(Dron.theta)), (float)(Dron.X - 10 * Math.Cos(Dron.theta)), (float)(Dron.Y - 10 * Math.Sin(Dron.theta)));
            }
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            foreach (Point p in Echo)
            {
                e.Graphics.DrawEllipse(Pens.LightGray, p.X - 5, p.Y - 5, 10, 10);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            Refresh();

            if (Form2 != null)
            {
                Dron.V = (Math.Pow(cp.X - Dron.X, 2) + Math.Pow(cp.Y - Dron.Y, 2)) * 0.0001;
                Dron.theta = Math.Atan2(cp.Y-Dron.Y, cp.X-Dron.X);
                Dron.Vx = Dron.V * Math.Cos(Dron.theta);
                Dron.Vy = Dron.V * Math.Sin(Dron.theta);
 
                Dron.X += Dron.Vx;
                Dron.Y += Dron.Vy;
                Dron.fi = Math.Atan2(Dron.Vy, Dron.Vx);


                Echo.Clear();
                foreach (Point p in Pos)
                {
                    double lambda = Math.Atan2(p.Y - Dron.Y, p.X - Dron.X);
                    double range = Math.Sqrt(Math.Pow(p.Y - Dron.Y, 2)+Math.Pow(p.X - Dron.X,2));

                    Echo.Add(new Point((int)(range*Math.Cos(lambda-Dron.fi))+Width/2,(int)(range*Math.Sin(lambda-Dron.fi))+Height/2));
                }
                Form2.Refresh();
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            cp.X = e.X;
            cp.Y = e.Y;
        }
    }
}

