using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Sim.App.Model
{
    class mUpdate
    {
        public static bool HasUpdate(string url_ftp)
        {
            WebClient request = new WebClient();
            request.Credentials = new NetworkCredential("portalsk", "fra@1705");
            try
            {
                byte[] newFileData = request.DownloadData(url_ftp);
                string tempxml = Path.GetTempPath() + "sim_update.xml";
                File.WriteAllBytes(tempxml, newFileData);
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
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\" + "sim_update.xml";

                if (File.Exists(path))
                    return Convert.ToInt32(mXml.xList(path)[0]);
                else
                    return 0;
            }
        }
    }
}
