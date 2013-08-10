using BackupFromCloud.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BackupFromCloud.Helpers
{
    public class Helper<T>
    {
        public static void CreateFoldersForFile(string sPath, string dPath)
        {
            //Check if folders exist, otherwise create
            string[] folders = sPath.Split('/');
            string currFolder = dPath;

            for (int i = 1; i < folders.Length - 1; i++)
            {
                string path = currFolder + @"\" + folders[i];

                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Problem occured while creating folder structure", ex);
                    }
                }

                currFolder = path;
            }
        }


        public static void LoadFromXML(string path, List<T> list)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            FileStream fs = new FileStream(path, FileMode.Open);
            list = (List<T>)serializer.Deserialize(fs);
        }

        public static void SaveToXML(string path, List<T> list)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                serializer.Serialize(fs, list);

            }
        }
    }
}
