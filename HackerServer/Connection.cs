using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HackerServer
{
    public class Connection
    {
        TcpListener listenerSer;
        TcpClient accComm;
        string name;

        public Connection(TcpListener listenerSer, TcpClient accComm, string name)
        {
            this.listenerSer = listenerSer;
            this.accComm = accComm;
            this.name = name;
        }

        public TcpListener getSerListener()
        {
            return listenerSer;
        }

        public TcpClient getAccComm()
        {
            return accComm;
        }

        public string getName()
        {
            return name;
        }
    }
}
