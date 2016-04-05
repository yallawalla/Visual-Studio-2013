
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ZedGraph;
using System.Collections;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        ArrayList cols = new ArrayList();
        Form1 fParent;
        int  n = 1, count =0;
        double[] t = new double[3000];
        double[] x = new double[3000];
        double[] y = new double[3000];
        double[] z = new double[3000];

        public Form2(Form1 fp)
        {
            InitializeComponent();
            fParent = fp;
        }

        public int sActive()
        {
            return n;
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            chart1.Left = 0;
            chart1.Top = menuStrip1.Height;
            chart1.Width = this.Width;
            chart1.Height = (this.Height - 2 * menuStrip1.Height);
            DrawGraph(null);
            fbar.Left = this.Width - fbar.Width-10;
            fbar.Top = 0;
            fbar.Height = this.Height;


        }


        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArrayList p = new ArrayList();
            Stream f = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "\\Documents and Settings/Mocnik/My Documents/Projects/charger/VB";
            openFileDialog1.Filter = "scope files (*.scp)|*.scp|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((f = openFileDialog1.OpenFile()) != null)
                    {
                        StreamReader fs = new StreamReader(f);
                        p.Clear();
                        while (fs.EndOfStream == false)
                        {
                            p.Add(fs.ReadLine().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                        }
                        DrawGraph(p);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream f = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "\\Documents and Settings/Mocnik/My Documents/Projects/charger/VB";
            saveFileDialog1.Filter = "scope files (*.scp)|*.scp|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((f = saveFileDialog1.OpenFile()) != null)
                    {
                        StreamWriter fs = new StreamWriter(f);
                        foreach (string[] s in cols)
                        {
                            fs.WriteLine(string.Format("{0:f}", s[0]) + "   " +
                                            string.Format("{0:f}", s[1]) + "   " +
                                                string.Format("{0:f}", s[2]) + "   " +
                                                    string.Format("{0:f}", s[3]));
                        }
                        fs.Close();
                        f.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not save file from disk. Original error: " + ex.Message);
                }
            }
        }




        private double[] Smooth(double[] x)
        {
            double vx = 0,xx=0;
            if (fbar.Value != 1)
            {
                for (int i = 0; i < x.Count(); ++i)
                {

                    vx += (x[i] - xx - 2 * vx) /fbar.Value;
                    xx += vx / fbar.Value;
                    x[i] = xx;
                }
            }
            return x;
        }


        private void Plot2(double[] t, double[] x, double[] y, double[] z)
        {
            // This is to remove all plots
            chart1.GraphPane.CurveList.Clear();
            chart1.GraphPane.XAxis.MajorGrid.IsVisible = true;
            chart1.GraphPane.YAxis.MajorGrid.IsVisible = true;
            chart1.GraphPane.Chart.Fill.Brush = new System.Drawing.SolidBrush(Color.Black);

            chart1.GraphPane.Fill.Color = Color.DarkGray;

            chart1.GraphPane.XAxis.MajorGrid.Color = Color.LightGoldenrodYellow;
            chart1.GraphPane.YAxis.MajorGrid.Color = Color.LightGoldenrodYellow;

            x = Smooth(x);
            y = Smooth(y);
            z = Smooth(z);
 
            // GraphPane object holds one or more Curve objects (or plots)
            GraphPane myPane = chart1.GraphPane;

            // PointPairList holds the data for plotting, X and Y arrays
            PointPairList spl1 = new PointPairList(t, x);
            PointPairList spl2 = new PointPairList(t, y);
            PointPairList spl3 = new PointPairList(t, z);  

            // Add cruves to myPane object
           LineItem myCurve1 = myPane.AddCurve("U", spl1, Color.LightGreen, SymbolType.None);
           LineItem myCurve2 = myPane.AddCurve("I", spl2, Color.Red, SymbolType.None);
           LineItem myCurve3 = myPane.AddCurve("P", spl3, Color.Cyan, SymbolType.None);

            myCurve1.Line.Width = 1.0F;
            myCurve2.Line.Width = 1.0F;
            myCurve3.Line.Width = 3.0F;
//            myCurve3.Line.Style = DashStyle.Dot;
            myPane.Title.Text = "PFM6";
            myPane.XAxis.Title.Text = "uS";
            myPane.YAxis.Title.Text = "V/ A/ kW";

            // I add all three functions just to be sure it refeshes the plot.  
            chart1.AxisChange();
            chart1.Invalidate();
            chart1.Refresh();
        }


        public void DrawGraph(ArrayList p)
        {
            int mode = 1;

            if (p == null) return;

            cols = p;
            double[] t = new double[p.Count];
            double[] x = new double[p.Count];
            double[] y = new double[p.Count];
            double[] z = new double[p.Count];

            int i = 0;

            foreach (string[] s in p)
            {
                double.TryParse(s[0], out x[i]);
                double.TryParse(s[1], out y[i]);
                double.TryParse(s[3], out z[i]);
                ++i;
            }

            if (mode > 0)
            {
                fft(y, y.Count(), 0);
                fft(z, z.Count(), 0);
            }
            Plot2(t, x, y, z);
        }


        //_________________________________________________________________
        /* fix_fft.c - Fixed-point in-place Fast Fourier Transform  */
        /*
          All data are fixed-point short integers, in which -32768
          to +32768 represent -1.0 to +1.0 respectively. Integer
          arithmetic is used for speed, instead of the more natural
          floating-point.

          For the forward FFT (time -> freq), fixed scaling is
          performed to prevent arithmetic overflow, and to map a 0dB
          sine/cosine wave (i.e. amplitude = 32767) to two -6dB freq
          coefficients. The return value is always 0.

          For the inverse FFT (freq -> time), fixed scaling cannot be
          done, as two 0dB coefficients would sum to a peak amplitude
          of 64K, overflowing the 32k range of the fixed-point integers.
          Thus, the fix_fft() routine performs variable scaling, and
          returns a value which is the number of bits LEFT by which
          the output must be shifted to get the actual amplitude
          (i.e. if fix_fft() returns 3, each value of fr[] and fi[]
          must be multiplied by 8 (2**3) for proper scaling.
          Clearly, this cannot be done within fixed-point short
          integers. In practice, if the result is to be used as a
          filter, the scale_shift can usually be ignored, as the
          result will be approximately correctly normalized as is.

          Written by:  Tom Roberts  11/8/89
          Made portable:  Malcolm Slaney 12/15/94 malcolm@interval.com
          Enhanced:  Dimitrios P. Bouras  14 Jun 2006 dbouras@ieee.org
        */
        //_________________________________________________________________
        /*
          Henceforth "short" implies 16-bit word. If this is not
          the case in your architecture, please replace "short"
          with a type definition which *is* a 16-bit word.
        */
        //_________________________________________________________________
        /*
          Since we only use 3/4 of N_WAVE, we define only
          this many samples, in order to conserve data space.
        */
        //_________________________________________________________________
        /*
          FIX_MPY() - fixed-point multiplication & scaling.
          Substitute inline assembly for hardware-specific
          optimization suited to a particluar DSP processor.
          Scaling ensures that result remains 16-bit.
        */
        //_________________________________________________________________
        public const short N_WAVE = 1024;         /* full length of Sinewave[] */
        public const short LOG2_N_WAVE = 10;      /* log2(N_WAVE) */
        public short[] Sinewave = {
              0,    201,    402,    603,    804,   1005,   1206,   1406,
           1607,   1808,   2009,   2209,   2410,   2610,   2811,   3011,
           3211,   3411,   3611,   3811,   4011,   4210,   4409,   4608,
           4807,   5006,   5205,   5403,   5601,   5799,   5997,   6195,
           6392,   6589,   6786,   6982,   7179,   7375,   7571,   7766,
           7961,   8156,   8351,   8545,   8739,   8932,   9126,   9319,
           9511,   9703,   9895,  10087,  10278,  10469,  10659,  10849,
          11038,  11227,  11416,  11604,  11792,  11980,  12166,  12353,
          12539,  12724,  12909,  13094,  13278,  13462,  13645,  13827,
          14009,  14191,  14372,  14552,  14732,  14911,  15090,  15268,
          15446,  15623,  15799,  15975,  16150,  16325,  16499,  16672,
          16845,  17017,  17189,  17360,  17530,  17699,  17868,  18036,
          18204,  18371,  18537,  18702,  18867,  19031,  19194,  19357,
          19519,  19680,  19840,  20000,  20159,  20317,  20474,  20631,
          20787,  20942,  21096,  21249,  21402,  21554,  21705,  21855,
          22004,  22153,  22301,  22448,  22594,  22739,  22883,  23027,
          23169,  23311,  23452,  23592,  23731,  23869,  24006,  24143,
          24278,  24413,  24546,  24679,  24811,  24942,  25072,  25201,
          25329,  25456,  25582,  25707,  25831,  25954,  26077,  26198,
          26318,  26437,  26556,  26673,  26789,  26905,  27019,  27132,
          27244,  27355,  27466,  27575,  27683,  27790,  27896,  28001,
          28105,  28208,  28309,  28410,  28510,  28608,  28706,  28802,
          28897,  28992,  29085,  29177,  29268,  29358,  29446,  29534,
          29621,  29706,  29790,  29873,  29955,  30036,  30116,  30195,
          30272,  30349,  30424,  30498,  30571,  30643,  30713,  30783,
          30851,  30918,  30984,  31049,  31113,  31175,  31236,  31297,
          31356,  31413,  31470,  31525,  31580,  31633,  31684,  31735,
          31785,  31833,  31880,  31926,  31970,  32014,  32056,  32097,
          32137,  32176,  32213,  32249,  32284,  32318,  32350,  32382,
          32412,  32441,  32468,  32495,  32520,  32544,  32567,  32588,
          32609,  32628,  32646,  32662,  32678,  32692,  32705,  32717,
          32727,  32736,  32744,  32751,  32757,  32761,  32764,  32766,
          32767,  32766,  32764,  32761,  32757,  32751,  32744,  32736,
          32727,  32717,  32705,  32692,  32678,  32662,  32646,  32628,
          32609,  32588,  32567,  32544,  32520,  32495,  32468,  32441,
          32412,  32382,  32350,  32318,  32284,  32249,  32213,  32176,
          32137,  32097,  32056,  32014,  31970,  31926,  31880,  31833,
          31785,  31735,  31684,  31633,  31580,  31525,  31470,  31413,
          31356,  31297,  31236,  31175,  31113,  31049,  30984,  30918,
          30851,  30783,  30713,  30643,  30571,  30498,  30424,  30349,
          30272,  30195,  30116,  30036,  29955,  29873,  29790,  29706,
          29621,  29534,  29446,  29358,  29268,  29177,  29085,  28992,
          28897,  28802,  28706,  28608,  28510,  28410,  28309,  28208,
          28105,  28001,  27896,  27790,  27683,  27575,  27466,  27355,
          27244,  27132,  27019,  26905,  26789,  26673,  26556,  26437,
          26318,  26198,  26077,  25954,  25831,  25707,  25582,  25456,
          25329,  25201,  25072,  24942,  24811,  24679,  24546,  24413,
          24278,  24143,  24006,  23869,  23731,  23592,  23452,  23311,
          23169,  23027,  22883,  22739,  22594,  22448,  22301,  22153,
          22004,  21855,  21705,  21554,  21402,  21249,  21096,  20942,
          20787,  20631,  20474,  20317,  20159,  20000,  19840,  19680,
          19519,  19357,  19194,  19031,  18867,  18702,  18537,  18371,
          18204,  18036,  17868,  17699,  17530,  17360,  17189,  17017,
          16845,  16672,  16499,  16325,  16150,  15975,  15799,  15623,
          15446,  15268,  15090,  14911,  14732,  14552,  14372,  14191,
          14009,  13827,  13645,  13462,  13278,  13094,  12909,  12724,
          12539,  12353,  12166,  11980,  11792,  11604,  11416,  11227,
          11038,  10849,  10659,  10469,  10278,  10087,   9895,   9703,
           9511,   9319,   9126,   8932,   8739,   8545,   8351,   8156,
           7961,   7766,   7571,   7375,   7179,   6982,   6786,   6589,
           6392,   6195,   5997,   5799,   5601,   5403,   5205,   5006,
           4807,   4608,   4409,   4210,   4011,   3811,   3611,   3411,
           3211,   3011,   2811,   2610,   2410,   2209,   2009,   1808,
           1607,   1406,   1206,   1005,    804,    603,    402,    201,
              0,   -201,   -402,   -603,   -804,  -1005,  -1206,  -1406,
          -1607,  -1808,  -2009,  -2209,  -2410,  -2610,  -2811,  -3011,
          -3211,  -3411,  -3611,  -3811,  -4011,  -4210,  -4409,  -4608,
          -4807,  -5006,  -5205,  -5403,  -5601,  -5799,  -5997,  -6195,
          -6392,  -6589,  -6786,  -6982,  -7179,  -7375,  -7571,  -7766,
          -7961,  -8156,  -8351,  -8545,  -8739,  -8932,  -9126,  -9319,
          -9511,  -9703,  -9895, -10087, -10278, -10469, -10659, -10849,
         -11038, -11227, -11416, -11604, -11792, -11980, -12166, -12353,
         -12539, -12724, -12909, -13094, -13278, -13462, -13645, -13827,
         -14009, -14191, -14372, -14552, -14732, -14911, -15090, -15268,
         -15446, -15623, -15799, -15975, -16150, -16325, -16499, -16672,
         -16845, -17017, -17189, -17360, -17530, -17699, -17868, -18036,
         -18204, -18371, -18537, -18702, -18867, -19031, -19194, -19357,
         -19519, -19680, -19840, -20000, -20159, -20317, -20474, -20631,
         -20787, -20942, -21096, -21249, -21402, -21554, -21705, -21855,
         -22004, -22153, -22301, -22448, -22594, -22739, -22883, -23027,
         -23169, -23311, -23452, -23592, -23731, -23869, -24006, -24143,
         -24278, -24413, -24546, -24679, -24811, -24942, -25072, -25201,
         -25329, -25456, -25582, -25707, -25831, -25954, -26077, -26198,
         -26318, -26437, -26556, -26673, -26789, -26905, -27019, -27132,
         -27244, -27355, -27466, -27575, -27683, -27790, -27896, -28001,
         -28105, -28208, -28309, -28410, -28510, -28608, -28706, -28802,
         -28897, -28992, -29085, -29177, -29268, -29358, -29446, -29534,
         -29621, -29706, -29790, -29873, -29955, -30036, -30116, -30195,
         -30272, -30349, -30424, -30498, -30571, -30643, -30713, -30783,
         -30851, -30918, -30984, -31049, -31113, -31175, -31236, -31297,
         -31356, -31413, -31470, -31525, -31580, -31633, -31684, -31735,
         -31785, -31833, -31880, -31926, -31970, -32014, -32056, -32097,
         -32137, -32176, -32213, -32249, -32284, -32318, -32350, -32382,
         -32412, -32441, -32468, -32495, -32520, -32544, -32567, -32588,
         -32609, -32628, -32646, -32662, -32678, -32692, -32705, -32717,
         -32727, -32736, -32744, -32751, -32757, -32761, -32764, -32766,
        };

        public void fft(double[] p, int nn, int isign)
        {
            int i, j;
            for (i = 1; i < nn; i = i << 1) ;
            short[] freal = new short[i];
            short[] fim = new short[i];
            for (j = 0; j < i; ++j)
            {
                freal[j] = fim[j] = 0;
                if (j < nn)
                    freal[j] = (short)p[j];
            }
            fix_fft(freal, fim, 10, 0);
            for (j = 20; j < i - 20; ++j)
            {
                freal[j] = fim[j] = 0;
                fim[j] = fim[j] = 0;
            }
            fix_fft(freal, fim, 10, 1);
            for (j = 1; j < nn; ++j)
                //                p[j] = (freal[j] * freal[j] + fim[j] * fim[j]);
                p[j] = freal[j];
        }

        public short FIX_MPY(short a, short b)
        {
            return ((short)(((a) * (b) + 0x4000) >> 15));
        }
        //_________________________________________________________________
        /*
          fix_fft() - perform forward/inverse fast Fourier transform.
          fr[n],fi[n] are real and imaginary arrays, both INPUT AND
          RESULT (in-place FFT), with 0 <= n < 2**m; set inverse to
          0 for forward transform (FFT), or 1 for iFFT.
        */
        //_________________________________________________________________
        public short fix_fft(short[] fr, short[] fi, short m, short inverse)
        {
            short mr, nn, i, j, l, k, istep, n, scale, shift;
            short qr, qi, tr, ti, wr, wi;

            n = (short)(1 << m);

            /* max FFT size = N_WAVE */
            if (n > N_WAVE)
                return -1;

            mr = 0;
            nn = (short)(n - 1);
            scale = 0;

            /* decimation in time - re-order data */

            for (m = 1; m <= nn; ++m)
            {
                l = n;
                do
                {
                    l >>= 1;
                } while (mr + l > nn);
                mr = (short)((mr & (l - 1)) + l);

                if (mr <= m)
                    continue;
                tr = fr[m];
                fr[m] = fr[mr];
                fr[mr] = tr;
                ti = fi[m];
                fi[m] = fi[mr];
                fi[mr] = ti;
            }

            l = 1;
            k = LOG2_N_WAVE - 1;
            while (l < n)
            {
                if (inverse != 0)
                {
                    shift = 0;
                    for (i = 0; i < n; ++i)
                    {
                        j = fr[i];
                        if (j < 0)
                            j = (short)-j;
                        m = fi[i];
                        if (m < 0)
                            m = (short)-m;
                        if (j > 16383 || m > 16383)
                        {
                            shift = 1;
                            break;
                        }
                    }
                    if (shift != 0)
                        ++scale;
                }
                else
                {
                    /*
                        fixed scaling, for proper normalization --
                        there will be log2(n) passes, so this results
                        in an overall factor of 1/n, distributed to
                        maximize arithmetic accuracy.
                    */
                    shift = 1;
                }
                /*
                    it may not be obvious, but the shift will be
                    performed on each data point exactly once,
                    during this pass.
                */
                istep = (short)(l << 1);
                for (m = 0; m < l; ++m)
                {
                    j = (short)(m << k);
                    wr = Sinewave[j + N_WAVE / 4];
                    wi = (short)(-Sinewave[j]);
                    if (inverse != 0)
                        wi = (short)-wi;
                    if (shift != 0)
                    {
                        wr >>= 1;
                        wi >>= 1;
                    }
                    for (i = m; i < n; i += istep)
                    {
                        j = (short)(i + l);
                        tr = (short)(FIX_MPY(wr, fr[j]) - FIX_MPY(wi, fi[j]));
                        ti = (short)(FIX_MPY(wr, fi[j]) + FIX_MPY(wi, fr[j]));
                        qr = fr[i];
                        qi = fi[i];
                        if (shift != 0)
                        {
                            qr >>= 1;
                            qi >>= 1;
                        }
                        fr[j] = (short)(qr - tr);
                        fi[j] = (short)(qi - ti);
                        fr[i] = (short)(qr + tr);
                        fi[i] = (short)(qi + ti);
                    }
                }
                --k;
                l = istep;
            }
            return scale;
        }

        private void Simmer1_Click(object sender, EventArgs e)
        {
            n = 1;
            fParent.ScopeMenu_Click(sender, e);
        }

        private void Simmer2_Click(object sender, EventArgs e)
        {
            n = 2;
            fParent.ScopeMenu_Click(sender, e);
        }

        public void   ScopeBinary(int v, int to)
        {
            if (v < 0)
            {
                count = count / 4;
                while (count < 3000)
                {
                    t[count] = t[count - 1];
                    x[count] = x[count - 1];
                    y[count] = y[count - 1];
                    z[count] = z[count - 1];
                    ++count;
                }

                Plot2(t,x, y, z);
                count = 0;
            }
            else
            {
                switch (count % 4)
                {
                    case 0:
                        t[count / 4] = count * to/60 / 4;
                        x[count++ / 4] = v * Constants.PFMU;
                        break;
                    case 1:
                        if (v > 127)
                            v -= 256;
                        x[count++ / 4] += 256.0 * v * Constants.PFMU;
                        break;
                    case 2:
                       y[count++ / 4] = v * Constants.PFMI;
                        break;
                    case 3:
                        if (v > 127)
                            v -= 256;
                        y[count / 4] += 256.0 * v * Constants.PFMI;
                        z[count / 4] = x[count / 4] * y[count / 4]/1000;
                        ++count;
                        break;
                }
            }
        }

        private void fbar_Scroll(object sender, ScrollEventArgs e)
        {

        }
    }
}




