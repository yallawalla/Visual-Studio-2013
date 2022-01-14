using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rk
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int k = 0; k < 10000; k += 1000)
            {
                chart1.Series.Add("s" + k.ToString());
                chart1.Series["s" + k.ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
 //               for (int n = 100; n < 1000; n += 10)
 //                   chart1.Series["s" + k.ToString()].Points.AddXY(n, drag(n, k));
            }
        }

        public void hit(double z, double y)
        {
//            chart2.Series["Series1"].Points.AddXY(z, y);   
        }

    }
}
