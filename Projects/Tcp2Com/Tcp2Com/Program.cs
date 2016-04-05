using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcpLib;

namespace Tcp2Com
{
    class Program
    {
        private TcpServer Server;
        private System.IO.Ports.SerialPort com;
        private SerialProvider Provider;

        static void Main(string[] args)
        {
            SerialProvider Provider = new SerialProvider();
            TcpServer Server = new TcpServer(Provider, 7);
            Server.Start();
        }
    }
}
