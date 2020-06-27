using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.ComponentModel;

namespace Sim.Updater.Model
{
    class mFTP
    {
        public void Upload(string fileName, string ftp_url, BackgroundWorker bgk)
        {
            var ftpWebRequest = (FtpWebRequest)WebRequest.Create(ftp_url);
            ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
            using (var inputStream = File.OpenRead(fileName))
            using (var outputStream = ftpWebRequest.GetRequestStream())
            {
                var buffer = new byte[1024 * 1024];
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
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(_url));
            request.Proxy = null;
            request.Credentials = new NetworkCredential("portalsk", "fra@1705");
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            long size = response.ContentLength;
            response.Close();
            return (int)size;
        }

        public void Download(string ftp_url, string fileName, BackgroundWorker bgk)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[2048];

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp_url);
            request.Credentials = new NetworkCredential("portalsk", "fra@1705");

            //System.Windows.MessageBox.Show(FileSize(ftp_url).ToString());
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
    }
}
