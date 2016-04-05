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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int rotX(int x, int y, double f)
        {
            return (int)(x * Math.Cos(f) - y * Math.Sin(f));
        }

        private int rotY(int x, int y, double f)
        {
            return (int)(y * Math.Cos(f) + x * Math.Sin(f));
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int r = 150;
            Pen pBlack = new Pen(Color.Black, 2);
            Pen pRed = new Pen(Color.Red, 1);

            e.Graphics.DrawLine(pBlack, this.Width / 2, 0, this.Width / 2, this.Height);
            e.Graphics.DrawLine(pBlack, 0, this.Height / 2, this.Width, this.Height / 2);
            int rr = 3*r/4;
            e.Graphics.DrawEllipse(pBlack, this.Width / 2-rr, this.Height / 2-rr, 2*rr, 2*rr);


            
            for (int i=0; i<1024; ++i)
            {
         //       int xx = (int)(r * Math.Sqrt(2) * Math.Cos((2 * Math.PI * i) / 1024));
                int xx = (int)((Math.Abs(512 - i) - 256) * r * Math.Sqrt(2) / 256);
                int yy = 3 * ((int)(r * Math.Sin((2 * Math.PI * i) / 1024)) - (int)(r / 5 * Math.Sin((6 * Math.PI * i) / 1024))) / 4;
                e.Graphics.DrawEllipse(pRed, this.Width / 2 + rotX(xx, yy, Math.PI / 4), this.Height / 2 - rotY(xx, yy, Math.PI / 4), 2, 2);
         //       e.Graphics.DrawEllipse(pRed, this.Width / 2 + xx, this.Height / 2 - yy, 2, 2);

                //if (i > 0 && i < 128)
                //{
                //    e.Graphics.DrawEllipse(pBlack, this.Width / 2 + r, this.Height / 2 - Convert.ToInt32(r * Math.Tan((2 * Math.PI * i) / 1024)), 2, 2);
                //    continue;
                //}
                //if (i > 0 && i < 265)
                //{
                //    e.Graphics.DrawEllipse(pBlack, this.Width / 2 + Convert.ToInt32(r / Math.Tan((2 * Math.PI * i) / 1024)), this.Height / 2 - r, 2, 2);
                //    continue;
                //}
                //if (i > 512 && i < (512+128))
                //{
                //    e.Graphics.DrawEllipse(pBlack, this.Width / 2 - r, this.Height / 2 - Convert.ToInt32(-r * Math.Tan((2 * Math.PI * i) / 1024)), 2, 2);
                //    continue;
                //}
                //if (i > 512 && i < (512 + 256))
                //{
                //    e.Graphics.DrawEllipse(pBlack, this.Width / 2 + Convert.ToInt32(-r / Math.Tan((2 * Math.PI * i) / 1024)), this.Height / 2 + r, 2, 2);
                //    continue;
                //}

        //        e.Graphics.DrawEllipse(pBlack, this.Width / 2 + Convert.ToInt32(r * Math.Cos((2 * Math.PI * i) / 1024)), this.Height / 2 - Convert.ToInt32(r * Math.Sin((2 * Math.PI * i) / 1024)), 2, 2);

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
