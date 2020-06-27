using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Publish.ViewModel
{
    class Files
    {

        public static string Sim_Exe_Name
        {
            get { return @"Sim.App.Desktop.exe"; }
        }

        public static string Sim_Exec_Path
        {
            get
            {
                return Directories.Sim_Dir + Sim_Exe_Name;
            }
        }

        private static string _sim_package_name;
        public static string Sim_Package_Name
        {
            get { return _sim_package_name; }
            set
            {
                _sim_package_name = value;
            }
        }

        private static string _sim_install_name;
        public static string Sim_Install_Name
        {
            get { return _sim_install_name; }
            set
            {
                _sim_install_name = value;
            }
        }

        public static string[] Listar(string xpath, string extensao)
        {
            return System.IO.Directory.GetFiles(xpath, extensao, System.IO.SearchOption.AllDirectories);
        }
    }
}
