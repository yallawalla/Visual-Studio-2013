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
        public double X, Y, Vx, Vy, fi, V, theta,lambda;
        public void Draw(Graphics g, Pen p)
        {
            Rectangle r = new Rectangle( (int)X-10, (int)Y-5, 20, 10 );
            using (Matrix m = new Matrix())
            {
                m.RotateAt((float)(fi/Math.PI*180), new PointF(r.Left + (r.Width / 2), r.Top + (r.Height / 2)));
                g.Transform = m;
                g.DrawRectangle(p, r);
                g.ResetTransform();
            }
        }
        public void Beam(Graphics g, int w, int l) {
            GraphicsPath gp = new GraphicsPath();
            gp.AddPie((int)X - l/2, (int)Y - l/2, l, l, (int)(lambda/Math.PI*180)-w/2,w);
            PathGradientBrush pgb = new PathGradientBrush(gp);
            pgb.CenterPoint = new Point((int)X,(int)Y);
            pgb.CenterColor = Color.White;
            pgb.SurroundColors = new Color[] { Color.Black };

            g.FillPath(pgb, gp);

            pgb.Dispose();
            gp.Dispose();
        }
        public void Update()
        {
            Vx = V * Math.Cos(theta);
            Vy = V * Math.Sin(theta);
            X += Vx;
            Y += Vy;
            fi = Math.Atan2(Vy, Vx);
        }
    }

    public partial class Form1 : Form
    {
        Random      rnd = new Random();
        List<Point> Pos = new List<Point>(),  
                    Echo = new List<Point>();
        Vehicle     Dron, DronEst;
        Form        Form2=null;
        Point       cp;

        public Form1()
        {
            InitializeComponent();
            Cursor = Cursors.Hand;
            timer1.Enabled = true;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            cp.X = e.X;
            cp.Y = e.Y;
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
                Dron            = new Vehicle();
                Dron.X          = e.X;
                Dron.Y          = e.Y;

                DronEst         = new Vehicle();
                DronEst.X       = Width / 2;
                DronEst.Y       = Height / 2;

                Form2           =new Form1();
                Form2.Show();
                Form2.Width     = Width;
                Form2.Height    = Height;
                Form2.Left      = Left + Width;
                Form2.Top       = Top;
                Form2.Paint     += new PaintEventHandler(Form2_Paint);

                Echo.Clear();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            foreach (Point p in Pos)
                e.Graphics.DrawEllipse(Pens.Yellow, p.X - 5, p.Y - 5, 10, 10);
            if (Form2 != null)
            {
                Dron.Draw(e.Graphics, Pens.Red);
                Dron.Beam(e.Graphics,30,200);
            }
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            foreach (Point p in Echo)
                e.Graphics.DrawEllipse(Pens.LightGray, p.X - 2, p.Y - 2, 4, 4);
            DronEst.Draw(e.Graphics, Pens.Yellow);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Form2 != null)
            {
                foreach (Point p in Pos)
                {
                    double range = Math.Sqrt(Math.Pow(p.Y - Dron.Y, 2) + Math.Pow(p.X - Dron.X, 2));
                    double l = Math.Atan2(p.Y - Dron.Y, p.X - Dron.X) - Dron.fi - Dron.lambda;

                    if (l > Math.PI)
                        l -= 2 * Math.PI;
                    if (l < -Math.PI)
                        l += 2 * Math.PI;

                    if (Math.Abs(l) < Math.PI/18.0/2.0 && range < 200)
                        Echo.Add(new Point((int)(range * Math.Cos(Dron.lambda + DronEst.fi) + DronEst.X), (int)(range * Math.Sin(Dron.lambda + DronEst.fi) + DronEst.Y)));
                }

                Dron.V = (Math.Pow(cp.X - Dron.X, 2) + Math.Pow(cp.Y - Dron.Y, 2)) * 0.0001;
                Dron.theta = Math.Atan2(cp.Y-Dron.Y, cp.X-Dron.X);
                Dron.Update();

                DronEst.V = Dron.V;
                DronEst.theta = Dron.theta;
                DronEst.Update();
                Dron.lambda = (Dron.lambda + Math.PI / 18.0) % (2*Math.PI);

                Form2.Refresh();
            }  
            Refresh();
        }
    }
}

