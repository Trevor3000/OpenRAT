using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HackerServer
{
    public partial class Screenshot : Form
    {
        public Screenshot(Bitmap b)
        {
            InitializeComponent();
            pictureBox1.Image = b;
            pictureBox1.Height = b.Height;
            pictureBox1.Width = b.Width;
            this.Height = b.Height;
            this.Width = b.Width;
        }
    }
}
