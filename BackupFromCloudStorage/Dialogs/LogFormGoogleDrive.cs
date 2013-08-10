using BackupFromCloud.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace BackupFromCloud.Dialogs
{
    public partial class LogFormGoogleDrive : DockContent
    {
        public LogFormGoogleDrive()
        {
            InitializeComponent();

            LogController.LogGoogle +=LogController_Log;
        }

        private void LogController_Log(string msg)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    LogController_Log(msg);
                });
                return;
            }

            this.listBox1.Items.Add(msg);
        }
    }
}
