namespace SmartHomeWin
{
    partial class Home
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.deviceListView = new System.Windows.Forms.ListView();
            this.devName = new System.Windows.Forms.ColumnHeader();
            this.devId = new System.Windows.Forms.ColumnHeader();
            this.devType = new System.Windows.Forms.ColumnHeader();
            this.devStatus = new System.Windows.Forms.ColumnHeader();
            this.devOnline = new System.Windows.Forms.ColumnHeader();
            this.devHotkey = new System.Windows.Forms.ColumnHeader();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnOnOff = new System.Windows.Forms.Button();
            this.btnBindHotKey = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAutoStart = new System.Windows.Forms.ToolStripMenuItem();
            this.seperator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.btnUnbindKeys = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // deviceListView
            // 
            this.deviceListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.devName,
            this.devId,
            this.devType,
            this.devStatus,
            this.devOnline,
            this.devHotkey});
            this.deviceListView.FullRowSelect = true;
            this.deviceListView.HideSelection = false;
            this.deviceListView.Location = new System.Drawing.Point(24, 51);
            this.deviceListView.MultiSelect = false;
            this.deviceListView.Name = "deviceListView";
            this.deviceListView.Size = new System.Drawing.Size(788, 339);
            this.deviceListView.TabIndex = 3;
            this.deviceListView.UseCompatibleStateImageBehavior = false;
            this.deviceListView.View = System.Windows.Forms.View.Details;
            this.deviceListView.SelectedIndexChanged += new System.EventHandler(this.DeviceListView_SelectedIndexChanged);
            // 
            // devName
            // 
            this.devName.Text = "Name";
            this.devName.Width = 230;
            // 
            // devId
            // 
            this.devId.Text = "Device ID";
            this.devId.Width = 150;
            // 
            // devType
            // 
            this.devType.Text = "Type";
            this.devType.Width = 50;
            // 
            // devStatus
            // 
            this.devStatus.Text = "Status";
            this.devStatus.Width = 50;
            // 
            // devOnline
            // 
            this.devOnline.Text = "Online";
            this.devOnline.Width = 50;
            // 
            // devHotkey
            // 
            this.devHotkey.Text = "Hotkey";
            this.devHotkey.Width = 230;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(830, 51);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 33);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // btnOnOff
            // 
            this.btnOnOff.Enabled = false;
            this.btnOnOff.Location = new System.Drawing.Point(830, 99);
            this.btnOnOff.Name = "btnOnOff";
            this.btnOnOff.Size = new System.Drawing.Size(100, 33);
            this.btnOnOff.TabIndex = 4;
            this.btnOnOff.Text = "ON/OFF";
            this.btnOnOff.UseVisualStyleBackColor = true;
            this.btnOnOff.Click += new System.EventHandler(this.BtnOnOff_Click);
            // 
            // btnBindHotKey
            // 
            this.btnBindHotKey.Enabled = false;
            this.btnBindHotKey.Location = new System.Drawing.Point(830, 147);
            this.btnBindHotKey.Name = "btnBindHotKey";
            this.btnBindHotKey.Size = new System.Drawing.Size(100, 33);
            this.btnBindHotKey.TabIndex = 6;
            this.btnBindHotKey.Text = "Bind Hotkey";
            this.btnBindHotKey.UseVisualStyleBackColor = true;
            this.btnBindHotKey.Click += new System.EventHandler(this.BtnBindHotKey_Click);
            this.btnBindHotKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BtnBindHotKey_KeyDown);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Hello";
            this.notifyIcon.Click += new System.EventHandler(this.NotifyIcon_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmFile,
            this.tsmAbout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(952, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmFile
            // 
            this.tsmFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLogin,
            this.menuAutoStart,
            this.seperator1,
            this.menuExit});
            this.tsmFile.Name = "tsmFile";
            this.tsmFile.Size = new System.Drawing.Size(37, 20);
            this.tsmFile.Text = "File";
            // 
            // menuLogin
            // 
            this.menuLogin.Name = "menuLogin";
            this.menuLogin.Size = new System.Drawing.Size(182, 22);
            this.menuLogin.Text = "Login";
            this.menuLogin.Click += new System.EventHandler(this.MenuLogin_Click);
            // 
            // menuAutoStart
            // 
            this.menuAutoStart.Checked = true;
            this.menuAutoStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuAutoStart.Name = "menuAutoStart";
            this.menuAutoStart.Size = new System.Drawing.Size(182, 22);
            this.menuAutoStart.Text = "Open when pc starts";
            // 
            // seperator1
            // 
            this.seperator1.Name = "seperator1";
            this.seperator1.Size = new System.Drawing.Size(179, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(182, 22);
            this.menuExit.Text = "Exit";
            // 
            // tsmAbout
            // 
            this.tsmAbout.Name = "tsmAbout";
            this.tsmAbout.Size = new System.Drawing.Size(52, 20);
            this.tsmAbout.Text = "About";
            // 
            // btnUnbindKeys
            // 
            this.btnUnbindKeys.Enabled = false;
            this.btnUnbindKeys.Location = new System.Drawing.Point(830, 196);
            this.btnUnbindKeys.Name = "btnUnbindKeys";
            this.btnUnbindKeys.Size = new System.Drawing.Size(100, 33);
            this.btnUnbindKeys.TabIndex = 6;
            this.btnUnbindKeys.Text = "Unbind All";
            this.btnUnbindKeys.UseVisualStyleBackColor = true;
            this.btnUnbindKeys.Click += new System.EventHandler(this.BtnUnbindKeys_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 413);
            this.Controls.Add(this.btnUnbindKeys);
            this.Controls.Add(this.btnBindHotKey);
            this.Controls.Add(this.btnOnOff);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.deviceListView);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Smart Home Win";
            this.Load += new System.EventHandler(this.Home_Load);
            this.Resize += new System.EventHandler(this.Home_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView deviceListView;
        private System.Windows.Forms.ColumnHeader devName;
        private System.Windows.Forms.ColumnHeader devId;
        private System.Windows.Forms.ColumnHeader devType;
        private System.Windows.Forms.ColumnHeader devStatus;
        private System.Windows.Forms.ColumnHeader devOnline;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnOnOff;
        private System.Windows.Forms.Button btnBindHotKey;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ColumnHeader devHotkey;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmFile;
        private System.Windows.Forms.ToolStripMenuItem menuLogin;
        private System.Windows.Forms.ToolStripSeparator seperator1;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem tsmAbout;
        private System.Windows.Forms.ToolStripMenuItem menuAutoStart;
        private System.Windows.Forms.Button btnUnbindKeys;
    }
}

