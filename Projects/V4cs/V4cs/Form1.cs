using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;


namespace V4
{
    public partial class Form1 : Form
    {
        public delegate void dllCallback(int n, IntPtr p);
        
        [DllImport(@"V4.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _dllmain();
        [DllImport(@"V4.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _dllButtonPress(int n);
        [DllImport(@"V4.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _dllUseCallback(dllCallback p);

        Thread      V4dll;
        string      LcdBuffer=null;
        string      ExtLcdBuffer = null;
        
        public Form1()
        {
            InitializeComponent();

            Lcd.Font = new Font(FontFamily.GenericMonospace, 2*Lcd.Font.Size);

            V4dll = new Thread(_dllmain);
            V4dll.Start();
            _dllUseCallback(new dllCallback(V4Call));
        }

        private void V4Call(int n,IntPtr p)
        {
            try
            {
                if (n == -1)
                    Application.Exit();
                if (n == 0)
                    LcdBuffer = Marshal.PtrToStringAnsi(p, 82);
                if (n == 1)
                    ExtLcdBuffer = Marshal.PtrToStringAnsi(p, 32);
                if(LcdBuffer != null && ExtLcdBuffer != null)
                    Invoke(new EventHandler(LcdShow));
            }
            catch { }
        }

        private void LcdShow(object sender, EventArgs e)
        {
            try
            {
                Lcd.Text = 
                    ExtLcdBuffer.Substring(0, 8)+LcdBuffer.Substring(0, 20)    + "\r\n" +
                    ExtLcdBuffer.Substring(8, 8)  +LcdBuffer.Substring(20, 20) + "\r\n" +
                    ExtLcdBuffer.Substring(16, 8) +LcdBuffer.Substring(40, 20) + "\r\n" +
                    ExtLcdBuffer.Substring(24, 8)+LcdBuffer.Substring(60, 20) ;

                    Lcd.SelectionStart = 9 * (int)(LcdBuffer[80] / 20) + LcdBuffer[80] + 8;
                    Lcd.SelectionLength = 0;

                    if (LcdBuffer[81] == ' ')
                    {
                        Lcd.SelectionStart = 0;
                    }

                    if(LcdBuffer[81]==220) {
                     Lcd.SelectionLength = 1;
                    }
            }
            catch {
            }
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    _dllButtonPress(e.KeyValue);
                    break;
            }
            switch (e.KeyCode)
            {
                case Keys.F1:
                    _dllButtonPress(129);
                    break;
                case Keys.F2:
                    _dllButtonPress(130);
                    break;
                case Keys.F3:
                    _dllButtonPress(131);
                    break;
                case Keys.F4:
                    _dllButtonPress(132);
                    break;
                case Keys.Enter:
                    _dllButtonPress(128);
                    break;
                case Keys.F12:
                    _dllButtonPress(142);
                    break;
                case Keys.Left:
                    _dllButtonPress(133);
                    break;
                case Keys.Right:
                    _dllButtonPress(134);
                    break;
                case Keys.Up:
                    _dllButtonPress(135);
                    break;
                case Keys.Down:
                    _dllButtonPress(136);
                    break;
                case Keys.Delete:
                    _dllButtonPress(137);
                    break;
                case Keys.Subtract:
                    _dllButtonPress(140);
                    break;
                case Keys.Escape:
                    _dllButtonPress(141);
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
