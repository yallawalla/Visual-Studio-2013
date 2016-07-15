using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;


namespace qdf200
{
    public partial class Form1 : Form
    {
        Thread rx = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
                Console.WriteLine(port);
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Connect_DropDownOpening(object sender, EventArgs e)
        {
            Connect.DropDownItems.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
                if(port.IndexOf("ACM")>0 || port.IndexOf("USB")>0)
                    Connect.DropDownItems.Add(new ToolStripMenuItem(port));
        }

        private void Connect_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Text = "...connecting";
//            Properties.wtf.Default.port = e.ClickedItem.Text;
//            Properties.wtf.Default.Save();
            if(com.IsOpen)
            {
                com.Close();
                rx.Abort();
            }
            string s = e.ClickedItem.Text;
            int n = s.IndexOf("CO");
            s = s.Substring(s.IndexOf("tty"));
            try { 
                com.PortName = s;
                com.Open();
                rx = new Thread(new ThreadStart(rxData));
                rx.Start();
            }
            catch
            {
                Text = s + " not connected ...";
            }
 
        }

        void rxData()
        {
            byte tmpByte = (byte)com.ReadByte();
            Console.Write("start...");
            while (true)
            {
                string rxString = "";
                while (tmpByte != 255)
                {
                    rxString += ((char)tmpByte);
                    tmpByte = (byte)com.ReadByte();
                }
                if(rxString != "")
                    Console.Write(rxString);
                Thread.Sleep(5);
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (com.IsOpen == true)
            {
                com.Write(e.KeyChar.ToString());
            }
        }

    }
}
