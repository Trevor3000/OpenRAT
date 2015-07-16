using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace HackerServer
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
            Socket s = new Socket(SocketType.Stream, ProtocolType.Tcp);
            s.Connect(IPAddress.Parse("127.0.0.1"), 2013);
            s.SendFile(path);
            s.Close();
        }
    }
}
