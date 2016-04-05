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
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string pString = "";
        string rxString = "";
        string txString = "";
        StreamWriter log = null;
        string[] iapFile = null;
        Tcp tcp = null;
        Udp udp=null;
        Form2 pulse=null;
        Form3 valve = null;
        pShape pyro = null; 

        public Form1()
        {
            InitializeComponent();
            text.Width = Width-10;
            text.Height = Height-50;
            text.Font = Properties.wtf.Default.font;
            Properties.wtf.Default.logfile = "wtf" + (1 + Convert.ToInt32(Properties.wtf.Default.logfile.Substring(3, 5), 16)).ToString("X5") + ".log";
            Properties.wtf.Default.Save();
            log = new StreamWriter(Properties.wtf.Default.logfile);
            log.AutoFlush = true;
            OpenPort(Properties.wtf.Default.port);
        }
        
 
        //.................. menu items 
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.ActiveForm.Close();
        }

        private void Connect_Opening(object sender, EventArgs e)
        {
            connectToolStripMenuItem.DropDownItems.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
                connectToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(port));
            foreach (string s in Properties.wtf.Default.ip)
            {
                connectToolStripMenuItem.DropDownItems.Add(s);
            }
        }
        //.................. serial port 

        private void Connect_Clicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Text = "...connecting";
            Properties.wtf.Default.port = e.ClickedItem.Text;
            Properties.wtf.Default.Save();
            if(com.IsOpen)
                com.Close();
            if(tcp!=null)
                if (tcp.isOpen)
                    tcp.Close();
            OpenPort(Properties.wtf.Default.port);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ContextMenu rclick = new ContextMenu();
            MenuItem copy = new MenuItem();
            MenuItem paste = new MenuItem();
            MenuItem clear = new MenuItem();
            MenuItem selectall = new MenuItem();
            rclick.MenuItems.AddRange(new MenuItem[] { copy, paste, clear, selectall});
            copy.Index = 0;
            copy.Text = "Copy";
            copy.Click += copy_Click;
            paste.Index = 1;
            paste.Text = "Paste";
            paste.Click += paste_Click;
            clear.Index = 2;
            clear.Text = "Clear";
            clear.Click += clear_Click;
            selectall.Index = 3;
            selectall.Text = "Select all";
            selectall.Click += selectall_Click;
            text.ContextMenu = rclick;
            text.Width = Width-10;
            text.Height = Height - 50;
            bar.Width = Width - Reset.Size.Width - bar.Left - 20;
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            com.RtsEnable = true;
            Thread.Sleep(300);
            com.RtsEnable = false;
        }
         
        private void Font_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = text.Font;
            DialogResult dr = fd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                text.Font = fd.Font;
                Properties.wtf.Default.font = fd.Font;
                Properties.wtf.Default.Save();
            }

        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.InitialDirectory = Directory.GetCurrentDirectory();
            fd.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                File.Copy(fd.FileName, "log.bak");
                text.LoadFile("log.bak", RichTextBoxStreamType.PlainText);
                File.Delete("log.bak");
            }
        }

        void copy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(text.SelectedText);
            }
            catch
            {
                Clipboard.SetText("\r");
            }
        }

        void paste_Click(object sender, EventArgs e)
        {
            WritePort(Clipboard.GetText());
        }

        void clear_Click(object sender, EventArgs e)
        {
            text.Clear();
        }

        void selectall_Click(object sender, EventArgs e)
        {
            try
            {
                text.SelectAll();
            }
            catch
            {
            }
        }
        
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            WritePort(e.KeyChar.ToString());
            e.Handled = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                WritePort("\x1b[11~");
            if (e.KeyCode == Keys.F2)
                WritePort("\x1b[12~");
            if (e.KeyCode == Keys.F3)
                WritePort("\x1b[13~");
            if (e.KeyCode == Keys.F4)
                WritePort("\x1b[14~");
            if (e.KeyCode == Keys.F5)
                WritePort("\x1b[15~");
            if (e.KeyCode == Keys.F6)
                WritePort("\x1b[17~");
            if (e.KeyCode == Keys.F7)
                WritePort("\x1b[18~");
            if (e.KeyCode == Keys.F8)
                WritePort("\x1b[19~");
            if (e.KeyCode == Keys.F9)
                WritePort("\x1b[20~");
            if (e.KeyCode == Keys.F10)
                WritePort("\x1b[21~");
            if (e.KeyCode == Keys.F11)
                WritePort("\x1b[23~");
            if (e.KeyCode == Keys.F12)
                WritePort("\x1b[24~");
            if (e.KeyCode == Keys.Up)
                WritePort("\x1b[A");
            if (e.KeyCode == Keys.Down)
                WritePort("\x1b[B");
            if (e.KeyCode == Keys.Left)
                WritePort("\x1b[D");
            if (e.KeyCode == Keys.Right)
                WritePort("\x1b[C");
        }

         private void OpenPort(string portname)
        {
  
                try
                {
                    if (portname.Substring(0, 3) == "COM")  
                    {
                        if (com.IsOpen)
                            com.Close();
                        com.PortName = portname;
                        com.Open();

                    }
                    else
                    {
                        tcp = new Tcp(portname);
                        tcp.tcpEvent += tcpRxEvent;
                    }
                         Login.Interval = 1000;
                        Login.Enabled = true;               }
                catch { }
            Properties.wtf.Default.port = portname;
            Properties.wtf.Default.Save();
        }


         private void Form1_Resize(object sender, EventArgs e)
         {
             text.Width = Width-10;
             text.Height = Height - 50;
             bar.Width = Width - Reset.Size.Width - bar.Left-20;

         }

         private void LFdelay_Enter(object sender, KeyPressEventArgs e)
         {
             if (e.KeyChar == '\r')
                 try
                 {
                     TxTimer.Enabled = false;
                     TxTimer.Interval = Convert.ToInt32(toolStripTextBox1.Text);
                     toolStripTextBox1.Owner.Hide();
                     lfdelayMenu.Owner.Hide();
                 }
                 catch
                 {
                     MessageBox.Show("napačna vrednost...");
                 }
         }
        
        
        public void WritePort(string s)
        {
            txString += s;
            if (TxTimer.Enabled == false)
                TxTimer_Tick(null, null);
        }
        
        
        private void TxTimer_Tick(object sender, EventArgs e)
        {
            int n = txString.IndexOf("\r");
            if (n >= 0)
            {
                if (com.IsOpen)
                    com.Write(txString.Substring(0, n + 1));
                if (tcp != null)
                    tcp.Write(txString.Substring(0, n + 1));
                txString = txString.Substring(n + 1);
                TxTimer.Enabled = true;
            }
            else
            {
                if (com.IsOpen)
                    com.Write(txString);
                if (tcp != null)
                    tcp.Write(txString);
                txString = "";
                if (txString != "")
                    TxTimer.Enabled = true;
                else
                    TxTimer.Enabled = false;
            }
        }

        private void Send2Host_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader fs = new StreamReader(fd.FileName);
                    WritePort(fs.ReadToEnd());
                    fs.Close();
                }
                catch
                {
                    MessageBox.Show("tega fila se ne da brat...");
                }
            }
        }

        private void Baudrate_Click(object sender, EventArgs e)
        {
            com.Close();
            com.BaudRate = Convert.ToInt32(sender.ToString());
            Properties.wtf.Default.baud = com.BaudRate;
            Properties.wtf.Default.Save();
            com.Open();
        }

        private void Connect_MouseDown(object sender, MouseEventArgs e)
        {
            string s;
            if (e.Button == MouseButtons.Right)
            {
                s = Microsoft.VisualBasic.Interaction.InputBox("Enter IP", "TCP connection, port 23", Properties.wtf.Default.port, e.X + this.Left, e.Y + this.Top);
                Properties.wtf.Default.ip.Add(s);
                Properties.wtf.Default.Save();
            }
        }


        private void comRxEvent(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                rxString += com.ReadExisting();
                this.Invoke(new EventHandler(ParsingRxData));  
            }
            catch {}
        }

        private void tcpRxEvent(object sender, EventArgs e)
        {
            try
            {
                rxString += tcp.ReadExisting();
                this.Invoke(new EventHandler(ParsingRxData));
            }
            catch { }
        }


        private void iapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "hex files (*.hex)|*.hex|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader fs = new StreamReader(fd.FileName);
                    iapFile = File.ReadAllLines(fd.FileName);
                    bar.Value = bar.Minimum = 0;
                    bar.Maximum = 2*iapFile.Count();
                    bar.Enabled = true;
                    fs.Close();
                    while (true)
                    {
                        WritePort(iapFile[bar.Value/2] + "\r");
                        if (iapFile[bar.Value/2].Substring(7, 2) == "00")
                            break;
                        bar.Value += 2;
                    }
                }
                catch
                {
                    MessageBox.Show("tega fila se ne da brat...");
                }
            }
        }

        private void ParsingRxData(object sender, EventArgs e)
        {
            log.Write(rxString);
            if (bar.Enabled == false)
            {
                pString += rxString;
                for (int i = pString.IndexOf("\r\n"); pString != "" && i >= 0; i = pString.IndexOf("\r\n")) 
                {
                    if (valve != null)
                    {
                        try
                        {
                            valve.Show();
                            valve.parse(pString.Substring(0,i));
                        }
                        catch
                        {
                            valve.Dispose();
                            valve = null;
                        }
                    }

                    if (pyro != null)
                    {
                        try
                        {
                            pyro.Show();
                            pyro.parse(pString.Substring(0, i));
                        }
                        catch
                        {
                            pyro.Dispose();
                            pyro = null;
                        }
                    }
                    
                    if (pulse != null)
                    {
                        try
                        {
                            pulse.Show();
                            pulse.parse(pString.Substring(0, i));
                        }
                        catch
                        {
                            pulse.Dispose();
                            pulse = null;
                        }
                    }
                    
                    if (pString.Substring(0, 1) == "v")
                    {
                        Login.Enabled=false;
                        this.Text = Properties.wtf.Default.port +" connected";
                 //       pyroToolStripMenuItem_Click(null, null);
                   //     this.WindowState = FormWindowState.Minimized;
                    }
                    
                    pString = pString.Substring(i+2);
                    if (pString != "" && pString.Substring(0, 1) == ">")
                        pString = pString.Substring(1);

                }
                text.AppendText(rxString);
                while (true)
                {
                    int n = text.Text.IndexOf('\b');
                    if (n < 0)
                        break;
                    text.Text = text.Text.Substring(0, n - 1) + text.Text.Substring(n + 1);
                }
                text.SelectionStart = text.Text.Length;
                rxString = "";                
            }
//----------------------------------------------------------------------------------------------
            else
            {
                for(int n=rxString.IndexOf("-",0); n != -1; n=rxString.IndexOf("-",++n))
                    if(++bar.Value % 2 == 0)
                        while(true)
                        {
                            WritePort(iapFile[bar.Value/2] + "\r");
                            if (iapFile[bar.Value/2].Substring(7, 2) == "00")
                                break;
                            bar.Value += 2;
                            if (bar.Value / 2 == iapFile.Count())
                            {
                                bar.ForeColor = Color.Lime;
                                bar.Enabled = false;
                                text.Text += "\r\n";
                                WritePort("\x1b");
                                break;
                            }
                        }
                    bar.Refresh();
            }
//----------------------------------------------------------------------------------------------
            rxString = "";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
            System.Environment.Exit(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WritePort("\xd");
        }


        private void valveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            valve = new Form3(this);
            valve.Show();
            valve.Left = Width / 2;
            valve.Top = Height / 2;
        }

        private void Login_Tick(object sender, EventArgs e)
        {
                WritePort("\rv\r");
        }

       
        private void ReceiveMessage(object sender, EventArgs e)
        {
            string s = udp.Read();
            IPAddress a;
            if (s.Substring(0, 5) == "LwIP " && IPAddress.TryParse(s.Substring(5), out a) == true)
            {
                rxString += "ping from " + s.Substring(5) + "\r\n>";
                Properties.wtf.Default.ip.Add(s.Substring(5));
                this.Invoke(new EventHandler(ParsingRxData));
            }

        }

        private void netToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void whatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface Interface in Interfaces)
            {
                rxString += "\r\n";
                rxString += Interface.Description;
                if (Interface.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                if (Interface.OperationalStatus != OperationalStatus.Up) continue;
                UnicastIPAddressInformationCollection UnicastIPInfoCol = Interface.GetIPProperties().UnicastAddresses;
                foreach (UnicastIPAddressInformation UnicatIPInfo in UnicastIPInfoCol)
                {
                    rxString += "\r\nIP Address is " + UnicatIPInfo.Address.ToString();
                    rxString += "\r\nSubnet Mask is " + UnicatIPInfo.IPv4Mask.ToString();
                }
            }
            rxString += "\r\n>";
            this.Invoke(new EventHandler(ParsingRxData));
        }

        private void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (udp == null)
            {
                udp = new Udp(this, 54321);
                udp.rxEvent += ReceiveMessage;
            }
            udp.Write("Lwip ???", IPAddress.Broadcast, 54321);
        }

        private void pyroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pyro = new pShape(this);
            pyro.Show();
            pyro.Left = Width / 2;
            pyro.Top = Height / 2;
        }

        private void pulseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pulse = new Form2(this);
            pulse.Show();
            pulse.Left = Width / 2;
            pulse.Top = Height / 2;
        }


   }
}
