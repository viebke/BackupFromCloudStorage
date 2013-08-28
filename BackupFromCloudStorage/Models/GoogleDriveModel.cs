using System;
using System.Diagnostics;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Util;
using Google.Apis.Services;
using Google.Apis.Authentication.OAuth2;
using System.Windows.Forms;
using BackupFromCloud.Dialogs;
using System.Collections.Generic;
using BackupFromCloud.Controllers;
using System.IO;
using BackupFromCloud.Helpers;
using System.Text;
using System.Linq;
using System.Net;
using System.ComponentModel;
using Google.Apis.Authentication;

namespace BackupFromCloud.Models
{
    public class GoogleDriveModel
    {
        public delegate void UpdateProgressGetFiles(int count);
        public event UpdateProgressGetFiles UpdateProgressGetFilesEvent;
        public delegate void UpdateProgressDownloadFiles(int count, int tot);
        public event UpdateProgressDownloadFiles UpdateProgressDownloadFilesEvent;
        public delegate void UpdateName(string name);
        public event UpdateName UpdateNameEvent;

        private List<Google.Apis.Drive.v2.Data.File> files;
        private List<Google.Apis.Drive.v2.Data.File> folders;
        private Dictionary<string, string> dirsId;

        private static GoogleDriveModel model;
        private string CLIENT_ID = "940274689786.apps.googleusercontent.com";
        private string CLIENT_SECRET = "TzIECmZx5Gg1vz7VbVh6GrXF";
        private DriveService service;
        private OAuth2Authenticator<NativeApplicationClient> authenticator;

        internal GoogleDriveModel()
        {
            this.dirsId = new Dictionary<string, string>();
            this.files = new List<Google.Apis.Drive.v2.Data.File>();
            this.folders = new List<Google.Apis.Drive.v2.Data.File>();
        }

        internal static GoogleDriveModel Get()
        {
            if (model == null)
            {
                model = new GoogleDriveModel();
            }

            return model;
        }

        internal void Load()
        {
            FilesResource.ListRequest request;
            LogController.AddEntryGoogle("Load started");

            try
            {
                // Register the authenticator and create the service
                var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description, CLIENT_ID, CLIENT_SECRET);
                this.authenticator = new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthorization);
                this.service = new DriveService(new BaseClientService.Initializer()
                {
                    Authenticator = authenticator
                });

                request = this.service.Files.List();

                try
                {
                    if (this.UpdateNameEvent != null)
                    {
                        UpdateNameEvent(this.GetName());
                    }
                }
                catch
                {
                    LogController.AddEntryGoogle("Unable to get name for logged in user.");
                }
            }
            catch (Exception ex)
            {
                LogController.AddEntryGoogle(string.Format("Unable to authenticate. {0}", ex));
                throw new Exception("Unable to autheticate user", ex);
            }

            try
            {
                do
                {
                    FileList files = request.Execute();
                    List<Google.Apis.Drive.v2.Data.File> listFiles = files.Items.Where(x => x.MimeType != "application/vnd.google-apps.folder").ToList();
                    List<Google.Apis.Drive.v2.Data.File> listFolders = files.Items.Where(x => x.MimeType == "application/vnd.google-apps.folder").ToList();

                    this.folders.AddRange(listFolders);
                    this.files.AddRange(listFiles);

                    request.PageToken = files.NextPageToken;

                    if (this.UpdateProgressGetFilesEvent != null)
                    {
                        UpdateProgressGetFilesEvent(this.files.Count + this.folders.Count);
                    }

                } while (request.PageToken != null);
            }
            catch (Exception ex)
            {
                LogController.AddEntryGoogle(string.Format("Unable to fetch files. {0}", ex));
                throw ex;
            }

            LogController.AddEntryGoogle("Load finished");
        }

        internal void SaveToDisk(string destFolderPath, BackgroundWorker worker)
        {
            LogController.AddEntryGoogle("Save started");

            string name = GetName();

            destFolderPath += @"\GoogleDrive_" + name + DateTime.Now.ToString("yyyy_MM_dd_hh_mm");

            try
            {
                Directory.CreateDirectory(destFolderPath);
            }
            catch (Exception ex)
            {
                LogController.AddEntryGoogle("Unable to add destination folder. Download aborted");
                throw ex;
            }

            this.CreateDirectoryTree(new List<Google.Apis.Drive.v2.Data.File>(this.folders), destFolderPath);

            //Download files
            for (int i = 0; i < this.files.Count; i++)
            {
                if (worker.CancellationPending)
                {
                    return;
                }

                try
                {
                    this.DownloadFile(this.files[i], destFolderPath);
                    UpdateProgressDownloadFilesEvent(i + 1, this.files.Count);
                }
                catch (Exception ex)
                {
                    LogController.AddEntryGoogle(string.Format("Unable to download file: {0}. Exception: {1}", this.files[i].ToString(), ex.Message));
                }
            }

            LogController.AddEntryGoogle("Save finished");
        }

        private string GetName()
        {
            string name = ((AboutResource)this.service.About).Get().Execute().Name;
            return name;
        }

        private void CreateDirectoryTree(List<Google.Apis.Drive.v2.Data.File> list, string dPath)
        {
            foreach (Google.Apis.Drive.v2.Data.File file in list)
            {
                try
                {
                    this.dirsId.Add(file.Id, file.Title);
                    Directory.CreateDirectory(dPath + @"\" + file.Title);
                }
                catch (Exception ex)
                {
                    LogController.AddEntryGoogle(string.Format("Unable to careate directories. {0}", ex));
                    throw ex;
                }
            }
        }

        private void DownloadFile(Google.Apis.Drive.v2.Data.File file, string destFolderPath)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                         new Uri(file.DownloadUrl));
                authenticator.ApplyAuthenticationToRequest(request);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string parentsPath = file.Parents == null ? string.Empty : this.dirsId[file.Parents[0].Id] + @"\";

                using (FileStream fs = System.IO.File.Create(destFolderPath + @"\" + parentsPath + file.Title + "." + file.FileExtension))
                {
                    response.GetResponseStream().CopyTo(fs);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                LogController.AddEntryGoogle(string.Format("Unable to save file {1} to disk: {0}", ex.Message, file.Title));
            }
        }

        private static IAuthorizationState GetAuthorization(NativeApplicationClient arg)
        {
            // Get the auth URL:
            IAuthorizationState state = new AuthorizationState(new[] { DriveService.Scopes.Drive.GetStringValue() });
            state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);
            Uri authUri = arg.RequestUserAuthorization(state);

            // Request authorization from the user (by opening a browser window):
            Process.Start(authUri.ToString());
            AuthenticationCode authCodeForm = new AuthenticationCode();

            if (authCodeForm.ShowDialog() == DialogResult.OK)
            {
                return arg.ProcessUserAuthorization(authCodeForm.Code, state);
            }

            return null;
        }

    }
}
