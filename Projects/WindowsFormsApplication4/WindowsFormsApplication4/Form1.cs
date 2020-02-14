using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        int[] decode = new int[4096];
 
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 12; ++i)
            {
                decode[rot12(1, i)] = rot32(1, 2 * i);
                decode[rot12(3, i)] = rot32(2, 2 * i);
                decode[rot12(7, i)] = rot32(4, 2 * i);
                decode[rot12(15, i)] = rot32(8, 2 * i);
                decode[rot12(31, i)] = rot32(16, 2 * i);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            SolidBrush gray = new System.Drawing.SolidBrush(System.Drawing.Color.DarkGray);
            SolidBrush red = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            Graphics  g = this.CreateGraphics();
            int k=0;

            for (int i = 0; i < 12; ++i)
            {
                int locX = Width / 2 + Convert.ToInt32(100 * Math.Cos(2 * Math.PI * i / 12));
                int locY = Height / 2 - Convert.ToInt32(100 * Math.Sin(2 * Math.PI * i / 12));
               

                double fi0 = Math.Atan2(Height / 2 - y, x - Width / 2);
                double fi = Math.Atan2(locY - y, x - locX);
                double d0 = Math.Sqrt((Math.Pow((Width / 2 - x), 2) + Math.Pow((Height / 2 - y), 2)));
                double d = Math.Sqrt((Math.Pow((locX - x), 2) + Math.Pow((locY - y), 2)));

                Text = (fi0 / 2 / Math.PI * 360).ToString() + " // " + (fi / 2 / Math.PI * 360).ToString();

                if(Math.Abs(fi-fi0) < Math.PI/24 && d < d0)
//                if (Math.Sqrt((Math.Pow((locX - x), 2) + Math.Pow((locY - y), 2))) < 100)
                    k |= 1 << i;
            }
            k = decode[k];

           for (int i = 0; i < 24; ++i)
            {
                int locX = Width / 2 + Convert.ToInt32(100 * Math.Cos(2 * Math.PI * i / 24));
                int locY = Height / 2 - Convert.ToInt32(100 * Math.Sin(2 * Math.PI * i / 24));

                if ((k & (1<<i)) != 0)
                    g.FillEllipse(red, locX-10, locY-10, 20, 20);
                else
                    g.FillEllipse(gray, locX-10, locY-10, 20, 20);
            }
        }


        private int rot12(int i, int n)
        {
            return (((i << n) | (i >> (12 - n))) & 0xfff);
        }

        private int rot32(int i, int n)
        {
            return ((i << n) | (i >> (32 - n)));
        }


    }

}
