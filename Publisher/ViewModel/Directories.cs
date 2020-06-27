using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Publish.ViewModel
{
    class Directories
    {

        public static string Sim_Dir
        {
            get
            {
                return string.Format(@"{0}{1}", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"\CSharp\Projetos\Sim\Apps\bin\Release\");
            }
        }

        public static string Desktop
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Desktop); }
        }

        public static string Sim_FTP_Pub
        {
            get { return @"ftp://ftp.portalsk.com/portalsk.com/web/sys/sim/pub/"; }
        }

        public static string Sim_FTP_Install
        {
            get { return @"ftp://ftp.portalsk.com/portalsk.com/web/sys/sim/install/new/"; }
        }

        public static string Sim_FTP_Root
        {
            get { return @"ftp://ftp.portalsk.com/portalsk.com/web/sys/sim/"; }
        }

        public static string Sim_FTP_Files
        {
            get { return @"ftp://ftp.portalsk.com/portalsk.com/web/sys/sim/files/new/"; }
        }
    }
}
