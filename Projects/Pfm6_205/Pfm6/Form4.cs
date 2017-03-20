using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form4 : Form
    {
        Rectangle r,rf;
        Brush rGrad,rfGrad;
        
        int xo, yo,th1,th2,tl1,tl2;
        public int tmax;
        Point p1, p2;
        public Form4()
        {
            InitializeComponent();
        }

        public string ShowTemp(int temp)
        {
            tmax = th1 = th2 = tl1 = tl2 = temp;
            if (Visible == true)
            {
                Refresh();
                this.Text =
                    "f " + (200 * r.X / Width - 39).ToString() +                               // 25,35,20,95
                    "," + (200 * (r.X + r.Width) / Width - 39).ToString() +
                    "," + (139 - 200 * (r.Y + r.Height) / Height).ToString() +
                    "," + (139 - 200 * r.Y / Height).ToString() + '\r';
                return (this.Text);
            }
            else
                return "";
        }

        public string  ShowTemp(string[] s)
        {
            if (s.Length > 10)
            {
                th1 = (int)Convert.ToSingle(s[8]);
                th2 = (int)Convert.ToSingle(s[9]);
                tl1 = (int)Convert.ToSingle(s[10]);
                tl2 = (int)Convert.ToSingle(s[11]);
                tmax = Math.Max(Math.Max(Math.Max(th1, th2), tl1), tl2);
            }
            else if (s.Length > 4)
            {
                th1 = (int)Convert.ToSingle(s[8]);
                th2 = (int)Convert.ToSingle(s[9]);
                tl1 = th1;
                tl2 = th2;
                tmax = Math.Max(Math.Max(Math.Max(th1, th2), tl1), tl2);
            }
            else
                return "";
            if(Visible==true) {
                Refresh();
                this.Text =
                    "f " + (200 * r.X / Width - 39).ToString() +                               // 25,35,20,95
                    "," + (200 * (r.X + r.Width) / Width - 39).ToString() +
                    "," + (139 - 200 * (r.Y + r.Height) / Height).ToString() +
                    "," + (139 - 200 * r.Y / Height).ToString() + '\r'; ;
            return (this.Text);
            } else
                return "";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(rfGrad, rf);
            e.Graphics.DrawRectangle(Pens.Transparent, r);
            p1.X = 0;
            p2.X = Width;
            for (int i = 2; i < 8; ++i)
            {
                p1.Y = p2.Y = i * Height / 10;
                e.Graphics.DrawLine(Pens.DarkGray, p1, p2);
                if(i<7 && i>1)
                    e.Graphics.DrawString((140-20*i).ToString(),Font , Brushes.Black,p1);
            }
            p1.Y = 0;
            p2.Y = Height;
            for (int i = 2; i<8; ++i)
            {
                p1.X = p2.X = i * Width / 10;
                e.Graphics.DrawLine(Pens.DarkGray, p1, p2);
                if (i < 7 && i > 1)
                    e.Graphics.DrawString((20 * i-40).ToString(), Font, Brushes.Black, p1.X,rf.Y+rf.Height);
            }

            p1.X = 0;
            p1.Y = p2.Y = r.Y + r.Height;
            p2.X = r.X;
            e.Graphics.DrawLine(Pens.Black, p1, p2);

            p1.Y = r.Y;
            p1.X = r.X + r.Width;
            e.Graphics.DrawLine(Pens.Black, p2, p1);

            p2.X = Width;
            p2.Y = p1.Y;
            e.Graphics.DrawLine(Pens.Black, p1, p2);

            p1.X = p2.X = rf.X + (tmax * Width) / 10 / 20;
            p1.Y = rf.Y + rf.Height/10;
            p2.Y = rf.Y + 9*rf.Height/10;
            e.Graphics.DrawLine(Pens.Green, p1, p2);
            e.Graphics.DrawString(tmax.ToString(), Font, Brushes.Black, p1);

            p1.X = p2.X = rf.X + (th1 * Width) / 10 / 20;
            p1.Y = rf.Y;
            p2.Y = rf.Y - rf.Height/10;
            e.Graphics.DrawLine(Pens.Black, p1, p2);
            p1.X = p2.X = rf.X + (th2 * Width) / 10 / 20;
            p1.Y = rf.Y;
            p2.Y = rf.Y - rf.Height/10;
            e.Graphics.DrawLine(Pens.Red, p1, p2);
            p1.X = p2.X = rf.X + (tl1 * Width) / 10 / 20;
            p1.Y = rf.Y + 11*rf.Height/10;
            p2.Y = rf.Y + rf.Height;
            e.Graphics.DrawLine(Pens.Black, p1, p2);
            p1.X = p2.X = rf.X + (tl2 * Width) / 10 / 20;
            p1.Y = rf.Y + 11 * rf.Height / 10;
            p2.Y = rf.Y + rf.Height;
            e.Graphics.DrawLine(Pens.Red, p1, p2);

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
                xo = e.X;
                yo = e.Y;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int dx=e.X - xo;
            int dy=e.Y - yo;
            xo = e.X;
            yo = e.Y;
            
            if (e.Button == MouseButtons.Left)
            {
                r.X += dx;
                r.Y += dy;
                Invalidate();
            }
            if (e.Button == MouseButtons.Right)
            {
               r.Width += dx * 2;
               r.X -= dx;
               if (r.Width < rf.Width / 50)
                   r.Width = rf.Width / 50;
               if (r.Width > rf.Width)
                   r.Width = rf.Width;
                
                r.Y += dy;
                r.Height -= dy * 2;
                if (r.Height < rf.Height / 50)
                    r.Height = rf.Height / 50;
                if (r.Height > rf.Height)
                    r.Height = rf.Height;
                Invalidate();
            }

            if (r.X < rf.X)
                r.X = rf.X;
            if (r.X > rf.X + rf.Width - r.Width)
                r.X = rf.X + rf.Width - r.Width;

            if (r.Y < rf.Y)
                r.Y = rf.Y;
            if (r.Y > rf.Y + rf.Height - +r.Height)
                r.Y = rf.Y + rf.Height - +r.Height;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            rf = new Rectangle(Width / 5, Height / 5, Width / 2, Height / 2);
            r = new Rectangle(7 * Width / 20, Height / 5, 1 * Width / 20, 4 * Height / 10);
            rfGrad = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.Aquamarine, Color.OrangeRed, 0, false);
            rGrad = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.Aqua, Color.OrangeRed, 0, false);
        }

        private void FanEnter_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

    }
}
