using BackupFromCloud.Controllers;
using BackupFromCloud.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BackupFromCloud.Dialogs
{
    public partial class GoogleDriveForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public GoogleDriveForm()
        {
            InitializeComponent();
        }

        private void buttonBackup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBoxPath.Text))
            {
                MessageBox.Show("Path is empty, please provide a path");
                return;
            }

            this.buttonBackup.Enabled = false;
            this.buttonOpenFolder.Enabled = false;
            this.buttonCancel.Enabled = true;
            this.textBoxPath.Enabled = false;

            this.backgroundWorker1.RunWorkerAsync();
        }

        void model_UpdateProgressGetFilesEvent(int count)
        {
            //cross thread - so you don't get the cross theading exception
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    model_UpdateProgressGetFilesEvent(count);
                });
                return;
            }

            this.labelCountFileInfo.Text = count.ToString() + " (including folders)";
        }

        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBoxPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.buttonBackup.Enabled = true;
            this.buttonCancel.Enabled = false;
            this.buttonOpenFolder.Enabled = true;
            this.textBoxPath.Enabled = true;
            this.labelNameValue.Text = string.Empty;

            this.backgroundWorker1.CancelAsync();

            this.labelCountDownload.Text = string.Empty;
            this.labelCountFileInfo.Text = string.Empty;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.buttonBackup.Enabled = true;
            this.buttonCancel.Enabled = false;
            this.textBoxPath.Enabled = true;
            this.buttonOpenFolder.Enabled = true;
            this.labelNameValue.Text = string.Empty;

            MessageBox.Show("Backup completed");

            this.labelCountDownload.Text = string.Empty;
            this.labelCountFileInfo.Text = string.Empty;
        }

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.backgroundWorker1.WorkerSupportsCancellation = true;
                GoogleDriveController controller = new GoogleDriveController();
                GoogleDriveModel model = controller.Get();

                model.UpdateProgressGetFilesEvent += model_UpdateProgressGetFilesEvent;
                model.UpdateProgressDownloadFilesEvent += model_UpdateProgressDownloadFilesEvent;
                model.UpdateNameEvent += model_UpdateNameEvent;

                try
                {
                    controller.DownloadFiles(this.textBoxPath.Text, this.backgroundWorker1);
                }
                catch
                {
                    MessageBox.Show("Unable to download files, see log for more information");
                }
            }
            catch
            {
                MessageBox.Show("Unable to initiate model, see log for more information");
            }
        }

        void model_UpdateNameEvent(string name){
            
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    model_UpdateNameEvent(name);
                });
                return;
            }

            this.labelNameValue.Text = name;
        }

        void model_UpdateProgressDownloadFilesEvent(int count, int tot)
        {
            //cross thread - so you don't get the cross theading exception
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    model_UpdateProgressDownloadFilesEvent(count, tot);
                });
                return;
            }

            this.progressBarDownload.Value = (int)((double)count / (double)tot * 100);
            this.labelCountDownload.Text = count + "/" + tot;
        }

        private void GoogleDriveForm_Load(object sender, EventArgs e)
        {

        }
    }
}
