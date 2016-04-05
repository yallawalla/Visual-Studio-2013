using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pwmtab
{
    public partial class Form1 : Form
    {
        BOOL MessageBeep( UINT uType // beep type );
        [DllImport("User32.dll")]
        static extern Boolean MessageBeep(UInt32 beepType);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
