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
    public partial class LogFormDropbox : DockContent
    {
        public LogFormDropbox()
        {
            InitializeComponent();

            LogController.LogDropbox += LogController_LogDropbox;
        }

        void LogController_LogDropbox(string msg)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    LogController_LogDropbox(msg);
                });
                return;
            }

            this.listBox1.Items.Add(msg);
        }
    }
}
