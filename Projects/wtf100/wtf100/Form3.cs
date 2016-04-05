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
    public partial class Form3 : Form
    {
        Form1 f = null;
        pShape scope = null;
        int[] adc;


        public Form3(Form1 fparent)
        {
            InitializeComponent();
            f = fparent;
    
            a1.MaxValue = a2.MaxValue = a3.MaxValue = 5;
            a1.ScaleLinesMajorStepValue = a2.ScaleLinesMajorStepValue = a3.ScaleLinesMajorStepValue = a2.MaxValue / 5;
            a1.Range_Idx = a2.Range_Idx = a3.Range_Idx = 0;
            a1.RangeStartValue = a2.RangeStartValue = a3.RangeStartValue = 0;
            a1.RangeEndValue = a2.RangeEndValue = a3.RangeEndValue = a2.MaxValue * 8 / 20;
            a1.Range_Idx = a2.Range_Idx = a3.Range_Idx = 1;
            a1.RangeStartValue = a2.RangeStartValue = a3.RangeStartValue = a2.MaxValue * 8 / 20;
            a1.RangeEndValue = a2.RangeEndValue = a3.RangeEndValue = a2.MaxValue * 8 / 10;
            a1.Range_Idx = a2.Range_Idx = a3.Range_Idx = 2;
            a1.RangeStartValue = a2.RangeStartValue = a3.RangeStartValue = a2.MaxValue * 8 / 10;
            a1.RangeEndValue = a2.RangeEndValue = a3.RangeEndValue = a2.MaxValue;

            pAIR.Value = pAIR.Minimum = (int)Properties.wtf.Default.a3min * 50 / 0xffff;
            pAIR.Maximum = (int)Properties.wtf.Default.a3max;

            pH2O.Value = pH2O.Minimum = 0;
            pH2O.Maximum = (int)Properties.wtf.Default.a2max*5/4;
            pH2O.TickFrequency = pH2O.Maximum / 5;
            f.WritePort("+D 50\r");
        }

        public void parse(string s)
        {
            try
            {
                adc = Array.ConvertAll(s.Split(','), Int32.Parse);
                float k = (4 - 1) / (Properties.wtf.Default.a1max - Properties.wtf.Default.a1min);
                float n = 1 - (4 - 1) / (Properties.wtf.Default.a1max - Properties.wtf.Default.a1min) * Properties.wtf.Default.a1min;
                //.Value += (k * adc[0] + n - a1.Value) / 2;
                a1.Value += (k * adc[2] + n - a1.Value) / 2;
                
                k = (4 - 1) / (Properties.wtf.Default.a2max - Properties.wtf.Default.a2min);
                n = 1 - (4 - 1) / (Properties.wtf.Default.a2max - Properties.wtf.Default.a2min) * Properties.wtf.Default.a2min;
                //a2.Value += (k * adc[2] + n - a2.Value) / 2;
                a2.Value += (k * adc[1] + n - a2.Value) / 2;

                k = (4 - 1) / (Properties.wtf.Default.a3max - Properties.wtf.Default.a3min);
                n = 1 - (4 - 1) / (Properties.wtf.Default.a3max - Properties.wtf.Default.a3min) * Properties.wtf.Default.a3min;
                //a3.Value += (k * adc[1] + n - a3.Value) / 2;
                a3.Value += (k * adc[3] + n - a3.Value) / 2;
                
                t1.Text = a1.Value.ToString("0.00");
                t2.Text = a2.Value.ToString("0.00");
                t3.Text = a3.Value.ToString("0.00");
                
                //if (scope != null)
                //{
                //    try
                //    {
                //        scope.Show();
                //        scope.parse(s);
                //    }
                //    catch
                //    {
                //        scope.Dispose();
                //        scope = null;
                //    }
                //}
            }
            catch { }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void VH2O_CheckedChanged(object sender, EventArgs e)
        {
            if (VH2O.Checked != true)
                f.WritePort("-w\r");
            else
                f.WritePort("+w\r");
        }

         private void a1_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
                Properties.wtf.Default.a1min = adc[2];
            if (Control.ModifierKeys == Keys.Shift)
                Properties.wtf.Default.a1max = adc[2];
            Properties.wtf.Default.Save();
        }

        private void a2_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
                Properties.wtf.Default.a2min =  adc[1];
            if (Control.ModifierKeys == Keys.Shift)
                Properties.wtf.Default.a2max = adc[1];
            Properties.wtf.Default.Save();
        }

        private void a3_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
                Properties.wtf.Default.a3min = adc[3];
            if (Control.ModifierKeys == Keys.Shift)
                Properties.wtf.Default.a3max = adc[3];
            Properties.wtf.Default.Save();
        }

 
        private void pH2O_Scroll(object sender, EventArgs e)
        {
            f.WritePort("p " + (pH2O.Value).ToString() + "\r");
        }

        private void pAIR_Scroll(object sender, EventArgs e)
        {
            f.WritePort("a" + (pAIR.Value).ToString() + "\r");
        }

        private void a2_DoubleClick(object sender, EventArgs e)
        {

        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            f.WritePort("-D 10\r");
        }
    }

}
