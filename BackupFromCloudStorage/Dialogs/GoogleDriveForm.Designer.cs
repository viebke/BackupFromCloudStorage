namespace BackupFromCloud.Dialogs
{
    partial class GoogleDriveForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonOpenFolder = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.progressBarDownload = new System.Windows.Forms.ProgressBar();
            this.labelDownloadProgress = new System.Windows.Forms.Label();
            this.labelMetdataFiles = new System.Windows.Forms.Label();
            this.buttonBackup = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.labelCountFileInfo = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.labelCountDownload = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelNameValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOpenFolder
            // 
            this.buttonOpenFolder.Location = new System.Drawing.Point(180, 12);
            this.buttonOpenFolder.Name = "buttonOpenFolder";
            this.buttonOpenFolder.Size = new System.Drawing.Size(29, 23);
            this.buttonOpenFolder.TabIndex = 17;
            this.buttonOpenFolder.Text = "...";
            this.buttonOpenFolder.UseVisualStyleBackColor = true;
            this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Location = new System.Drawing.Point(297, 11);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(79, 23);
            this.buttonCancel.TabIndex = 16;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(11, 15);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(163, 20);
            this.textBoxPath.TabIndex = 15;
            // 
            // progressBarDownload
            // 
            this.progressBarDownload.Location = new System.Drawing.Point(11, 98);
            this.progressBarDownload.Name = "progressBarDownload";
            this.progressBarDownload.Size = new System.Drawing.Size(588, 23);
            this.progressBarDownload.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarDownload.TabIndex = 14;
            // 
            // labelDownloadProgress
            // 
            this.labelDownloadProgress.AutoSize = true;
            this.labelDownloadProgress.Location = new System.Drawing.Point(8, 73);
            this.labelDownloadProgress.Name = "labelDownloadProgress";
            this.labelDownloadProgress.Size = new System.Drawing.Size(98, 13);
            this.labelDownloadProgress.TabIndex = 13;
            this.labelDownloadProgress.Text = "Download progress";
            // 
            // labelMetdataFiles
            // 
            this.labelMetdataFiles.AutoSize = true;
            this.labelMetdataFiles.Location = new System.Drawing.Point(8, 49);
            this.labelMetdataFiles.Name = "labelMetdataFiles";
            this.labelMetdataFiles.Size = new System.Drawing.Size(118, 13);
            this.labelMetdataFiles.TabIndex = 11;
            this.labelMetdataFiles.Text = "Metadata files fetched: ";
            // 
            // buttonBackup
            // 
            this.buttonBackup.Location = new System.Drawing.Point(215, 12);
            this.buttonBackup.Name = "buttonBackup";
            this.buttonBackup.Size = new System.Drawing.Size(75, 23);
            this.buttonBackup.TabIndex = 10;
            this.buttonBackup.Text = "Backup";
            this.buttonBackup.UseVisualStyleBackColor = true;
            this.buttonBackup.Click += new System.EventHandler(this.buttonBackup_Click);
            // 
            // labelCountFileInfo
            // 
            this.labelCountFileInfo.AutoSize = true;
            this.labelCountFileInfo.Location = new System.Drawing.Point(136, 49);
            this.labelCountFileInfo.Name = "labelCountFileInfo";
            this.labelCountFileInfo.Size = new System.Drawing.Size(13, 13);
            this.labelCountFileInfo.TabIndex = 19;
            this.labelCountFileInfo.Text = "0";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork_1);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // labelCountDownload
            // 
            this.labelCountDownload.AutoSize = true;
            this.labelCountDownload.Location = new System.Drawing.Point(136, 73);
            this.labelCountDownload.Name = "labelCountDownload";
            this.labelCountDownload.Size = new System.Drawing.Size(0, 13);
            this.labelCountDownload.TabIndex = 20;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(294, 49);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 21;
            this.labelName.Text = "Name:";
            // 
            // labelNameValue
            // 
            this.labelNameValue.AutoSize = true;
            this.labelNameValue.Location = new System.Drawing.Point(338, 49);
            this.labelNameValue.Name = "labelNameValue";
            this.labelNameValue.Size = new System.Drawing.Size(0, 13);
            this.labelNameValue.TabIndex = 22;
            // 
            // GoogleDriveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 143);
            this.Controls.Add(this.labelNameValue);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.labelCountFileInfo);
            this.Controls.Add(this.buttonOpenFolder);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.progressBarDownload);
            this.Controls.Add(this.labelDownloadProgress);
            this.Controls.Add(this.labelCountDownload);
            this.Controls.Add(this.labelMetdataFiles);
            this.Controls.Add(this.buttonBackup);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GoogleDriveForm";
            this.Text = "Google Drive";
            this.Load += new System.EventHandler(this.GoogleDriveForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenFolder;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.ProgressBar progressBarDownload;
        private System.Windows.Forms.Label labelDownloadProgress;
        private System.Windows.Forms.Label labelMetdataFiles;
        private System.Windows.Forms.Button buttonBackup;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label labelCountFileInfo;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label labelCountDownload;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelNameValue;
    }
}