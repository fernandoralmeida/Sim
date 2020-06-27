using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sim.Common.Helpers
{
    public static class Folders
    {
        public static string SystemDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static string SimPDF = "Sim.Desktop.PDFs";
        public static string Pdf = "Pdfs";
        public static string SimApp = AppDomain.CurrentDomain.BaseDirectory;
        public static string Xps = "Rels";

        public static void Create(string pathFolder, string nameFolder)
        {

            DirectoryInfo pasta = new DirectoryInfo(string.Format(@"{0}\{1}", pathFolder, nameFolder));

            if (!Directory.Exists(pasta.FullName))
            {
                Directory.CreateDirectory(pasta.FullName);
            }

        }

        public static void Delete(string pathFolder, string nameFolder)
        {

            DirectoryInfo pasta = new DirectoryInfo(string.Format(@"{0}\{1}", pathFolder, nameFolder));

            if (Directory.Exists(pasta.FullName))
                Directory.Delete(pasta.FullName, true);

        }
    }
}
