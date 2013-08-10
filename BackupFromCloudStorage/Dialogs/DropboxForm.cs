using BackupFromCloud.Controllers;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace BackupFromCloud.Dialogs
{
    public partial class DropboxForm : DockContent
    {
        DropboxModel model;
        DropboxController controller;

        public DropboxForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox1.Text))
            {
                MessageBox.Show("No path provided for backup files");
                return;
            }

            try
            {
                this.controller = new DropboxController();
                this.model = controller.Get();
            }
            catch
            {
                MessageBox.Show("Unable to initiate model, see log for more details");
            }

            model.FolderUpdatedEvent += model_FolderUpdatedEvent;
            model.UpdateProgressEvent += model_UpdateProgressEvent;

            this.buttonBackup.Enabled = false;
            this.buttonOpenFolder.Enabled = false;
            this.buttonCancel.Enabled = true;
            this.textBox1.Enabled = false;

            this.backgroundWorker1.RunWorkerAsync();
            this.labelCountDownload.Text = string.Empty;
        }

        private void model_UpdateProgressEvent(int count, int tot)
        {
            //cross thread - so you don't get the cross theading exception
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    model_UpdateProgressEvent(count, tot);
                });
                return;
            }

            this.progressBarDownload.Value = (int)(((double)count / (double)tot) * 100);
            this.labelCountDownload.Text = count + "/" + tot;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.backgroundWorker1.WorkerSupportsCancellation = true;
                controller.DownloadFiles(this.textBox1.Text, this.checkBoxUpdateMetadata.Checked, this.backgroundWorker1);
            }
            catch
            {
                MessageBox.Show("Unable to download files, see log for more information");
            }
        }

        void model_FolderUpdatedEvent(string folder)
        {
            //cross thread - so you don't get the cross theading exception
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    model_FolderUpdatedEvent(folder);
                });
                return;
            }

            this.listBox1.Items.Add(folder);
            this.listBox1.SelectedIndex++;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = this.folderBrowserDialog1.SelectedPath.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.CancelAsync();
            
            MessageBox.Show("Backup cancelled");

            this.buttonBackup.Enabled = true;
            this.buttonOpenFolder.Enabled = true;
            this.buttonCancel.Enabled = false;
            this.textBox1.Enabled = true;

            this.progressBarDownload.Value = 0;
            this.labelCountDownload.Text = string.Empty;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Backup completed");

            this.buttonBackup.Enabled = true;
            this.buttonOpenFolder.Enabled = true;
            this.buttonCancel.Enabled = false;
            this.textBox1.Enabled = true;
            this.labelCountDownload.Text = string.Empty;
        }
    }
}
