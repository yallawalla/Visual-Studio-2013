using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slip2Net
{


    class Serial
    {
 

        public bool IsOpen { get { return this.com.IsOpen; } }      
        private SerialPort com=null;
        private byte[] buffer_slip = new byte[2000];
        private int n_slip = -1;
  
        public Serial(string s)
        {
            com = new SerialPort(s);
            try
            {
                com.Open();
                com.DataReceived += new SerialDataReceivedEventHandler(SlipRx);
            } catch { }
        }

        byte[] buf = new byte[5000];   
        public void  SlipTx(byte[] b, int len)
        {

            int j = 0;
            buf[j++] = SLIP_END;
            for (int i = 0; i < len; ++i)
            {
                switch(b[i])
                {
                    case SLIP_END:
                        buf[j++] = SLIP_ESC;
                        buf[j++] = SLIP_ESC_END;
                        break;
                    case SLIP_ESC:
                        buf[j++] = SLIP_ESC;
                        buf[j++] = SLIP_ESC_ESC;
                        break;
                    default:
                        buf[j++] = b[i];
                        break;
                }
            }
            buf[j++] = SLIP_END;
            if (com.IsOpen == true)
                com.Write(buf, 0, j);
        }




        public delegate void cbf(byte[] buffer, int len);
        public cbf SendComData = null; 

        private void SlipRx(object sender, SerialDataReceivedEventArgs e)
        {
            while (com.BytesToRead > 0)
            {   
                byte b = (byte)com.ReadByte();
                if(n_slip == -1)
                {
                    if(b==SLIP_END)
                        n_slip =0;
                    continue;
                }
                switch(b)
                {
                    case SLIP_END:
                        if (SendComData != null)
                            SendComData(buffer_slip, n_slip);
                        n_slip = -1;
                        break;

                    case SLIP_ESC:
                        buffer_slip[n_slip] = SLIP_ESC;
                        break;

                    case SLIP_ESC_END:
                        if(buffer_slip[n_slip] == SLIP_ESC)
                            buffer_slip[n_slip++] = SLIP_END;
                        else
                            buffer_slip[n_slip++] = SLIP_ESC_END;
                        break;

                    case SLIP_ESC_ESC:  
                        if(buffer_slip[n_slip] == SLIP_ESC)
                            buffer_slip[n_slip++] = SLIP_ESC;
                        else
                            buffer_slip[n_slip++] = SLIP_ESC_ESC;
                        break;

                    default:
                        if(n_slip < buffer_slip.Length)
                            buffer_slip[n_slip++] = b;
                        break;
                }   
            }
        }

        private const byte SLIP_END = 0xC0;
        private const byte SLIP_ESC = 0xDB;
        private const byte SLIP_ESC_END = 0xDC;
        private const byte SLIP_ESC_ESC = 0xDD;
    }
}

