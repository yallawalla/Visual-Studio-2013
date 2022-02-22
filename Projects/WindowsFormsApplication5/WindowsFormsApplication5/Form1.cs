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
        int posX, posY, velX, velY, accX, accY;
        int estX, estY, estVX, estVY;
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (posX != 0 && posY != 0)
            {
                accX = Cursor.Position.X - posX - velX;
                accY = Cursor.Position.Y - posY - velY;
                velX = Cursor.Position.X - posX;
                velY = Cursor.Position.Y - posY;
                posX = Cursor.Position.X;
                posY = Cursor.Position.Y;
            }

            estVX += accX;
            estVY += accY;
            estX += estVX;
            estY += estVY;
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawEllipse(Pens.Yellow, estX - 5, estY - 5, 10, 10);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            velX = velY = accX = accY = 0;

            estVX = estVY = 0;
            estX = e.X;
            estY = e.Y;
        }
    }
}
