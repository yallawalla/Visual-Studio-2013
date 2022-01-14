using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {
        Pen blackPen = new Pen(Color.Black, 1);
        Brush red = new SolidBrush(Color.Red);
        Brush green = new SolidBrush(Color.Green);
        Random rnd = new Random();
        double nom = 0;

        public Form1()
        {
            InitializeComponent();
            Width = Height = 500;
            Show();
            Rectangle rect = new Rectangle(50, 50, Width - 100, Height - 100);
            using (Graphics g = this.CreateGraphics())
            {
                g.DrawRectangle(blackPen, rect);
                g.DrawEllipse(blackPen, rect);
               for (int i = 0; i < 1000000; ++i)
                {
                    int x = rnd.Next(0, 400)-200;
                    int y = rnd.Next(0, 400)-200;
                    if (x * x + y * y > 200 * 200) {
                        g.FillRectangle(green, x+250, y+250, 1, 1);
                    }
                   else {
                        ++nom;
                        g.FillRectangle(red, x+250, y+250, 1, 1);
                    }
                    Text = (4*nom / i).ToString() ;
                }
            }
        }

    }   
}

