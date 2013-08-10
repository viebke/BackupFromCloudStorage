using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace BackupFromCloud.Controllers
{
    class DropboxController
    {
        DropboxModel model;

        public DropboxModel Get()
        {
            this.model = new DropboxModel();
            return model;
        }

        public void DownloadFiles(string destFolderPath, bool updateMeta, BackgroundWorker worker)
        {
            try
            {
                this.model.Load(destFolderPath, updateMeta, worker);
            }
            catch
            {
                return;
            }
            
            this.model.SaveToDisk(destFolderPath, worker);
        }
    }
}
