using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.IO;

namespace Sim.Monitoring.ViewModel
{
    using Mvvm.Helpers.Notifiers;
    using Model;
    using System.Collections.ObjectModel;
    using System.Windows.Data;

    class vmShow : NotifyProperty
    {
        #region Declarations
        BackgroundWorker bgWorker = new BackgroundWorker();
        ObservableCollection<mFiles> _list = new ObservableCollection<mFiles>();
        ObservableCollection<FileInfo> _infolist = new ObservableCollection<FileInfo>();
        private Visibility _showinfo;
        private bool _startprogress;
        private object _dcos;
        private object _dcapps;
        private object _dcdesempenho;
        private object _dclistfiles;
        #endregion

        #region Properties
        public Visibility ShowInfo
        {
            get { return _showinfo; }
            set
            {
                _showinfo = value;
                RaisePropertyChanged("ShowInfo");
            }
        }

        public ObservableCollection<mFiles> ListFiles
        {
            get { return _list; }
            set { _list = value;
                RaisePropertyChanged("ListFiles");
            }
        }

        public ObservableCollection<FileInfo> infoListFiles
        {
            get { return _infolist; }
            set
            {
                _infolist = value;
                RaisePropertyChanged("infoListFiles");
            }
        }

        public bool Startprogress
        {
            get { return _startprogress; }
            set
            {
                _startprogress = value;
                RaisePropertyChanged("Startprogress");
            }
        }

        public object DataContextOS
        {
            get { return _dcos; }
            set
            {
                _dcos = value;
                RaisePropertyChanged("DataContextOS");
            }
        }

        public object DataContextApps
        {
            get { return _dcapps; }
            set
            {
                _dcapps = value;
                RaisePropertyChanged("DataContextApps");
            }
        }

        public object DataContextDesempenho
        {
            get { return _dcdesempenho; }
            set
            {
                _dcdesempenho = value;
                RaisePropertyChanged("DataContextDesempenho");
            }
        }

        public object DataContextListFiles
        {
            get { return _dclistfiles; }
            set
            {
                _dclistfiles = value;
                RaisePropertyChanged("DataContextListFiles");
            }
        }
        #endregion

        #region Constructor
        public vmShow()
        {
            //ShowInfo = Visibility.Collapsed;
            Mvvm.Helpers.Observers.GlobalNavigation.Pagina = "INFORMAÇÕES TÉCNICAS";
            Startprogress = true;

            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);

            bgWorker.RunWorkerAsync();
            DataContextDesempenho = new vmDesempenho();
        }
        #endregion

        #region Methods
        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            Startprogress = false;
            ShowInfo = Visibility.Collapsed;
        }

        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //ProgressValue = e.ProgressPercentage;
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
            System.Threading.Thread.Sleep(10);

            ShowInfo = Visibility.Visible;

            DataContextOS = new vmOS();

            System.Reflection.Assembly assemblyname =
                System.Reflection.Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory
                + "Sim.App.Desktop.exe");

            DataContextApps = new vmApps(assemblyname.Location);

            //vmListFiles vmFL = new vmListFiles();

            var _list1 = new ObservableCollection<FileInfo>();

            var exenames = from fullFilename
                           in Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.*", SearchOption.AllDirectories)
                           select Path.GetFileName(fullFilename);

            foreach (string filename in exenames)
            {
                //mFiles fl = new mFiles();
                FileInfo fi = new FileInfo(filename);
                if (fi.Extension == ".dll" || fi.Extension == ".exe" || fi.Extension == ".xml")
                {
                    //fl.Name = Path.GetFileNameWithoutExtension(fi.Name);
                    //fl.Extension = fi.Extension;
                    //System.Windows.MessageBox.Show(fi.Length.ToString());
                    //fl.Sizefile = string.Format("{0}KB", fi.Length / 1024);
                    _list1.Add(fi);
                }
            }

            infoListFiles = _list1;
            /*
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListFiles);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Extension");
            view.GroupDescriptions.Add(groupDescription);
            */
            //DataContextListFiles = vmFL;
        }

        #endregion
    }
}
