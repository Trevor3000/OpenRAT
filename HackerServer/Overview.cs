using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HackerServer
{
    public partial class Overview : Form
    {
        public Overview()
        {
            InitializeComponent();
        }

        private void Overview_Load(object sender, EventArgs e)
        {
            ServerListener listener = new ServerListener(dataGridView1);
            Thread t = new Thread(listener.listen);
            t.Start();
        }

        private void start_Click(object sender, EventArgs e)
        {
            string name = (string)dataGridView1.SelectedRows[0].Cells[0].Value;

            foreach (Connection c in ServerListener.allConnections)
            {
                if (c.getName().Equals(name))
                {
                    try
                    {
                        Form1 form = new Form1(c);
                        form.Show();
                        return;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der Benutzer hat sich wahrscheinlich ausgeloggt", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }

        }
    }
}
