using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Collections;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        public int Ack;
        public Form3(SerialPort sParent, int nErase, int nProgram)
        {
            InitializeComponent();
            progressBarErase.Maximum = nErase;
            progressBarProgram.Maximum = nProgram;
            progressBarErase.Value = progressBarProgram.Value = 0;
        }

        public void Init(SerialPort sp, ArrayList p)
        {
            Ack = 1;
            sp.Write("!\r");
            while (Ack > 0)
                Application.DoEvents();
            Ack = progressBarErase.Maximum;
            sp.Write("e " + Convert.ToString(progressBarErase.Maximum, 16) + "\r");
            while (Ack > 0)
            {
                progressBarErase.Value = progressBarErase.Maximum - Ack;
                Application.DoEvents();
            }
            foreach (string s in p)
            {
                Ack = 999;
                sp.Write(s + "\r");
                progressBarProgram.Value++;
                while (Ack > 0)
                    Application.DoEvents();
            }
            sp.Write("!\r");
        }

        public void SetAck(string s)
        {
            if (s.IndexOf("(") > 0)
                Ack = Convert.ToInt32(s.Substring(s.IndexOf("(") + 1, 2), 16);
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}


