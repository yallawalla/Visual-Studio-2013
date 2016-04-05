using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        StreamWriter fout = new StreamWriter("menu.c");
        StreamReader fin = new StreamReader("text.txt");
        public Form1()
        {
            InitializeComponent();
            menu(0, "","");
            fout.Close();
            fin.Close();
        }

        string menu(int lev, string s, string type)
        {
            int c;
            string szMenu = "", szSub = "", szFunc = "", szArg = "", szPath = s;
            while (true)
            {
                try
                {
                    c = fin.Read();
                    switch (c)
                    {
                        case -1:
                            return (null);
                        case '\r':
                            break;
                        case '\t':
                            break;
                        case '\n':
                            break;
                        case '(':
                            add_function(szPath);
                            add_arg(menu_opt);
                            break;
                        case ')':
                            if (szPath != "")
                                return (szPath);
                            else
                                return ("NULL");
                        case '{':
                            add_function(menu);
                            add_arg(menu_opt);
                            menu(++lev, szPath, "{");
                            break;
                        case '}':
 


                            T.Text += szMenu    + "\r\n";
                            T.Text += szFunc    + "\r\n";
                            T.Text += szArg     + "\r\n";

                            return (szPath);
                        case ';':
                            break;
                        default:
                            if(type != "") {
                                if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_')
                                    szPath += (char)c;
                                else
                                    szPath += "c_" + c.ToString("X2");
                            }
                            break;
                    }
                }
                catch { }

            }
        }
    }
}
