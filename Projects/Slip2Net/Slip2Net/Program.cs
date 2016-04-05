using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using PcapDotNet.Base;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Arp;
using PcapDotNet.Packets.Dns;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.Gre;
using PcapDotNet.Packets.Http;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.Igmp;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace Slip2Net
{
    public class SysTrayApp : Form
    {
        [STAThread]
        public static void Main()
        {
            Application.Run(new SysTrayApp());
        }

        private const int _SEND_BUFF_LEN=20000;

        private Serial com;
        private PacketCommunicator eth;
        private PacketSendBuffer sendBuffer;
        private Packet Tx;
        private NotifyIcon trayIcon;
        private bool loop = false;
        private int nPackets = 0;
        public SysTrayApp()
        {
            trayIcon = new NotifyIcon();
            trayIcon.ContextMenu = new ContextMenu();
            SetupRightClickmenu(trayIcon.ContextMenu);
            trayIcon.Text = "Com2Net";
            trayIcon.Icon = Properties.Resources.LiveUpdate;
            trayIcon.Visible = true;
        }
        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);  
            comSubItem_Click(null, null); 
            nicSubItem_Click(null, null);
        }
        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }
        private void SetupRightClickmenu(ContextMenu trayMenu)
        {
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            if (allDevices.Count == 0)
            {
                MessageBox.Show("... nimaš interfejsa, gotof si");
            }

            string[] ports = SerialPort.GetPortNames();
            if (ports.Length == 0)
            {
                MessageBox.Show("... nimaš portof, gotof si");
            }

            trayMenu.MenuItems.Add(new MenuItem("Com"));
            trayMenu.MenuItems.Add(new MenuItem("Net"));
            trayMenu.MenuItems.Add(new MenuItem("Exit", exitItem_Click));

            foreach (string port in ports)
                trayMenu.MenuItems[0].MenuItems.Add(port, comSubItem_Click);
            for (int i = 0; i != allDevices.Count; ++i)
                trayMenu.MenuItems[1].MenuItems.Add(new MenuItem(allDevices[i].Description, nicSubItem_Click));
        }
        void exitItem_Click(object sender, EventArgs e)
        {
            loop = false;
            Application.Exit();
        }       
        void comSubItem_Click(object sender, EventArgs e)
        {
            if (sender == null)
                com=new Serial(Properties.S2N.Default.com);
            else
            {
                com = new Serial(((MenuItem)sender).Text);
                Properties.S2N.Default.com = ((MenuItem)sender).Text;
                Properties.S2N.Default.Save();
            }
            if (com.IsOpen == false)
                MessageBox.Show("port ne valja .....");
            else
                com.SendComData = nicSendData;
        }
        void nicSubItem_Click(object sender, EventArgs e)
        {
            PacketDevice selectedDevice; 
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            if (sender == null)
                selectedDevice = allDevices[Properties.S2N.Default.nic];
            else
            {
                selectedDevice = allDevices[((MenuItem)sender).Index];
                Properties.S2N.Default.nic = ((MenuItem)sender).Index;
                Properties.S2N.Default.Save();
            }
            eth = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 100);
            sendBuffer = new PacketSendBuffer(_SEND_BUFF_LEN);
            Thread t = new Thread(new ThreadStart(NicThread));
            t.IsBackground = true;
            t.Start();
        }
        private void PacketHandler(Packet packet)
        {
            if (com != null && com.IsOpen == true)
            {
                //com.SlipTx(packet.Buffer, packet.Length);
                //    string hex = BitConverter.ToString(packet.Buffer);
                //    hex = hex.Replace("-", ":");
                //    if (hex.Replace(":", "").Substring(40, 4) == "0001"&& hex.Replace(":", "").Substring(0, 12) == "040102010805")
                //        Console.WriteLine(hex.Substring(0, 17) + " r " + hex.Substring(18, 17) + " " + hex.Replace(":","").Substring(24, 4) + " " + hex.Replace(":","").Substring(28));
            }     
        }
        private void NicThread()
        { 
            loop = true;  
            while (loop==true)
            {
                do
                {
                    eth.ReceiveSomePackets(out nPackets, 200, PacketHandler);
                    eth.Transmit(sendBuffer,true);
                    sendBuffer.Dispose();
                    sendBuffer = new PacketSendBuffer(_SEND_BUFF_LEN);
                    Application.DoEvents();
                    Thread.Sleep(1);    
                } while (loop == true);
                eth.Break();

            }
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();this.ClientSize = new System.Drawing.Size(390, 262);
            this.Name = "SysTrayApp";
            this.Load += new System.EventHandler(this.SysTrayApp_Load);
            this.ResumeLayout(false);

        }
        private void SysTrayApp_Load(object sender, EventArgs e)
        {

        }
        private void nicSendData(byte[] b, int len)
        {
            byte[] bb = new byte[len];
            Array.Copy(b, 12,bb, 0,len - 12);
            string hex = BitConverter.ToString(b);
            hex = hex.Replace("-", ":");
            EthernetLayer ethernetLayer =
                new EthernetLayer
                {
                    Destination = new MacAddress(hex.Substring(0, 17)),
                    Source = new MacAddress(hex.Substring(18, 17)),
                    EtherType = EthernetType.Arp,
                };

            PayloadLayer payloadLayer =
                new PayloadLayer
                {
                    Data = new Datagram(bb),
                };


            PacketBuilder builder = new PacketBuilder(ethernetLayer, payloadLayer);
            Tx = builder.Build(DateTime.Now);
            Array.Copy(b, Tx.Buffer, len);           
            sendBuffer.Enqueue(Tx);
            //hex = BitConverter.ToString(b);
            //hex = hex.Replace("-", ":"); Console.WriteLine(hex.Substring(0, 17) + " w " + hex.Substring(18, 17) + " " + hex.Replace(":", "").Substring(24, 4));// + " " + hex.Replace(":", "").Substring(28,Tx.Count));
        }
    }
}
