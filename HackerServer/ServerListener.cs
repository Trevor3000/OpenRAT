using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HackerServer
{
    class ServerListener
    {
        DataGridView dgv;
        public static volatile List<Connection> allConnections;
        public ServerListener(DataGridView dgv)
        {
            this.dgv = dgv;
            allConnections = new List<Connection>();
        }

        public void listen()
        {
            while (true)
            {
                TcpListener listenerComm = new TcpListener(2015);
                listenerComm.Start();
                TcpClient accComm = listenerComm.AcceptTcpClient();

                byte[] buffer = new byte[1024];
                accComm.GetStream().Read(buffer, 0, 1024);
                string name = ASCIIEncoding.ASCII.GetString(buffer);
                name = name.Substring(0, name.IndexOf("\0"));
                Connection user = new Connection(new TcpListener(2015), accComm, name);

                DataGridViewRow dr = (DataGridViewRow)dgv.Rows[0].Clone();
                dr.Cells[0].Value = name;
                if (this.dgv.InvokeRequired)
                {
                    this.dgv.BeginInvoke((MethodInvoker)delegate() { this.dgv.Rows.Add(dr); ;});
                }
                else
                {
                    this.dgv.Rows.Add(dr);
                }

                listenerComm.Stop();
                allConnections.Add(user);
            }

        }
    }
}
