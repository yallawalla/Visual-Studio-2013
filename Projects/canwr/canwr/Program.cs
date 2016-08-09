using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

// gcc -c -Wall -Werror -fpic ./canwr.c -DUNIX
// gcc -shared -o libcanwr.so canwr.o

namespace canwr
{

    class Program
    {
        [DllImport("./cancan/bin/Debug/libcancan.so", EntryPoint = "can_init")]
        private static extern int can_init(int[] filter, int[] mask, int n);
        [DllImport("./cancan/bin/Debug/libcancan.so", EntryPoint = "can_write")]
        private static extern int can_write(int id, byte[] data, int n);
        [DllImport("./cancan/bin/Debug/libcancan.so", EntryPoint = "can_read")]
        private static extern int can_read(byte[] data, int n);

        static void Main(string[] args)
        {

            string s;
            byte[]  buf = new byte[1600];
            int[]   filters = new int[] {0x21,0x41,0xb0,0xb1};
            int[]   masks = new int[] {0xff,0xff,0xff,0xff};
            byte[] data = new byte[16];
            byte[] bb = new byte[16];
            if(can_init(filters, masks, 4) != 0)
                Console.WriteLine("init. error...");
            do
            {
                if (Console.KeyAvailable == true)
                {
                    data[0] = (byte)Console.ReadKey().Key;
                    can_write(0x21, data, 1);
                }
                Thread.Sleep(300);
                Console.WriteLine(".");
                int n = can_read(buf, buf.Length);
                if (n > 0)  {
                    for (int i = 0; i < n; i += 16)
                    {
                        Array.Copy(buf, i, bb, 0, 16);
                        Console.WriteLine(BitConverter.ToString(bb));
                   }
                }
            } while (true);

        }
    }
}

