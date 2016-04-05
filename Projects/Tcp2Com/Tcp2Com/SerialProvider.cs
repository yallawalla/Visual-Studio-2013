using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using TcpLib;

namespace Tcp2Com
{
	/// <SUMMARY>
	/// SerialProvider
	/// </SUMMARY>
    public class SerialProvider : TcpServiceProvider
	{
        private SerialPort com = null;
        private ConnectionState conn = null;

        private void comRxEvent(object sender, SerialDataReceivedEventArgs e)
        {
            readRxData(this,e);
        }

        private void readRxData(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[1024];      
            if(conn != null && com.BytesToRead > 0)
                conn.Write(buffer,0,com.Read(buffer,0,1024));
        }

        public override object Clone()
        {
            return new SerialProvider();
        }



		public override void OnAcceptConnection(ConnectionState state)
		{
            try
            {
                string[] ports = SerialPort.GetPortNames();
                int n = 0;
                foreach (string port in ports)
                {
                    string s = (n++).ToString() + ") " + port;
                    try
                    {
                        com = new SerialPort(port);
                        com.Open();
                        com.Close();
                    }
                    catch
                    {
                        s += " - open";
                    }
                    finally
                    {
                        s += "\r\n";
                        state.Write(Encoding.ASCII.GetBytes(s), 0, s.Length); 
                    }
                }
                com=null;
            }
            catch (Exception e)
            {
                state.EndConnection(); 
            }
        }


		public override void OnReceiveData(ConnectionState state)
		{
			byte[] buffer = new byte[1024];
 			while(state.AvailableData > 0)
			{
				int readBytes = state.Read(buffer, 0, 1024);
                if (com == null)
                {
                    string[] ports = SerialPort.GetPortNames();
                    buffer[0] -= (byte)'0';
                    if (buffer[0] >= 0 && buffer[0] < ports.Length)
                        try
                        {
                            com = new SerialPort(ports[buffer[0]], 921600, Parity.None, 8, StopBits.One);
                            com.Handshake = Handshake.None;
                            com.DataReceived += new SerialDataReceivedEventHandler(comRxEvent);
                            conn = state;
                            com.Open();
                            string s = "...connected\r\n";
                            state.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
                        }
                        catch (Exception e)
                        {
                            state.Write(Encoding.ASCII.GetBytes(e.Message+"\r\n"), 0, e.Message.Length+2);
                            com = null;
                        }
                }
                else
                {
                    if (readBytes > 0)
                    {
                        com.Write(buffer, 0, readBytes);
                    }
                }
			}
		}


		public override void OnDropConnection(ConnectionState state)
		{
            if (com.IsOpen == true)
                com.Close();
            conn = null;
		}
	}
}
