using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class Constants
    {
        public const double ADC2U = (3.3 / 4096.0);
        public const double PFMU = ADC2U * (7.5 + 2000) / 7.5;          // ADC >> U
        public const double PFMI = ADC2U * 1000 / 2.99999;              // ADC >> I

        public const double IGBTU = ADC2U * (5.6 + 2000) / 7.5;
        public const double Ts = (0.000001*4);
        public const double EScale = 65536.0;
        public const int DumpBufferLength = 1024;
    }
}
