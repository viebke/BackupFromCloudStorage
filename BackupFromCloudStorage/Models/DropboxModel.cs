using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DropNet;
using System.Diagnostics;
using System.IO;
using DropNet.Models;
using System.Xml.Serialization;
using System.Threading;
using BackupFromCloud.Controllers;
using BackupFromCloud.Helpers;
using System.ComponentModel;

namespace Models
{

    class DropboxModel
    {
        public delegate void UpdateCurrentFolderProcess(string folder);
        public delegate void UpdateProgress(int count, int total);
        public event UpdateCurrentFolderProcess FolderUpdatedEvent;
        public event UpdateProgress UpdateProgressEvent;

        private static DropboxModel model;
        private DropNetClient client;
        private List<MetaData> AllMetadata { get; set; }
        private List<FileInfo> Files { get; set; }

        const string METADATANAME = "/DropBoxMetadata.xml";

        internal DropboxModel()
        {
            try
            {
                DropNetClient client = new DropNetClient("763s7xzmvkxmfkn", "dzl7p8qdt0p1f5v");
                client.GetToken();
                string url = client.BuildAuthorizeUrl();
                Process proc = Process.Start("iexplore", url);
                bool authenticated = false;

                while (!authenticated)
                {
                    System.Threading.Thread.Sleep(5000);

                    try
                    {
                        client.GetAccessToken();
                    }
                    catch { }

                    authenticated = true;
                }

                this.client = client;
            }
            catch (Exception ex)
            {
                LogController.AddEntryDropbox(string.Format("Unable to authenticate user: {0}", ex.Message));
                throw new Exception("Authentication failed");
            }
        }

        internal static DropboxModel Get()
        {
            if (model == null)
            {
                model = new DropboxModel();
            }

            return model;
        }

        internal void Load(string destFolderPath, bool updateMeta, BackgroundWorker worker)
        {
            LogController.AddEntryDropbox("Load started");

            if (updateMeta)
            {
                MetaData root = this.client.GetMetaData("/");
                this.AllMetadata = new List<MetaData>();

                try
                {
                    this.RecursiveGetMetadataForFiles(root,worker);
                    Helper<MetaData>.SaveToXML(destFolderPath + METADATANAME, this.AllMetadata);
                }
                catch (Exception ex)
                {
                    LogController.AddEntryDropbox(string.Format("Problem getting folderstructure: {0}", ex.Message));
                    throw ex;
                }

                try
                {
                    Helper<MetaData>.SaveToXML(destFolderPath + METADATANAME, this.AllMetadata);
                }
                catch (Exception ex)
                {
                    LogController.AddEntryDropbox(string.Format("Problem saving XML metadata: {0}", ex.Message));
                    throw ex;
                }
            }
            else
            {
                try
                {
                    Helper<MetaData>.LoadFromXML(destFolderPath + METADATANAME, this.AllMetadata);
                }
                catch (Exception ex)
                {
                    LogController.AddEntryDropbox(string.Format("Problem while deserializing XML: {0}", ex.Message));
                    throw ex;
                }
            }

            LogController.AddEntryDropbox("Load finished");
        }

        public void SaveToDisk(string destFolderPath, BackgroundWorker worker)
        {
            LogController.AddEntryDropbox("Save started");

            destFolderPath += @"\" + DateTime.Now.ToString("Dropbox_yyyy_MM_dd_hh_mm");

            //Download files
            for (int i = 0; i < this.AllMetadata.Count; i++)
            {
                if (worker.CancellationPending)
                {
                    return;
                }

                try
                {
                    this.DownloadFile(this.AllMetadata[i], destFolderPath);
                    UpdateProgressEvent(i + 1, this.AllMetadata.Count);
                }
                catch (Exception ex)
                {
                    LogController.AddEntryDropbox(string.Format("Unable to download file: {0}. Exception: {1}", this.AllMetadata[i].Path, ex.Message));
                }
            }

            LogController.AddEntryDropbox("Save finished");
        }


        private void DownloadFile(MetaData mData, string dPath)
        {
            Helper<string>.CreateFoldersForFile(mData.Path, dPath);

            try
            {
                //Download file to folder
                byte[] fileInBytes = this.client.GetFile(mData.Path);
                FileStream fs = new FileStream(dPath + mData.Path, FileMode.Create);
                fs.Write(fileInBytes, 0, fileInBytes.Length);
                fs.Close();
            }
            catch (Exception ex)
            {
                LogController.AddEntryDropbox(string.Format("Unable to download filestream: {0}", ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }


        private void RecursiveGetMetadataForFiles(MetaData dir, BackgroundWorker worker)
        {
            if (worker.CancellationPending)
            {
                throw new Exception("Cancelled");
            }

            try
            {
                if (dir.Is_Dir)
                {
                    dir = this.client.GetMetaData(dir.Path);

                    if (FolderUpdatedEvent != null)
                    {
                        FolderUpdatedEvent(dir.Path);
                    }

                    foreach (MetaData child in dir.Contents)
                    {
                        if (child.Is_Dir)
                        {
                            this.RecursiveGetMetadataForFiles(child, worker);
                        }
                        else
                        {
                            this.AllMetadata.Add(child);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogController.AddEntryDropbox(string.Format("Unable to create directories: {0}", ex));
            }
        }
    }
}
