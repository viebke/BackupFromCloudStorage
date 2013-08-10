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
    public partial class AuthenticationCode : Form
    {
        public AuthenticationCode()
        {
            InitializeComponent();
        }

        public string Code { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Code = this.textBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
