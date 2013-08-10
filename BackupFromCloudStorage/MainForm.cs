using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using BackupFromCloud.Dialogs;

namespace BackupFromCloud
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            LogFormGoogleDrive logFormG = new LogFormGoogleDrive();
            logFormG.ShowHint = DockState.DockBottom;
            logFormG.Show(this.dockPanel1);

            LogFormDropbox logFormD = new LogFormDropbox();
            logFormD.ShowHint = DockState.DockBottom;
            logFormD.Show(this.dockPanel1);

            this.dockPanel1.Dock = DockStyle.Fill;
            this.dockPanel1.BackColor = Color.Black;
            this.dockPanel1.BringToFront();

            DropboxForm dForm = new DropboxForm();
            dForm.ShowHint = DockState.Document;
            dForm.Show(this.dockPanel1);

            GoogleDriveForm gForm = new GoogleDriveForm();
            gForm.ShowHint = DockState.Document;
            gForm.Show(this.dockPanel1);

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            new AboutForm().Show();
        }
    }
}
