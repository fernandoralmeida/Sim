using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Publish.ViewModel
{
    class Folders
    {
        public static string LocalInstaller
        {
            get { return @"Sim.Install.Local"; }
        }

        public static string Publish
        {
            get { return @"Published"; }
        }

        public static string Publish_Path
        {
            get { return Directories.Desktop + "\\" + Publish; }
        }

        public static string Temp
        {
            get { return @"Temp"; }
        }
    }
}
