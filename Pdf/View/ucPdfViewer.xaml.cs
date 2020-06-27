using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Net;
using System.Security;
using System.Runtime.ExceptionServices;
using System.IO;

namespace Sim.Pdf.View
{
    using Model;
    using Helpers;
    /// <summary>
    /// Interaction logic for ucPdfViewer.xaml
    /// </summary>
    public partial class ucPdfViewer : UserControl
    {

        int pages = 0;
        int currentpage = 0;

        public double AlternateWidth()
        {
            double width = SystemParameters.PrimaryScreenWidth;

            width = (width / 2);

            return width;
        }

        public double AlternateHeight()
        {
            
            double height = SystemParameters.PrimaryScreenHeight;

            height = height - (height * 0.2);

            return height;
        }
        
        public ucPdfViewer(string file)
        {
            InitializeComponent();

            filedownloading = file.Replace(@"\", @"/");
            string arquivo = file.Replace(@"/", "");
            arquivo = arquivo.Replace(@"\", "");
            filedownloaded = string.Format(@"{0}{1}\{2}", Common.Helpers.Folders.SimApp, Common.Helpers.Folders.Pdf, arquivo.Remove(0, 5));
            
            this.CheckFile();
        }

        [SecurityCritical]
        [HandleProcessCorruptedStateExceptions]        
        BitmapSource ShowPDF(string filename)
        {
            
            IntPtr ctx = IntPtr.Zero;
            IntPtr stm = IntPtr.Zero;
            IntPtr doc = IntPtr.Zero;
            IntPtr p = IntPtr.Zero;

            try
            {
                //MessageBox.Show(filename);

                ctx = Methods.NewContext();
                stm = Methods.OpenFile(ctx, filename);
                doc = Methods.OpenDocumentStream(ctx, stm);
                p = Methods.LoadPage(doc, currentpage);

                pages = Methods.CountPages(doc);

                Rectangle b = new Rectangle();
                Methods.BoundPage(doc, p, ref b);

                return PdfImage.CreateBitmapSourceFromBitmap(PdfImage.Page(ctx, doc, p, b));

            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);           
            }
            finally
            {
                Methods.FreePage(doc, p);
                Methods.CloseDocument(doc);
                Methods.CloseStream(stm);
                Methods.FreeContext(ctx);
            }
        }        

        private void goFirst_Click(object sender, RoutedEventArgs e)
        {
            if (currentpage != 0)
            {
                currentpage = 0;
                imgPDF.Source = ShowPDF(filedownloaded);
                Rotulo.Content = string.Format("{0}/{1}", currentpage + 1, pages);
            }
        }

        private void goPrev_Click(object sender, RoutedEventArgs e)
        {
            if (currentpage > 0)
            {
                currentpage --;
                imgPDF.Source = ShowPDF(filedownloaded);
                Rotulo.Content = string.Format("{0}/{1}", currentpage + 1, pages);
            }
        }

        private void goNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentpage < pages - 1)
            {
                currentpage ++;
                imgPDF.Source = ShowPDF(filedownloaded);
                Rotulo.Content = string.Format("{0}/{1}", currentpage + 1, pages);
            }
        }

        private void goLast_Click(object sender, RoutedEventArgs e)
        {
            if (currentpage != pages - 1)
            {
                currentpage = pages - 1;
                imgPDF.Source = ShowPDF(filedownloaded);
                Rotulo.Content = string.Format("{0}/{1}", currentpage + 1, pages);
            }
        }

        private void goPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(filedownloaded);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Apps.Alerta!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Downloading File
        /// <summary>
        /// Variaveis
        /// </summary>
        string filedownloading = string.Empty;
        string filedownloaded = string.Empty;

