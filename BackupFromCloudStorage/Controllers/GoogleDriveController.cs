using BackupFromCloud.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BackupFromCloud.Controllers
{
    public class GoogleDriveController
    {
        GoogleDriveModel model;

        public GoogleDriveModel Get()
        {
            this.model = new GoogleDriveModel();
            return this.model;
        }

        public void DownloadFiles(string destFolderPath, BackgroundWorker worker)
        {
            this.model.Load();

            if (worker.CancellationPending)
            {
                return;
            }

            this.model.SaveToDisk(destFolderPath, worker);
        }
    }
}
