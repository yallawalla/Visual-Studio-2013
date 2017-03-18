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
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int binread = 0;
        int adcrate = 0;
        string RxString = "";
        string IapStr = "";
        Form2 Scope = null;
        Form3 Boot = null;
        Form4 Fan = new Form4();
        ArrayList cols = new ArrayList();
        Tcp tcp = null;
        int rcv = -1;

        public Form1()
        {
            InitializeComponent();
            ReadLabels();
            OpenPort(Properties.pfm6.Default.port);
            shaped.Checked = Properties.pfm6.Default.shaped;
            channel1.Checked = Properties.pfm6.Default.channel1;
            channel2.Checked = Properties.pfm6.Default.channel2;

            aGauge1.MaxValue = 1000;
            aGauge1.ScaleLinesMajorStepValue = aGauge1.MaxValue / 5;
            aGauge1.Range_Idx = 0;
            aGauge1.RangeStartValue = 0;
            aGauge1.RangeEndValue = aGauge1.MaxValue * 8 / 20;
            aGauge1.Range_Idx = 1;
            aGauge1.RangeStartValue = aGauge1.MaxValue * 8 / 20;
            aGauge1.RangeEndValue = aGauge1.MaxValue * 8 / 10;
            aGauge1.Range_Idx = 2;
            aGauge1.RangeStartValue = aGauge1.MaxValue * 8 / 10;
            aGauge1.RangeEndValue = aGauge1.MaxValue;
        }
        //.................. menu items 

        public void ScopeMenu_Click(object sender, EventArgs e)
        {
            try
            {
                Scope.Show();
            }
            catch (NullReferenceException exc)                          // prvic
            {
                exc.ToString();
                Scope = new Form2(this);
                Scope.Show();
            }
            catch (ObjectDisposedException exc)                         // ker cepec je zaprl Form2
            {
                exc.ToString();
                Scope = new Form2(this);
                Scope.Show();
            }
            finally
            {
                cols.Clear();
//                PortWrite("-? " + Scope.sActive().ToString() + "\r");
                PortWrite("$ " + Scope.sActive().ToString() + "\r");
            }
        }

        private void ResetMenu_Click(object sender, EventArgs e)
        {
            com.RtsEnable = true;
            Thread.Sleep(300);
            com.RtsEnable = false;
        }

        //.................. serial port 

        private void OpenPort(string portname)
        {
            if (portname.Substring(0, 3) == "COM")
                try
                {
 //                   IgbtTemp.Enabled = false;
                    if (com.IsOpen)
                        com.Close();
                    com.PortName = portname;
                    com.Open();
                    Login.Interval = 1000;
                    Login.Enabled = true;
                    ButtonsFrame.Enabled = false;
                    ScopeMenu.Enabled = false;
                }
                catch { }
            else
            {
                tcp = new Tcp(portname);
                tcp.tcpEvent += tcpRxEvent;
            }
            Properties.pfm6.Default.port = portname;
            Properties.pfm6.Default.Save();
        }

        private void IapProc(object sender, EventArgs e)
        {
            int n;
            do
            {
                n = IapStr.IndexOf(":-");
                if (n != -1)
                {
                    if (BootBar.Visible == true)
                        IapStart();
                    IapStr = IapStr.Substring(n + 2);
                }

                n = IapStr.IndexOf("--");
                if (n != -1)
                {
                    if (BootBar.Visible == true)
                        IapStart();
                    IapStr = IapStr.Substring(n + 2);
                }
            } while (n != -1);
        }

        private void tcpRxEvent(object sender, EventArgs e)
        {
            try
            {
                do
                {
                    rcv = tcp.ReadByte();
                    if (rcv != -1)
                        this.Invoke(new EventHandler(ParsingRxData));
                } while (rcv != -1);
            }
            catch { }
        }

        private void serialRxEvent(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                while (com.BytesToRead > 0)
                {
                    rcv = com.ReadByte();
                    this.Invoke(new EventHandler(ParsingRxData));
                }
            }
            catch { }
        }

 
 
