using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HackerServer
{
    public partial class Chat : Form
    {
        volatile TcpListener listenerChat;
        volatile StreamWriter sw;
        TcpClient clientChat;
        Thread chatThread;
        public Chat()
        {
            InitializeComponent();
            start();
        }

        private void start()
        {
            listenerChat = new TcpListener(2019);
            listenerChat.Start();

            clientChat = listenerChat.AcceptTcpClient();

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
            try
            {
                listenerChat.Stop();
                chatThread.Abort();
                clientChat.GetStream().Close();
                clientChat.Close();
            }
            catch (Exception)
            {
            }

        }


    }
}
