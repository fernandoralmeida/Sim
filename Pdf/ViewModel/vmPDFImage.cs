using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Security;
using System.Runtime.ExceptionServices;
using System.ComponentModel;

namespace Sim.Pdf.ViewModel
{

    using Mvvm.Helpers.Notifiers;
    using Mvvm.Helpers.Commands;
    using Model;
    using View;
    using Helpers;

    class vmPDFImage : NotifyProperty
    {

        #region Constructor

        public vmPDFImage(string urlbase, string file)
        {
            InDownload = Visibility.Collapsed;
            CommandBar = Visibility.Collapsed;

            mNetObserver.GlobalPropertyChanged += new PropertyChangedEventHandler(mNetObserver_GlobalPropertyChanged);

            _filedownload = file.Replace(@"\", "/");
            _filesave = file.Replace(@"\", "_");

            if (_filesave.StartsWith("_", StringComparison.OrdinalIgnoreCase))
                _filesave = _filesave.Remove(0, 1);
            
            _url = urlbase;

            if (!System.IO.File.Exists(_cahce + _filesave))
            {
                mNetObserver.DonwloadComplete = false;
                InDownload = Visibility.Visible;
                netwoork.DownloadFile(urlbase + _filedownload, _cahce + _filesave);
                PagePDF = ShowPDF(_cahce + _filesave);
            }
            else
                mNetObserver.DonwloadComplete = true;
        }

        #endregion

        #region Declarations

        private mNet netwoork = new mNet();        

        private string _filesave;
        private string _filedownload;
        private string _url;
        private string _cahce = string.Format(@"{0}{1}\", 
            Sim.Common.Helpers.Folders.SimApp, 
            Sim.Common.Helpers.Folders.Pdf);

        private string _paginas;
        private string _percent;
        private string _donloaded;
        private string _speed;
        private int _reportprogress;
        private int pages = 0;
        private int currentpage = 0;

        private ImageSource _pagepdf;

        private Visibility _cmdbar;
        private Visibility _indown;

        private ICommand _cmdnextpage;
        private ICommand _cmdlastpage;
        private ICommand _cmdprevpage;
        private ICommand _cmdfirstpage;
        private ICommand _cmdprint;

        #endregion

        #region Properties

        public ImageSource PagePDF
        { get { return _pagepdf; }
            set { _pagepdf = value;
                RaisePropertyChanged("PagePDF");
            }
        }

        public string Paginas
        {
            get { return _paginas; }
            set { _paginas = value; RaisePropertyChanged("Paginas"); }
        }

        public string Percent
        { get { return _percent; }
            set { _percent = value;
                RaisePropertyChanged("Percent");
            }
        }

        public string Downloaded {
            get { return _donloaded; }
            set { _donloaded = value;
                RaisePropertyChanged("Downloaded");
            }
        }

        public string Speed {
            get { return _speed; }
            set { _speed = value;
                RaisePropertyChanged("Speed");
            }
        }

        public int ReportProgress {
            get { return _reportprogress; }
            set { _reportprogress = value;
                RaisePropertyChanged("ReportProgress");
            }
        }

        public Visibility CommandBar {
            get { return _cmdbar; }
            set { _cmdbar = value;
                RaisePropertyChanged("CommandBar");
            }
        }

        public Visibility InDownload
        {
            get { return _indown; }
            set { _indown = value; 
                RaisePropertyChanged("InDownload"); 
            }
        }

        #endregion

        #region Commands

        public ICommand CommandFirstPage
        {
            get
            {
                if (_cmdfirstpage == null)
                    _cmdfirstpage = new DelegateCommand(ExecuteCommandFirstPage, null);
                return _cmdfirstpage;
            }
        }

        public ICommand CommandPrevPage
        {
            get
            {
                if (_cmdprevpage == null)
                    _cmdprevpage = new DelegateCommand(ExecuteCommandPrevPage, null);
                return _cmdprevpage;
            }
        }

        public ICommand CommandNextPage {
            get {
                if (_cmdnextpage == null)
                    _cmdnextpage = new DelegateCommand(ExecuteCommandNextPage, null);
                return _cmdnextpage;
            }
        }

        public ICommand CommandLastPage
        {
            get {
                if (_cmdlastpage == null)
                    _cmdlastpage = new DelegateCommand(ExecuteCommandLastPage, null);
                return _cmdlastpage;
            }
        }

        public ICommand CommandPrint
        {
            get
            {
                if (_cmdprint == null)
                    _cmdprint = new DelegateCommand(ExecuteCommandPrint, null);
                return _cmdprint;
            }
        }

        #endregion

        #region Methods

        private void ExecuteCommandFirstPage(object obj)
        {
            if (currentpage != 0)
            {
                currentpage = 0;
                PagePDF = ShowPDF(_cahce + _filesave);
                Paginas = string.Format("{0}/{1}", currentpage + 1, pages);
            }
        }

        private void ExecuteCommandPrevPage(object obj)
        {
            if (currentpage > 0)
            {
                currentpage--;
                PagePDF = ShowPDF(_cahce + _filesave);
                Paginas = string.Format("{0}/{1}", currentpage + 1, pages);
            }
        }

        private void ExecuteCommandNextPage(object obj)
        {
            if (currentpage < pages - 1)
            {
                currentpage++;
                PagePDF = ShowPDF(_cahce + _filesave);
                Paginas = string.Format("{0}/{1}", currentpage + 1, pages);
            }
        }

        private void ExecuteCommandLastPage(object obj)
        {
            if (currentpage != pages - 1)
            {
                currentpage = pages - 1;
                PagePDF = ShowPDF(_cahce + _filesave);
                Paginas = string.Format("{0}/{1}", currentpage + 1, pages);
            }
        }

        private void ExecuteCommandPrint(object obj)
        {
            try
            {
                System.Diagnostics.Process.Start(_cahce + _filesave);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Apps.Alerta!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void mNetObserver_GlobalPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            { 
                case "Speed":
                    Speed = mNetObserver.Speed;
                    break;

                case "IntPercent":
                    ReportProgress = mNetObserver.IntPercent;
                    break;

                case "Percent":
                    Percent = mNetObserver.Percent;
                    break;

                case "Downloaded":
                    Downloaded = mNetObserver.Downloaded;
                    break;

                case "DonwloadComplete":
                    if (mNetObserver.DonwloadComplete == true)
                    {
                        PagePDF = ShowPDF(_cahce + _filesave);
                        CommandBar = Visibility.Visible;
                        InDownload = Visibility.Collapsed;
                    }
                    break;
            }
        }

        [SecurityCritical]
        [HandleProcessCorruptedStateExceptions]
        private BitmapSource ShowPDF(string filename)
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

        #endregion
    }
}
