using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Updater.Common
{
    public class Files
    {
        public static string Sim_Exe
        {
            get { return @"Sim.App.Desktop.exe"; }
        }

        private static string _last_file_update;
        public static string Last_File_Update
        {
            get { return _last_file_update; }
            set { _last_file_update = value; }
        }

        public static string Last_File_Update_Path
        {
            get { return string.Format(@"{0}{1}", Directories.Temp, Last_File_Update); }
        }

        public static string Sim_Update_Xml
        {
            get { return @"sim_update.xml"; }
        }

        public static string Sim_Update_Xml_Path
        {
            get { return string.Format(@"{0}{1}", Directories.Temp, Sim_Update_Xml); }
        }

        public static string Url_File_Fullname
        {
            get { return string.Format(@"{0}{1}", Directories.Url_Http, Sim_Update_Xml); }
        }

        public static string File_Update
        {
            get { return @"sim_update.gz"; }
        }

        public static string File_Install
        {
            get { return @"sim_full_install.gz"; }
        }

        public static string File_Update_FullName
        {
            get { return Directories.Ftp_Update + File_Update; }
        }

        public static string File_Install_FullName
        {
            get { return Directories.Ftp_Update + File_Install; }
        }

        public static string File_Update_Xml_FullName
        {
            get { return Directories.Ftp_Update + Sim_Update_Xml; }
        }

        public static string File_Update_FullName_Temp
        {
            get { return string.Format(@"{0}{1}", Directories.Temp, File_Update); }
        }

        public static string File_Update_Xml_FullName_Temp
        {
            get { return string.Format(@"{0}{1}", Directories.Temp, Sim_Update_Xml); }
        }

        public static string File_xml_DATA
        {
            get { return string.Format(@"{0}{1}", Folders.AppData_Sim, @"xml\sim.data.xml"); }
        }

        public static string File_xml_APP
        {
            get { return string.Format(@"{0}{1}", Folders.AppData_Sim, @"xml\sim.apps.xml"); }
        }
    }
}
