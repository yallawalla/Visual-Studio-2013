using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;



namespace vdrive
{
    class Program
    {

        static class Subst {
            public static void MapDrive(char letter, string path) {
                if (!DefineDosDevice(0, devName(letter), path))
                    throw new Win32Exception();
            }
            public static void UnmapDrive(char letter) {
                if (!DefineDosDevice(2, devName(letter), null))
                    throw new Win32Exception();
            }
            public static string GetDriveMapping(char letter) {
                var sb = new StringBuilder(259);
                if (QueryDosDevice(devName(letter), sb, sb.Capacity) == 0) {
                    // Return empty string if the drive is not mapped
                    int err = Marshal.GetLastWin32Error();
                    if (err == 2) return "";
                    throw new Win32Exception();
                }
                return sb.ToString().Substring(4);
            }


            private static string devName(char letter) {
                return new string(char.ToUpper(letter), 1) + ":";
            }
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern bool DefineDosDevice(int flags, string devname, string path);
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern int QueryDosDevice(string devname, StringBuilder buffer, int bufSize);
        }

        static void Main(string[] args)
        {
            TcpClient tcpclnt = new TcpClient();
            tcpclnt.Client.ReceiveTimeout = 20;

			    tcpclnt.Connect("192.168.1.100", 6000);

			    var stm = tcpclnt.GetStream();
			    Console.WriteLine("tcp connected ...");


            Subst.MapDrive('z', @"c:\temp");
            Console.WriteLine("virtual drive " + Subst.GetDriveMapping('z'));
            int c=0;
            do
            {
                while (stm.DataAvailable)
                {
                    c = stm.ReadByte();
                    if (c != -1)
                        Console.Write(Convert.ToChar(c));
                }
                if (Console.KeyAvailable)
                {
                    c = Console.ReadKey(true).KeyChar;
                    if (c != 0x1b)
                        stm.WriteByte(Convert.ToByte(c));
                }
            } while (c != 0x1b);

            Subst.UnmapDrive('z');
            if(tcpclnt.Connected)
                tcpclnt.Close();
        }
    }
}



