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
    class FileReciever
    {
        String filename;
        String downloadpath;
        ProgressBar bar;
        public FileReciever(String filename, String downloadpath, ProgressBar bar)
        {
            this.filename = filename;
            this.downloadpath = downloadpath;
            this.bar = bar;
        }

        public void listen()
        {
            try
            {
                TcpListener listener = new TcpListener(2013);
                listener.Start();
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                FileStream output = File.Create(downloadpath + "\\" + filename);
                byte[] buffer = new byte[1024];
                int bytesRead;

                stream.Read(buffer, 0, buffer.Length);
                long size = Convert.ToInt64(ASCIIEncoding.ASCII.GetString(buffer));
                if (this.bar.InvokeRequired)
                {
                    this.bar.BeginInvoke((MethodInvoker)delegate() { this.bar.Maximum = (int)size; ;});
                }
                else
                {
                    bar.Maximum = (int)size;
                }
                int recBytes = 0;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, bytesRead);
                    recBytes += bytesRead;
                    if (this.bar.InvokeRequired)
                    {
                        this.bar.BeginInvoke((MethodInvoker)delegate() { this.bar.Value = (int)recBytes; ;});
                    }
                    else
                    {
                        bar.Value = recBytes;
                    }
                }

                if (this.bar.InvokeRequired)
                {
                    this.bar.BeginInvoke((MethodInvoker)delegate() { this.bar.Value = 0; });
                }
                else
                {
                    bar.Value = 0;
                }
                output.Close();
                listener.Stop();
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
    }
}
