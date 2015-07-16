using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HackerServer
{
    class ChatClass
    {
        private TextBox outputBox;
        private NetworkStream reader;
        private TcpClient client;

        public ChatClass(TextBox outputBox, TcpClient client)
        {
            this.outputBox = outputBox;
            this.reader = client.GetStream();
            this.client = client;
        }

        public void run()
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                reader.Read(buffer, 0, 1024);
                string response = ASCIIEncoding.ASCII.GetString(buffer);
                response = "Other guy: " + response + "\r\n";
                this.outputBox.BeginInvoke((MethodInvoker)delegate() { this.outputBox.Text += response ;}); 

                if(response.StartsWith("Other guy: Error: Client hat das Fenster geschlossen."))
                {
                    reader.Close();
                    client.Close();
                    break;
                }
            }
        }
    }
}
