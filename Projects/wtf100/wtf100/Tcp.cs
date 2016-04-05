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

namespace WindowsFormsApplication1
{
    class Udp
    {
        UdpClient udp;
        Thread thread;
        string udpData;
        Form parent;
        int lport;
        public event EventHandler rxEvent = null;

        public Udp(Form f, int port)
        {
            lport = port;
            parent = f;
            udp = new UdpClient(port);
            thread = new Thread(new ThreadStart(UdpReceiving));
            thread.Start();
            udp.EnableBroadcast = true;
        }

        ~Udp()
        {
            udp.Close();
        }

        public void Write(string s, IPAddress a, int port)
        {
            byte[] bytes = new byte[s.Length];
            System.Buffer.BlockCopy(s.ToCharArray(), 0, bytes, 0, bytes.Length);
            udp.Send(bytes, bytes.Length, new IPEndPoint(a, port));
        }

        public string Read()
        {
            string s = udpData;
            udpData = "";
            return s;
        }
            
        private void UdpReceiving()
        {
            while (true)
            {
                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, lport);
                byte[] content = udp.Receive(ref remoteIPEndPoint);
                if (content.Length > 0)
                {
                    udpData += Encoding.ASCII.GetString(content);
                    if (parent != null && rxEvent != null)
                        parent.Invoke(new EventHandler(rxEvent));
                    Thread.Sleep(10);
                }
            }
        }
    }

    class Tcp
    {
        public TcpClient client = new TcpClient();
        public NetworkStream stream;
        public event EventHandler tcpEvent = null;
        
        public Thread rxpoll = null;
        public bool isOpen = false;
        public string tcprx = "";
        public Tcp(string portname)
        {
            string  ip=portname;              
            int     port=23;              
            int i = portname.IndexOf(':');                
            if (i > 0)               
            {                    
                ip = portname.Substring(0, i);                    
                port = Convert.ToInt32(portname.Substring(i + 1));               
            }
                
            IAsyncResult ar = client.BeginConnect(ip, port, null, null);                
            System.Threading.WaitHandle wh = ar.AsyncWaitHandle;
                
            try                
            {                    
                if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3), false))                    
                {                       
                    client.Close();                        
                    isOpen = false;                    
                }                   
                else                   
                {                        
                    stream = client.GetStream();                       
                    rxpoll = new Thread(new ThreadStart(DataCheck));                      
                    rxpoll.Start();                        
                    isOpen = true;                    
                }               
            }               
            catch { }            
        }

        ~Tcp()           
        {                
            Close();            
        }

        public void Write(string s)
        {
            byte[] wb = Encoding.ASCII.GetBytes(s);
            try
            {
                if (stream.CanWrite)
                    stream.Write(wb, 0, wb.Count());
            }
            catch { }
        }
        
        public void Close()
        {
            if (rxpoll != null)
                rxpoll.Abort();
            client.Close();
            isOpen = false;
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
        
        public string ReadExisting()
        {
            tcprx = "";
            while(stream.DataAvailable)
                tcprx += Convert.ToString((char)stream.ReadByte());
            return tcprx;
        }
    }   
}
