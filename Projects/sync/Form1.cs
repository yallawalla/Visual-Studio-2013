using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sync
{
    public partial class Form1 : Form
    {
        Random rnd = new Random();
        int dt = 16000;

        int     N=300;
        int 	count, offset, upperq;
        ushort	syncval;
        float   sX=0, sX2=0, sY=0, sXY=0, a, b;
        struct  pnt { public float 	x,y;  }
        pnt[] p;

/*******************************************************************************
* Function Name	: 
* Description		: 
* Output				:
* Return				:
*******************************************************************************/
uint	eval(uint t) {
	return (uint)(t-syncval-t*b/(float)(65536.0*128.0));
}
/*******************************************************************************
* Function Name	: 
* Description		: 
* Output				:
* Return				:
*******************************************************************************/
float	linreg(float x, float y) {
	int	i = count++ % N;
	int	j = count;
	
	if (count > N) {
		j = N;
		sX -= p[i].x;
		sX2 -= p[i].x*p[i].x;
		sY -= p[i].y;
		sXY -= p[i].x*p[i].y;
	}

	p[i].x = x;
	p[i].y = y;

	sX += x;
	sX2 += x*x;
	sY += y;
	sXY += x*y;
// y=b*x + a
	if (j > 1) 
		b = (j*sXY/sX-sY)/(j*sX2/sX-sX);
	else 
		b=0;
	a = (sY - b*sX)/j;
	
	return a+b*x;
}
/*******************************************************************************
* Function Name	: 
* Description		: 
* Output				:
* Return				:
*******************************************************************************/
        public ushort	sync(ushort y) {
            if (count > 3*N) {
                float a0=a+b*count, b0=b;
                sX = sX2 = sY = sXY = offset = upperq = count = 0;
                while(count < N)
                    sync((ushort)(a0 + b0 * (count-N) % 0xffff));

                chart1.ChartAreas[0].AxisX.Maximum = 300;
                chart1.ChartAreas[0].AxisX.Minimum = 100;
                chart1.Series["Series1"].Points.Clear();
                chart1.Series["Series2"].Points.Clear();


	        }
	        if(count > 0) {
		        if(upperq != 0 && y < 0x4000)
			        offset += 0x10000;
		        else if(upperq==0 && y > 0xc000)
			        offset -= 0x10000;
	        }
	        if(y > 0x8000)
			        upperq=1;
	        else
			        upperq=0;
	        syncval=(ushort)linreg(count, y+offset);
	        return syncval;
        }



        public Form1()
        {
            InitializeComponent();
            p=new pnt[N];
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            uint a = eval(0x3910);
            uint b= eval(0x3910+10);

            g.DrawLine(new Pen(Color.Yellow, 1), 0, Height / 2 - a, Width, Height / 2 -b);

            for (int k = 0; k < Height; k+=100)
                g.DrawLine(new Pen(Color.Black, 1), 0, k, Width, k);
            for (int k = 0; k < Width; k += 100)
                g.DrawLine(new Pen(Color.Black, 1), k, 0, k, Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
         //   dt += 200;
            ushort p = (ushort)(dt + rnd.Next(-200, 200));
            ushort pp = sync(p);
            chart1.Series["Series1"].Points.AddXY(count, pp+(p-pp));
            chart1.Series["Series2"].Points.AddXY(count,pp);
            chart1.ChartAreas[0].AxisY.Maximum = 16200;
            chart1.ChartAreas[0].AxisY.Minimum = 15800;
        }
    }
}
