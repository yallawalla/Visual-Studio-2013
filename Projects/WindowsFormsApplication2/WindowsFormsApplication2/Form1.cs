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
        int[] Utab={260,320,380,400};
        int[] uStab={20, 30, 80, 99};

        private int __fit(int to,  int[] t,  int[] ft) {
int		f3=(ft[3]*(t[0]-to)-ft[0]*(t[3]-to)) / (t[0]-t[3]);
int		f2=(ft[2]*(t[0]-to)-ft[0]*(t[2]-to)) / (t[0]-t[2]);
int		f1=(ft[1]*(t[0]-to)-ft[0]*(t[1]-to)) / (t[0]-t[1]);
		f3=(f3*(t[1]-to) - f1*(t[3]-to)) / (t[1]-t[3]);
		f2=(f2*(t[1]-to)-f1*(t[2]-to)) / (t[1]-t[2]);
		return(f3*(t[2]-to)-f2*(t[3]-to)) / (t[2]-t[3]);
}


        public void add(double i, double j, double k)
        {
            chart1.Series["p1"].Points.AddXY(i,j);
            chart1.Series["p2"].Points.AddXY(i,k);
        }
        public void clear()
        {
            chart1.Series["p1"].Points.Clear();
            chart1.Series["p2"].Points.Clear();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            chart1.Left = 0;
            chart1.Top = 0;
            chart1.Width = Width;
            chart1.Height = Height;
        }

        public Form1()
        {
            InitializeComponent();
//            chart1.ChartAreas[0].AxisX.Minimum = Utab[0];
//            chart1.ChartAreas[0].AxisX.Maximum = Utab[Utab.Length-1];
            for (int i = Utab[0]/2; i < 2*Utab[Utab.Length - 1]; ++i)
                this.add(i,__fit(i,Utab,uStab),0);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
