using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.ComponentModel;

namespace Sim.Common.Helpers
{
    public class Ftp
    {

        public bool DeleteFileFTP(Uri serverUri)
        {

            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                return false;
            }
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverUri);
            request.Credentials = new NetworkCredential("portalsk", "fra@1705");
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            //Console.WriteLine("Delete status: {0}", response.StatusDescription);
            response.Close();
            return true;
        }

        public void UploadFileFTP(string fileName, string ftp_url, BackgroundWorker bgk)
        {
            
            if (FileSize(ftp_url) > 0)
            {
                DeleteFileFTP(new Uri(ftp_url));
            }
            
            var request = (FtpWebRequest)FtpWebRequest.Create(ftp_url);
            request.Credentials = new NetworkCredential("portalsk", "fra@1705");
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.UseBinary = true;
            request.KeepAlive = true;
            request.UsePassive = true;

            using (var inputStream = File.OpenRead(fileName))
            using (var outputStream = request.GetRequestStream())
            {
                var buffer = new byte[2048];
                int totalReadBytesCount = 0;
                int readBytesCount;
                while ((readBytesCount = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outputStream.Write(buffer, 0, readBytesCount);
                    totalReadBytesCount += readBytesCount;
                    var progress = totalReadBytesCount * 100.0 / inputStream.Length;
                    bgk.ReportProgress((int)progress);
                }
            }
        }


        private int FileSize(string _url)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(_url));
                request.Proxy = null;
                request.Credentials = new NetworkCredential("portalsk", "fra@1705");
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                long size = response.ContentLength;
                response.Close();
                return (int)size;
            }
            catch
            {
                return 0;
            }
        }

        public void DownloadFileFTP(string ftp_url, string fileName, BackgroundWorker bgk)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[2048];

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp_url);
            request.Credentials = new NetworkCredential("portalsk", "fra@1705");


            int filesize = FileSize(ftp_url);

            request.Method = WebRequestMethods.Ftp.DownloadFile;

            Stream reader = request.GetResponse().GetResponseStream();
            FileStream fileStream = new FileStream(fileName, FileMode.Create);

            int totalReadBytesCount = 0;
            while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
            {

                //System.Threading.Thread.Sleep(10);

                if (bytesRead == 0)
                    break;

                fileStream.Write(buffer, 0, bytesRead);
                totalReadBytesCount += bytesRead;
                var progress = totalReadBytesCount * 100.0 / filesize;
                bgk.ReportProgress((int)progress);
            }

            fileStream.Close();
        }

        public static IEnumerable<string> GetFilesInFtpDirectory(string url)
        {
            // Get the object used to communicate with the server.
            var request = (FtpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential("portalsk", "fra@1705");

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream);
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();    
                        yield return line;
                    }
                }
            }
        }

        public static ObservableCollection<string> ListFilesFtpDirectory(string url)
        {

            ObservableCollection<string> list = new ObservableCollection<string>();

            // Get the object used to communicate with the server.
            var request = (FtpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential("portalsk", "fra@1705");

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream);
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        list.Add(line);
                    }
                }
            }

            return list;
        }

    }
}
