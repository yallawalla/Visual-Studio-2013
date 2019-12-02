using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FTD2XX_NET;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] dbuf= new byte[8] { 0xff, 0x00, 0xff, 0x00, 0xff, 0x00, 0xff, 0x00 };
            uint n=0;
            UInt32 ftdiDeviceCount = 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK; 
            FTDI myFtdiDevice = new FTDI();
            ftStatus = myFtdiDevice.GetNumberOfDevices(ref ftdiDeviceCount);
            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];
            ftStatus = myFtdiDevice.GetDeviceList(ftdiDeviceList);
            for (UInt32 i = 0; i < ftdiDeviceCount; i++) {
                //Console.WriteLine("Device Index: " + i.ToString());
                //Console.WriteLine("Flags: " + String.Format("{0:x}", ftdiDeviceList[i].Flags));
                //Console.WriteLine("Type: " + ftdiDeviceList[i].Type.ToString());
                //Console.WriteLine("ID: " + String.Format("{0:x}", ftdiDeviceList[i].ID));
                //Console.WriteLine("Location ID: " + String.Format("{0:x}", ftdiDeviceList[i].LocId));
                //Console.WriteLine("Serial Number: " + ftdiDeviceList[i].SerialNumber.ToString());
                //Console.WriteLine("Description: " + ftdiDeviceList[i].Description.ToString());
                // Open device in our list by serial number
                ftStatus = myFtdiDevice.OpenBySerialNumber(ftdiDeviceList[i].SerialNumber);
                if (String.Equals(ftdiDeviceList[i].SerialNumber, "F5090101") ||
                    String.Equals(ftdiDeviceList[i].SerialNumber, "SkyPulse") ||
                    String.Equals(ftdiDeviceList[i].SerialNumber, "FTF2J188"))
                {
                        ftStatus = myFtdiDevice.SetBitMode(0xc0, 0x01);             // reset
                        Thread.Sleep(200);
                        ftStatus = myFtdiDevice.SetBitMode(0xc4, 0x01);             // reset
                        Thread.Sleep(200);
                        ftStatus = myFtdiDevice.SetBitMode(0x00, 0x00);             // threestate ???
                    return;
                }
                else if(String.Equals(ftdiDeviceList[i].SerialNumber, "LM-IO")) {
                        ftStatus = myFtdiDevice.SetBitMode(0xc8, 0x01);             // reset
                        Thread.Sleep(200);
                        ftStatus = myFtdiDevice.SetBitMode(0xcc, 0x01);             // reset
                        Thread.Sleep(200);

                        ftStatus = myFtdiDevice.SetBitMode(0x00, 0x00);             // threestate ???
                        return;
                }
                else
                    ftStatus = myFtdiDevice.Close();
            }
            Console.WriteLine("... ni jankotove škatle, ponovi vajo !!!");
            Thread.Sleep(3000);
            return;
        }
    }
}
