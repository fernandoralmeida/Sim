using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sim.Common.Helpers
{
    public static class Files
    {
        public static string[] List(string diretorio)
        {
            return Directory.GetFiles(diretorio, "*.*", SearchOption.AllDirectories);
        }

        public static List<string> ListFiles(string diretorio, string extencao)
        {
            
            List<string> listfilenames = new List<string>();

            var filenames = from fullFilename
                in Directory.EnumerateFiles(diretorio, extencao)
                            select Path.GetFileName(fullFilename);

            foreach (string filename in filenames)
            {
                listfilenames.Add(filename);
            }

            return listfilenames;// Directory.GetFiles(diretorio, "*.*", SearchOption.AllDirectories);
        }
        
        public static bool Delete(string pathfile)
        {
            // verifica se o caminho exite
            if (Directory.Exists(pathfile))
            {
                string[] nfiles = Directory.GetFiles(pathfile);

                //deleta um arquivo de cada vez
                foreach (string files in nfiles)
                {
                    string arquivo = Path.GetFullPath(files);
                    File.Delete(arquivo);
                }

                return true;
            }
            else
                return false;
        }

        public static bool DeleteAllFilesInFolder(string pathfile)
        {
            // verifica se o caminho exite
            if (Directory.Exists(pathfile))
            {
                string[] nfiles = Directory.GetFiles(pathfile);

                //deleta um arquivo de cada vez
                foreach (string files in nfiles)
                {
                    string arquivo = Path.GetFullPath(files);
                    File.Delete(arquivo);
                }

                return true;
            }
            else
                return false;
        }
    }


}