/*
                            >u
>U(bank)  Uc,Uc/2       ... 701V,350V,19.7V,-4.6V
>s
>s(immer) n,T1,T2       ... 150ns,150ns
>d
>d(elay)  T,PW          ... 100us,0.02
>p
>p(ulse)  T,PW          ... 100us,0.02
>b
>b(urst)  N,len,per     ... 1,1ms,1000ms
>f
>f(an)    Tl,Th,min,max ... 30,40,20,95
                            */
                        //case "2":
                        //case ":":
                        //case "D":
                        //case "#":

                        //case "_":
                        //case "!":
                        //case "e":
                        //case "$":
                        //case "x":

        private void ParsingRxData(object sender, EventArgs e)
        {
            byte b = Convert.ToByte(rcv);
            if (binread > 0 && Scope != null && Scope.Visible == true)
            {
                Scope.ScopeBinary(b, adcrate);
                Pbar.Value = Pbar.Maximum - binread;
                if (--binread == 0)
                    Scope.ScopeBinary(-1, adcrate);
            }
            else
            {
                RxString += (char)b;
                if (RxString.Contains("\r\n"))
                {
                    RxString = RxString.TrimStart('>', ' ');
                    switch (RxString.Substring(0, 1))
                    {
                        case "*":
                            this.Text = RxString;
                            break;

                        case ".":
                            break;

                        case "f":
                            aGauge2.Value = Fan.ShowTemp(RxString.Substring(2).Split(','));
                            break;

                        case "$":
                            {
                                string[] s = RxString.Substring(2).Split(',');
                                Pbar.Maximum  = binread = Convert.ToInt32(s[1]);
                                adcrate = 60*4;
                            }
 
                            break;
                        case "S":
                            int imax = Convert.ToInt32(RxString.Substring(2, 2), 16);
                            for (int i = 6; i < imax; i += 4)
                            {
                                try
                                {
                                    cols.Add(new String[4] {
                                        (Constants.Ts*cols.Count).ToString(),
                                        (Constants.IGBTU*(Convert.ToInt16(RxString.Substring(2*i+2,2) + RxString.Substring(2*i,2),16))).ToString(),
                                        (Constants.Ts*cols.Count).ToString(),
                                        (Constants.PFMI*(Convert.ToInt16(RxString.Substring(2*i+6,2) + RxString.Substring(2*i+4,2),16))).ToString()
                                    });
                                }
                                catch { }
                            }
                            if (cols.Count == 20000 / 4)
                            {
                                try
                                {
                                    Scope.DrawGraph(cols);
                                    cols.Clear();
                                }
                                catch { }
                            }
                            break;

                        case "v":

                            if (com.IsOpen)
                                this.Text = "PFM6 connected, port " + com.PortName + ", version " + RxString.Substring(0);
                            else
                                if (tcp.client.Connected)
                                    this.Text = "PFM6 connected, port " + tcp.stream.GetType().ToString() + ", version " + RxString.Substring(0);
                            Login.Enabled = false;
                            ButtonsFrame.Enabled = true;
                            ScopeMenu.Enabled = true;
                            charger.Checked=Properties.pfm6.Default.charger;
                            charger_CheckedChanged(null,null);
                            HV.Value=Properties.pfm6.Default.HVvalue;
                            SetHVCommand(null, null);


                            SimmerRate.Value = Properties.pfm6.Default.SimmerRate;
                            SimmerPw.Value= Properties.pfm6.Default.SimmerPw;
                            SetPFMSimmercommand(null, null);

                            DelayTime.Value = Properties.pfm6.Default.DelayTime;
                            DelayPw.Value = (int)(Properties.pfm6.Default.DelayPw);
                            SetDelay(null, null);
                            break;

                        case "-":
                            if (RxString.Substring(1, 1) == "?")
                            {
                                cols.Clear();
                                break;
                            }
                            if (RxString.Substring(1, 1) == "!")
                            {
                                string[] s = RxString.Substring(2).Split(',');
                                Pbar.Maximum  =binread =Convert.ToInt32(s[0]);
                                adcrate = Convert.ToInt32(s[1]);
                            }
                            break;

                        case "(":
                            try
                            {
                                --Boot.Ack;
                            }
                            catch
                            {
                            }
                            break;

                        case "4":
                            {
                                int i;
                                if (RxString.Substring(0, 2) == "40")
                                {
                                    try
                                    {
                                        switch (Convert.ToInt32(RxString.Substring(7, 2), 16))
                                        {
                                            case 0x00:  // pfm status
                                                SetErrorStatus(Convert.ToInt32(RxString.Substring(13, 2) + RxString.Substring(10, 2), 16),
                                                                Convert.ToInt32(RxString.Substring(19, 2) + RxString.Substring(16, 2), 16),
                                                                Convert.ToInt32(RxString.Substring(25, 2) + RxString.Substring(22, 2), 16));
                                                break;

                                            case 0x03:  // 
                                                if (Fan.Visible == true)
                                                    PortWrite(Fan.ShowTemp(Convert.ToInt32(RxString.Substring(10, 2), 16)) + "\r");
                                                break;

                                            case 0x07: //
                                                i = (Convert.ToInt32(RxString.Substring(19, 2) + RxString.Substring(16, 2) +
                                                                        RxString.Substring(13, 2) + RxString.Substring(10, 2), 16));
                                                // textBox2.Text = string.Format("{0:####0.0}", i * Constants.IGBTU * Constants.PFMI * Constants.Ts * Constants.EScale);
                                                textBox2.Text = string.Format("{0:####0.0}", (double)i / 1000.0);
                                                if(Scope.Visible==true)
                                                    PortWrite("$ " + Scope.sActive().ToString() + "\r");
                                                break;

                                            case 0x12:
                                                aGauge1.Range_Idx = 0;
                                                aGauge1.RangeStartValue = 0;
                                                aGauge1.RangeEndValue = (float)Constants.PFMU * (Convert.ToInt32(RxString.Substring(19, 2) + RxString.Substring(16, 2), 16)) / 8;

                                                aGauge1.Range_Idx = 1;
                                                aGauge1.RangeStartValue = (float)Constants.PFMU * (Convert.ToInt32(RxString.Substring(19, 2) + RxString.Substring(16, 2), 16)) / 8;
                                                aGauge1.RangeEndValue = (float)Constants.PFMU * (Convert.ToInt32(RxString.Substring(13, 2) + RxString.Substring(10, 2), 16)) / 4;

                                                aGauge1.Range_Idx = 2;
                                                aGauge1.RangeStartValue = (float)Constants.PFMU * (Convert.ToInt32(RxString.Substring(13, 2) + RxString.Substring(10, 2), 16)) / 4;
                                                aGauge1.RangeEndValue = aGauge1.MaxValue;

                                                aGauge2.Value = (Convert.ToInt32(RxString.Substring(22, 2), 16));
                                                if (Fan.Visible == true)
                                                    PortWrite(Fan.ShowTemp(Convert.ToInt32(RxString.Substring(22, 2), 16)) + "\r");
                                                break;
                                        }
                                    }
                                    catch { }
                                }
                            }
                            break;

                        default:
                            // MessageBox.Show(RxString);
                            break;
                    }
                    RxString = "";
                }
            }
        }

        //.................. sliders, checkboxes, buttons
        private void SlidersScroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackU, trackU.Value.ToString("0V"));
            toolTip1.SetToolTip(trackT, trackT.Value.ToString("0us"));
            toolTip1.SetToolTip(Period, (Period.Value/10.0).ToString("0.0Hz"));
            toolTip1.SetToolTip(Burst, Burst.Value.ToString());
            toolTip1.SetToolTip(Length, Length.Value.ToString("0ms"));
            toolTip1.SetToolTip(HV,HV.Value.ToString("0V"));
            toolTip1.SetToolTip(DAC1, "DAC1=" + ((DAC1.Value * 100 + 2048) / 4096).ToString()+"%");
            toolTip1.SetToolTip(DAC2, "DAC2=" + ((DAC2.Value * 100 + 2048) / 4096).ToString()+"%");
            toolTip1.SetToolTip(SimmerPw, SimmerPw.Value.ToString("0ns"));
            toolTip1.SetToolTip(SimmerRate, SimmerRate.Value.ToString("0uS"));
            toolTip1.SetToolTip(DelayPw, (DelayPw.Value / 100.0).ToString("0.00%"));
            toolTip1.SetToolTip(DelayTime, DelayTime.Value.ToString("0us"));

            aGauge1.Value = trackU.Value;
        }

        private void SetPFMcommand(object sender, MouseEventArgs e)
        {
            if(shaped.Checked == true)
                PortWrite(".03"
                    + ((10 * trackU.Value) % 256).ToString("X2") + ((10 * trackU.Value) / 256).ToString("X2")
                    + (trackT.Value % 256).ToString("X2") + (trackT.Value / 256).ToString("X2")
                    + "03\r");
            else
                PortWrite(".03"
                    + ((10 * trackU.Value) % 256).ToString("X2") + ((10 * trackU.Value) / 256).ToString("X2")
                    + (trackT.Value % 256).ToString("X2") + (trackT.Value / 256).ToString("X2")
                    + "01\r");

            textBox1.Text = (Burst.Value * Math.Pow(trackU.Value, 3) / 400 * trackT.Value * 1e-6).ToString("0.0J") + "/" +
                            (Burst.Value * Math.Pow(trackU.Value, 3) / 400 * trackT.Value * 1e-10 * Period.Value) .ToString("0.0kW");
 
        }  
        
        private void ResetPFMcommand(object sender, MouseEventArgs e)

        {
            int k = 10000 / Period.Value;
            PortWrite(".04"
                + (k % 256).ToString("X2") + (k / 256).ToString("X2")
                + Burst.Value.ToString("X2") + Length.Value.ToString("X2")
                + (0 % 256).ToString("X2") + (0).ToString("X2")
                + "\r");

            textBox1.Text = (Burst.Value * Math.Pow(trackU.Value, 3) / 400 * trackT.Value * 1e-6).ToString("0.0J") + " / " +
                            (Burst.Value * Math.Pow(trackU.Value, 3) / 400 * trackT.Value * 1e-10 * Period.Value).ToString("0.0kW");
        }

        private void SetPFMSimmercommand(object sender, MouseEventArgs e)
        {
            PortWrite("s " + SimmerPw.Value.ToString() + ',' + SimmerPw.Value.ToString() + ',' + SimmerRate.Value.ToString() + "\r");
            Properties.pfm6.Default.SimmerRate = SimmerRate.Value;
            Properties.pfm6.Default.SimmerPw = SimmerPw.Value;
            Properties.pfm6.Default.Save();

            //PortWrite(".06"
            //    + (SimmerRate.Value % 256).ToString("X2") + (SimmerRate.Value / 256).ToString("X2")
            //    + (SimmerRate.Value % 256).ToString("X2") + (SimmerRate.Value / 256).ToString("X2")
            //    + "\r");
        }  
        
        private void SetDelay(object sender, MouseEventArgs e)
        {
            PortWrite("d " + DelayTime.Value.ToString("0") + "," + (DelayPw.Value/100.0).ToString("0.00") + "\r");
            Properties.pfm6.Default.DelayTime = DelayTime.Value;
            Properties.pfm6.Default.DelayPw = DelayPw.Value;
            Properties.pfm6.Default.Save();
        }



        private void SetHVCommand(object sender, MouseEventArgs e)
        {
            PortWrite("u " + HV.Value.ToString() + "\r");
            Properties.pfm6.Default.HVvalue = HV.Value;
            Properties.pfm6.Default.Save();
        }

        private void Simmer_CheckedChanged(object sender, EventArgs e)
        {
            // SetHVCommand(null, null);
            PfmCommand(0);
        }

        private void PfmCommand(int OrValue)
        {
            int i = 0;
            if (Simmer1.Checked)
            {
                i |= 1;
            }
            if (Simmer2.Checked)
            {
                i |= 2;
            }
            i |= OrValue;
            PortWrite(".02" + i.ToString("X2") + "\r");
        }


        //.................. status & error panel

        private void ReadLabels()
        {
            string[][] labels = new string[][] { 
            new string[] {"Simmer1 Ready"   ,"Simmer1  Error"   ,"not specified"}, 
            new string[] {"Simmer2 Ready"   ,"Simme 2  Error"   ,"CML"},
            new string[] { "."              ,"."                ,"TEMP"},
            new string[] {"."               ,"IGBT Idle Error"  ,"VIN_UV"},
            new string[] {"."               ,"IGBT Overheat"    ,"IOUT_OC"},
            new string[] {"."               ,"IGBT Drive Error" ,"VOUT_OV"},
            new string[] {"."               ,"."                ,"PSON_OFF"},
            new string[] {"Crowbar Ok"      ,"Crowbar Fired"    ,"BUSY"},
            new string[] {"HV Ready"        ,"PWM Error"        ,"."},
            new string[] {"."               ,"20V Error"        ,"."},
            new string[] {"."               ,"-5V Error"        ,"."},
            new string[] {"."               ,"."                ,"."},
            new string[] {"."               ,"ADC Watchdog"     ,"."},
            new string[] {"."               ,"Fan Error"        ,"."},
            new string[] {"."               ,"HV/2 Error"       ,"."},
            new string[] {"."               ,"I2C Error"        ,"."},
            new string[] {".",".","."},
            new string[] {".",".","."},
            new string[] {".",".","."},
            new string[] {".",".","."},
            new string[] {".",".","."},
            new string[] {".",".","."},
            new string[] {".",".","."},
            new string[] {".",".","."}};

            StatusList.Columns.Add("PFM status",StatusList.Width / 3 - 2, HorizontalAlignment.Center);
            StatusList.Columns.Add("PFM error", StatusList.Width / 3 - 2, HorizontalAlignment.Center);
            StatusList.Columns.Add("charger status", StatusList.Width / 3 - 2, HorizontalAlignment.Center);
            foreach (string[] s in labels)
            {
                ListViewItem lvi = new ListViewItem(s);
                lvi.UseItemStyleForSubItems = false;
                StatusList.Items.Add(lvi);
            }
        }

        private void SetErrorStatus(int s, int e, int xe)
        {
            int i, j;

            if ((s & 1) != 0)
                Simmer1.Checked = true;
            else
                Simmer1.Checked = false;

            if ((s & 2) != 0)
                Simmer2.Checked = true;
            else
                Simmer2.Checked = false;


            for (i = 1, j = 0; j < 16; ++j, i <<= 1)
            {
                if ((s & i) != 0)
                {
                    StatusList.Items[j].SubItems[0].BackColor = Color.LightGreen;
                }
                else
                {
                    StatusList.Items[j].SubItems[0].BackColor = Color.White;
                }
                if ((e & i) != 0)
                {
                    StatusList.Items[j].SubItems[1].BackColor = Color.Red;
                }
                else
                {
                    StatusList.Items[j].SubItems[1].BackColor = Color.White;
                }
                if ((xe & i) != 0)
                {
                    StatusList.Items[j].SubItems[2].BackColor = Color.Red;
                }
                else
                {
                    StatusList.Items[j].SubItems[2].BackColor = Color.White;
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ArrayList p = new ArrayList();
            Stream f = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "exec. files (*.hex)|*.hex|All files (*.*)|*.*";
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
                            p.Add(fs.ReadLine());
                        }
                        try
                        {
                            Boot.Show();
                        }
                        catch (NullReferenceException exc)          // prvic
                        {
                            exc.ToString();
                            Boot = new Form3(com, 16, p.Count);
                            Boot.Show();
                        }
                        catch (ObjectDisposedException exc)         // en cepec je zaprl Fom3
                        {
                            exc.ToString();
                            Boot = new Form3(com, 16, p.Count);
                            Boot.Show();
                        }
                        finally
                        {
                            Boot.Init(com, p);
                            Boot.Close();
                            f.Close();
                            fs.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void Delay_Tick(object sender, EventArgs e)
        {
            Delay.Enabled = false;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            PortWrite(e.KeyChar.ToString());
        }

        private void reset_Click(object sender, EventArgs e)
        {
            com.RtsEnable = true;
            Thread.Sleep(300);
            com.RtsEnable = false;
        }

        private void localBoot_Click(object sender, EventArgs e)
        {
            PortWrite("\x3");
        }

        private void IapStart()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Properties.pfm6.Default.bootdir;
            openFileDialog1.Filter = "exec. files (*.hex)|*.hex|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Properties.pfm6.Default.bootdir = openFileDialog1.InitialDirectory;
                Properties.pfm6.Default.Save();
                try
                {
                    StreamReader f = new StreamReader(openFileDialog1.FileName);
                    string s = f.ReadToEnd();
                    f.Close();
                    int i = 0;
                    int j = -2;
                    do
                    {
                        j = s.IndexOf("\r\n", j + 2);
                        ++i;
                    } while (j != -1);
                    BootBar.Maximum = 2 * i;
                    BootBar.Value = 0;
                    BootBar.Visible = true;
                    PortWrite(s);
                }
                catch
                { }
            }
            BootBar.Visible = false;
            PortWrite("\x19");
        }

        private void TempMenu_Click(object sender, EventArgs e)
        {
            Fan.Show();
        }

        private void serialPort1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show(e.EventType.ToString());
        }

        private void serialPort1_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            MessageBox.Show(e.EventType.ToString());
        }

        private void IgbtTemp_Tick(object sender, EventArgs e)
        {
            if (Login.Enabled == false)
            {
                PortWrite("f\r");
                PortWrite("u\r");
//                PortWrite(".12\r");
            }
        }

        private void Login_Tick(object sender, EventArgs e)
        {
            PortWrite("\r\r\rv\r");
        }

        private void XlapChanged(object sender, EventArgs e)
        {
            if (Xlap1.Checked == true)
                PortWrite("x 1\r");
            if (Xlap2.Checked == true)
                PortWrite("x 2\r");
            if (Xlap4.Checked == true)
                PortWrite("x 4\r");
        }

        private void ExitMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void eCEnable_Click(object sender, EventArgs e)
        {
            if (eCEnable.Text == "EC enable")
            {
                eCEnable.Text = "EC disable";
                PortWrite("u 1\r");
            }
            else
            {
                eCEnable.Text = "EC enable";
                PortWrite("u 0\r");
            }
        }

        public void PortWrite(string s)
        {
            try
            {
                if(com.IsOpen)
                    com.Write(s);
            }
            catch
            { }

            try
            {
                tcp.Write(s);
            }
            catch
            { }
        }



        private void ConnectMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            OpenPort(e.ClickedItem.ToString());
        }

        private void ConnectMenu_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                Properties.pfm6.Default.port = Microsoft.VisualBasic.Interaction.InputBox("Enter IP", "TCP connection, port 23", Properties.pfm6.Default.ip, e.X + this.Left, e.Y + this.Top);
            Properties.pfm6.Default.Save();
        }

        private void ConnectMenu_DropDownOpening(object sender, EventArgs e)
        {
            ConnectMenu.DropDownItems.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
                ConnectMenu.DropDownItems.Add(port);
            ConnectMenu.DropDownItems.Add(Properties.pfm6.Default.ip);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                tcp.client.Close();
                tcp = null;
            }
            catch
            { }

            IgbtTemp.Enabled = false;
            Login.Enabled = false;
        }

        private void TriggerButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PortWrite(".71\r");
            }

            if (e.Button == MouseButtons.Right)
                if (TriggerButton.Text.IndexOf("(R)") < 0)
                {
                    TriggerButton.Text = "Trigger(R)";
                    PortWrite("+m 7\r");
                }
                else
                {
                    TriggerButton.Text = "Trigger";
                    PortWrite("-m 7\r");
                }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
            System.Environment.Exit(1);
        }

        private void DAC_MouseUp(object sender, MouseEventArgs e)
        {
            PortWrite(".10"
                + (DAC2.Value % 256).ToString("X2") + (DAC2.Value / 256).ToString("X2")
                + (DAC1.Value % 256).ToString("X2") + (DAC1.Value / 256).ToString("X2")
                + "\r");
        }

        private void vLPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackT.Value = 1000;
            Burst.Value = 1;
            Length.Value = 3;
            SetPFMcommand(sender, null);
            ResetPFMcommand(sender, null);
            SlidersScroll(null, null);
        }

        private void sSPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackT.Value = 105;
            Burst.Value = 1;
            Length.Value = 3;
            SetPFMcommand(sender, null);
            ResetPFMcommand(sender, null);
            SlidersScroll(null, null);
        }

        private void sPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackT.Value = 350;
            Burst.Value = 1;
            Length.Value = 3;
            SetPFMcommand(sender, null);
            ResetPFMcommand(sender, null);
            SlidersScroll(null, null);
        }

        private void mSPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackT.Value = 175;
            Burst.Value = 1;
            Length.Value = 3;
            SetPFMcommand(sender, null);
            ResetPFMcommand(sender, null);
            SlidersScroll(null, null);
        }

        private void lPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackT.Value = 600;
            Burst.Value = 1;
            Length.Value = 3;
            SetPFMcommand(sender, null);
            ResetPFMcommand(sender, null);
            SlidersScroll(null, null);
        }

        private void qSPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackT.Value = 50;
            Burst.Value = 5;
            Length.Value = 1;
            SetPFMcommand(sender, null);
            ResetPFMcommand(sender, null);
            SlidersScroll(null, null);
        }

        private void shaped_CheckedChanged(object sender, EventArgs e)
        {
            Properties.pfm6.Default.shaped = shaped.Checked;
        }

        private void charger_CheckedChanged(object sender, EventArgs e)
        {
            Properties.pfm6.Default.charger = charger.Checked;
            Properties.pfm6.Default.Save();
            if (charger.Checked)
                PortWrite("-i,0x58,50000\r");
            else
                PortWrite("-i\r");
        }

        private void channelChanged(object sender, EventArgs e)
        {
            Simmer1.Checked = false;
            Simmer2.Checked = false;
            PortWrite("-m 11\r");
            PortWrite("-m 12\r");
            if (channel1.Checked == true)
            {
                PortWrite("+m 12\r");
            }
            if (channel2.Checked == true)
                PortWrite("+m 11\r");
            Properties.pfm6.Default.channel1 = channel1.Checked;
            Properties.pfm6.Default.channel2 = channel2.Checked;
            Properties.pfm6.Default.Save();

        }

    }


    public class Tcp
    {
        public TcpClient client = new TcpClient();
        public NetworkStream stream;
        public event EventHandler tcpEvent=null;

        private Thread rxpolling; 
        public Tcp(string ip)
        {
            client.Client.ReceiveTimeout = 20;
            try
            {
                client.Connect(ip, 23);
                stream = client.GetStream();
                rxpolling  = new Thread(new ThreadStart(DataCheck));
                rxpolling.Start();
            }
            catch { }
        }

        ~Tcp()
        {
            client.Close();
            if(rxpolling != null)
                rxpolling.Abort();
        }

        public void Write(string s)
        {
            byte[] wb = Encoding.ASCII.GetBytes(s);
            if(stream.CanWrite)
                stream.Write(wb, 0, wb.Count());
        }
        
        public void DataCheck()
        {
            while (true)
            {
                if (tcpEvent != null && stream.DataAvailable)
                    tcpEvent(this, new EventArgs());
                Thread.Sleep(10);
            }
        }

        public int ReadByte()
        {
            return stream.ReadByte();
        }
    }   

}

