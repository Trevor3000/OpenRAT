namespace HackerServer
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.startLogging = new System.Windows.Forms.Button();
            this.stopLogging = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.processButton = new System.Windows.Forms.Button();
            this.killButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.makeScreenshot = new System.Windows.Forms.Button();
            this.chatButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 29);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(427, 365);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(474, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(322, 20);
            this.textBox1.TabIndex = 1;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(474, 55);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(322, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // startLogging
            // 
            this.startLogging.Location = new System.Drawing.Point(474, 101);
            this.startLogging.Name = "startLogging";
            this.startLogging.Size = new System.Drawing.Size(91, 23);
            this.startLogging.TabIndex = 3;
            this.startLogging.Text = "Start Keyloggin";
            this.startLogging.UseVisualStyleBackColor = true;
            this.startLogging.Click += new System.EventHandler(this.startLogging_Click);
            // 
            // stopLogging
            // 
            this.stopLogging.Location = new System.Drawing.Point(705, 101);
            this.stopLogging.Name = "stopLogging";
            this.stopLogging.Size = new System.Drawing.Size(91, 23);
            this.stopLogging.TabIndex = 4;
            this.stopLogging.Text = "Stop Keyloggin";
            this.stopLogging.UseVisualStyleBackColor = true;
            this.stopLogging.Click += new System.EventHandler(this.stopLoggin_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(474, 221);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 173);
            this.listBox1.TabIndex = 5;
            // 
            // processButton
            // 
            this.processButton.Location = new System.Drawing.Point(705, 371);
            this.processButton.Name = "processButton";
            this.processButton.Size = new System.Drawing.Size(91, 23);
            this.processButton.TabIndex = 6;
            this.processButton.Text = "get processes";
            this.processButton.UseVisualStyleBackColor = true;
            this.processButton.Click += new System.EventHandler(this.processButton_Click);
            // 
            // killButton
            // 
            this.killButton.Location = new System.Drawing.Point(705, 342);
            this.killButton.Name = "killButton";
            this.killButton.Size = new System.Drawing.Size(91, 23);
            this.killButton.TabIndex = 7;
            this.killButton.Text = "kill process";
            this.killButton.UseVisualStyleBackColor = true;
            this.killButton.Click += new System.EventHandler(this.killButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(802, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(471, 365);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // makeScreenshot
            // 
            this.makeScreenshot.Location = new System.Drawing.Point(705, 204);
            this.makeScreenshot.Name = "makeScreenshot";
            this.makeScreenshot.Size = new System.Drawing.Size(90, 23);
            this.makeScreenshot.TabIndex = 9;
            this.makeScreenshot.Text = "Screenshot";
            this.makeScreenshot.UseVisualStyleBackColor = true;
            this.makeScreenshot.Click += new System.EventHandler(this.makeScreenshot_Click);
            // 
            // chatButton
            // 
            this.chatButton.Location = new System.Drawing.Point(705, 286);
            this.chatButton.Name = "chatButton";
            this.chatButton.Size = new System.Drawing.Size(90, 23);
            this.chatButton.TabIndex = 10;
            this.chatButton.Text = "Chat";
            this.chatButton.UseVisualStyleBackColor = true;
            this.chatButton.Click += new System.EventHandler(this.chatButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1285, 406);
            this.Controls.Add(this.chatButton);
            this.Controls.Add(this.makeScreenshot);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.killButton);
            this.Controls.Add(this.processButton);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.stopLogging);
            this.Controls.Add(this.startLogging);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.treeView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private volatile System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button startLogging;
        private System.Windows.Forms.Button stopLogging;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button processButton;
        private System.Windows.Forms.Button killButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button makeScreenshot;
        private System.Windows.Forms.Button chatButton;
    }
}

