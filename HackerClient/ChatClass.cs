using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HackerClient
{
    class ChatClass
    {
        private TextBox outputBox;
        private NetworkStream stream;
        private TcpClient client;

        public ChatClass(TextBox outputBox, TcpClient client)
        {
            this.outputBox = outputBox;
            this.stream = client.GetStream();
            this.client = client;
        }

        public void run()
        {

            while (true)
            {
                byte[] buffer = new byte[1024];
                try
                {
                   
                    stream.Read(buffer, 0, 1024);
                    string response = ASCIIEncoding.ASCII.GetString(buffer);
                    response = "Anonymous: " + response + "\r\n";
                    this.outputBox.BeginInvoke((MethodInvoker)delegate() { this.outputBox.Text += response; }); 
                }
                catch (Exception)
                {
                    stream.Close();
                    client.Close();
                    return;
                }
                
            }
        }
    }
}
