using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading;
using System.Windows.Forms;

namespace HackerClient
{
    public partial class Chat : Form
    {
        volatile TcpClient clientChat;
        volatile StreamWriter sw;
        private Thread chatThread;
        public Chat()
        {
            InitializeComponent();
            start();
        }

        private void start()
        {
            clientChat = new TcpClient();
            clientChat.Connect("nuggetor.ddns.net", 2019);

            sw = new StreamWriter(clientChat.GetStream(), ASCIIEncoding.ASCII);
            StreamReader sr = new StreamReader(clientChat.GetStream(), ASCIIEncoding.ASCII);

            ChatClass cclass = new ChatClass(outputBox, clientChat);
            chatThread = new Thread(cclass.run);
            chatThread.Start();

        }

        private void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(inputBox.Text + "\r\n");
                clientChat.GetStream().Write(buffer, 0, buffer.Length);
                outputBox.Text += "Du: " + inputBox.Text + "\r\n";
                inputBox.Text = "";
            }
        }

        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            byte[] buffer = new byte[1024];
            buffer = ASCIIEncoding.ASCII.GetBytes("Error: Client hat das Fenster geschlossen.");
            clientChat.GetStream().Write(buffer, 0, buffer.Length);
            chatThread.Abort();
            clientChat.GetStream().Close();
            clientChat.Close();
        
        }
    }
}
