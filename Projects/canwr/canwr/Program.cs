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
        private static extern int can_write(int id, char[] data, int n);
        [DllImport("./cancan/bin/Debug/libcancan.so", EntryPoint = "can_read")]
        private static extern int can_read(char[] data, int n);

        static void Main(string[] args)
        {
            int n;
            string s;
            char[]  buf = new char[16];
            int[]   filters = new int[] {0x21,0x41,0xb0,0xb1};
            int[]   masks = new int[] {0xff,0xff,0xff,0xff};
            char[]  data = new char[16];
              can_init(filters,masks,4);
            do
            {
                if (Console.KeyAvailable == true)
                {
                    data[0] = (char)Console.ReadKey().Key;
                    can_write(0x21, data, 1);
                }
                n = can_read(buf, 16);
                if (n > 0)  {
                    Console.Write(buf);
                    Thread.Sleep(10);
                }
            } while (true);

        }
    }
}