        /// <summary>
        /// Parte logica relacionada ao download de arquivos
        /// </summary>
        private void CheckFile()
        {
            try
            {

                if (!(File.Exists(filedownloaded)))
                {
                    gridDown.Visibility = Visibility.Visible;
                    //DownloadCurrent(filedownloading);
                    FileDownload(filedownloading);
                    //DownloadFile(filedownloading, filedownloaded);
                    
                }
                else
                {
                    filedownloaded = filedownloaded.Replace(@"/", @"\");
                    imgPDF.Source = ShowPDF(filedownloaded);
                    gridDown.Visibility = Visibility.Collapsed;
                    gridHeader.Visibility = Visibility.Visible;
                    gridPDF.Visibility = Visibility.Visible;
                    Rotulo.Content = string.Format("{0}/{1}", currentpage + 1, pages);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Sim.Apps.Alerta!"); }
        }

        /// <summary>
        /// Inicia o download
        /// </summary>
        /// <param name="url"></param>
        private void FileDownload(string url)
        {
            try
            {
                WebClient cliente = new WebClient();
                
                cliente.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloadComplete);
                cliente.DownloadProgressChanged += new DownloadProgressChangedEventHandler(FileDownloadProgress);
                //string UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.7) Gecko/20091221 Firefox/3.5.7";
                cliente.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                //cliente.OpenRead(filedownloading);
                cliente.UseDefaultCredentials = true;

                cliente.DownloadFileAsync(new Uri(url), filedownloaded);
                //cliente.DownloadFile(url, filedownloaded);
                

                //DownloadCurrent(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Reporta o progresso do download
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            //label1.Text = String.Format("Downloading... {0}%", e.ProgressPercentage);
            pgBar.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Ações ao terminar o download
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            filedownloaded = filedownloaded.Replace(@"/", @"\");
            imgPDF.Source = ShowPDF(filedownloaded);
            gridDown.Visibility = Visibility.Collapsed;
            gridHeader.Visibility = Visibility.Visible;
            gridPDF.Visibility = Visibility.Visible;
            Rotulo.Content = string.Format("{0}/{1}", currentpage + 1, pages);
        }

        private static void DownloadCurrent(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.Timeout = 3000;            
            webRequest.BeginGetResponse(new AsyncCallback(PlayResponeAsync), webRequest);
        }

        private static void PlayResponeAsync(IAsyncResult asyncResult)
        {
            int total = 0;
            int received = 0;
            HttpWebRequest webRequest = (HttpWebRequest)asyncResult.AsyncState;

            try
            {
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asyncResult))
                {
                    byte[] buffer = new byte[1024];

                    //MessageBox.Show(Common.Helpers.Folders.SimApp + @"cache\texte.pdf");

                    FileStream fileStream = File.OpenWrite(Common.Helpers.Folders.SimApp + @"cache\texte.pdf");
                    using (Stream input = webResponse.GetResponseStream())
                    {
                        total = (int)input.Length;

                        int size = input.Read(buffer, 0, buffer.Length);
                        while (size > 0)
                        {
                            fileStream.Write(buffer, 0, size);
                            received += size;
                            size = input.Read(buffer, 0, buffer.Length);
                        }
                    }

                    fileStream.Flush();
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Sim.App.Alerta!");
            }
        }

        /*
        private static void DownlaodAsync(string url)
        {
            //Create a stream for the file
            Stream stream = null;

            //This controls how many bytes to read at a time and send to the client
            int bytesToRead = 10000;

            // Buffer to read bytes in chunk size specified above
            byte[] buffer = new Byte[bytesToRead];

            // The number of bytes read
            try
            {
                //Create a WebRequest to get the file
                HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);

                //Create a response for this request
                HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

                if (fileReq.ContentLength > 0)
                    fileResp.ContentLength = fileReq.ContentLength;

                //Get the Stream returned from the response
                stream = fileResp.GetResponseStream();

                // prepare the response to the client. resp is the client Response
                var resp = HttpContext.Current.Response;

                //Indicate the type of data being sent
                resp.ContentType = "application/octet-stream";

                //Name the file 
                resp.AddHeader("Content-Disposition", "attachment; filename=\"" + url + "\"");
                resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());

                int length;
                do
                {
                    // Verify that the client is connected.
                    if (resp.IsClientConnected)
                    {
                        // Read data into the buffer.
                        length = stream.Read(buffer, 0, bytesToRead);

                        // and write it out to the response's output stream
                        resp.OutputStream.Write(buffer, 0, length);

                        // Flush the data
                        resp.Flush();

                        //Clear the buffer
                        buffer = new Byte[bytesToRead];
                    }
                    else
                    {
                        // cancel the download if client has disconnected
                        length = -1;
                    }
                } while (length > 0); //Repeat until no data is read
            }
            finally
            {
                if (stream != null)
                {
                    //Close the input stream
                    stream.Close();
                }
            }
        }
        */

        public static int DownloadFile(String remoteFilename, String localFilename)
        {
            // Function will return the number of bytes processed
            // to the caller. Initialize to 0 here.
            int bytesProcessed = 0;

            // Assign values to these objects here so that they can
            // be referenced in the finally block
            Stream remoteStream = null;
            Stream localStream = null;
            WebResponse response = null;

            // Use a try/catch/finally block as both the WebRequest and Stream
            // classes throw exceptions upon error
            try
            {
                // Create a request for the specified remote file name
                WebRequest request = WebRequest.Create(remoteFilename);
                if (request != null)
                {
                    // Send the request to the server and retrieve the
                    // WebResponse object 
                    response = request.GetResponse();
                    if (response != null)
                    {
                        // Once the WebResponse object has been retrieved,
                        // get the stream object associated with the response's data
                        remoteStream = response.GetResponseStream();

                        // Create the local file
                        localStream = File.Create(localFilename);

                        // Allocate a 1k buffer
                        byte[] buffer = new byte[1024];
                        int bytesRead;

                        // Simple do/while loop to read from stream until
                        // no bytes are returned
                        do
                        {
                            // Read data (up to 1k) from the stream
                            bytesRead = remoteStream.Read(buffer, 0, buffer.Length);

                            // Write the data to the local file
                            localStream.Write(buffer, 0, bytesRead);

                            // Increment total bytes processed
                            bytesProcessed += bytesRead;
                        } while (bytesRead > 0);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // Close the response and streams objects here 
                // to make sure they're closed even if an exception
                // is thrown at some point
                if (response != null) response.Close();
                if (remoteStream != null) remoteStream.Close();
                if (localStream != null) localStream.Close();
            }

            // Return total bytes processed to caller.
            return bytesProcessed;
        }

        #endregion

    }
}
