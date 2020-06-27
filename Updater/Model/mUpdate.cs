using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.ComponentModel;


namespace Sim.Updater.Model
{
    class mUpdate
    {
        public static bool HasUpdate(string url)
        {
            WebClient request = new WebClient();
            request.Credentials = new NetworkCredential("portalsk", "fra@1705");
            try
            {
                byte[] newFileData = request.DownloadData(url);
                string tempxml = Common.Directories.Temp + Common.Files.Sim_Update_Xml;
                File.WriteAllBytes(tempxml, newFileData);
                //System.Windows.MessageBox.Show(YesNo(tempxml).ToString());
                return YesNo(tempxml);
            }
            catch (WebException ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static bool YesNo(string lastversionfile)
        {
            if (CurrentUpdate < LastUpdate(lastversionfile))
                return true;
            else
                return false;
        }

        private static int LastUpdate(string filepath)
        {
            return Convert.ToInt32(mXml.xList(filepath)[0]);
        }

        private static int CurrentUpdate
        {
            get
            {
                string path = Common.Directories.CurrentApp + @"\" + Common.Files.Sim_Update_Xml;

                if (System.IO.File.Exists(path))
                    return Convert.ToInt32(mXml.xList(path)[0]);
                else
                    return 0;
            }
        }
    }
}
