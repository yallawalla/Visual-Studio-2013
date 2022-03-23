using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tiltblock
{
    public partial class Form1 : Form
    {
        SharpGLForm parent;
        public Form1(SharpGLForm f)
        {
            InitializeComponent();
            parent = f;
        }

        public void plot(double p1, double p2)
        {
            if(chart1.Series["Series1"].Points.Count > 500)
            {
                chart1.Series["Series1"].Points.RemoveAt(0);
                chart1.Series["Series2"].Points.RemoveAt(0);
            }
            chart1.Series["Series1"].Points.Add(p1);
            chart1.Series["Series2"].Points.Add(p2);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.offset();
        }
    }
}
