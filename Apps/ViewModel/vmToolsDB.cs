using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace Sim.App.ViewModel
{
    using Modulos.Accounts;
    using Common.Helpers;
    using Mvvm.Helpers.Notifiers;
    using Mvvm.Helpers.Commands;

    class vmToolsDB : NotifyProperty
    {
        #region Declarations

        private BackgroundWorker bgWorker = new BackgroundWorker();

        private bool _inbackup = false;
        private bool _inrestore = false;
        private bool _onnuvem;

        private bool _indelete = false;

        private bool _textboxenabled;
        private bool _startprogressbar;

        private string _connectionstring;
        private string _buttonconn;
        private string _selecteditem;
        private string _labelblackbox;
        private string _backuppath;
        private string _onnuvemstring;

        private string _nuvemftp = "ftp://ftp.portalsk.com/portalsk.com/web/sys/sim/backups/pmjahu/";

        private int _progress;
        
        private ICommand _commandsavedatabase;
        private ICommand _commandbackup;
        private ICommand _commandrestore;
        private ICommand _commanddelete;
        private ICommand _commanddir;

        private Visibility _blackbox;
        private Visibility _localbackupvisibility;

        private IEnumerable<string> _backuplist;

        #endregion

        #region Properties

        public Visibility Blackbox
        {
            get { return _blackbox; }
            set {
                _blackbox = value;
                RaisePropertyChanged("Blackbox");
            }
        }

        public Visibility LocalBackupVisibility
        {
            get { return _localbackupvisibility; }
            set
            {
                _localbackupvisibility = value;
                RaisePropertyChanged("LocalBackupVisibility");
            }
        }

        public IEnumerable<string> BackupList
        {
            get { return _backuplist; }
            set
            {
                _backuplist = value;
                RaisePropertyChanged("BackupList");
            }
        }

        public string SelectedItem
        {
            get { return _selecteditem; }
            set
            {
                _selecteditem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }

        public string ButtonConnection
        {
            get { return _buttonconn; }
            set
            {
                _buttonconn = value;
                RaisePropertyChanged("ButtonConnection");
            }
        }

        public string ConnectionString
        {
            get { return _connectionstring; }
            set
            {
                _connectionstring = value;
                RaisePropertyChanged("ConnectionString");
            }
        }

        public string LabelBalckbox
        {
            get { return _labelblackbox; }
            set
            {
                _labelblackbox = value;
                RaisePropertyChanged("LabelBalckbox");
            }
        }

        public string BackupPath
        {
            get { return _backuppath; }
            set { _backuppath = value;
            RaisePropertyChanged("BackupPath");
            }
        }

        public bool TextboxEnabled
        {
            get { return _textboxenabled; }
            set
            {
                _textboxenabled = value;
                RaisePropertyChanged("TextboxEnabled");
            }
        }

        public bool StartProgressBar
        {
            get { return _startprogressbar; }
            set
            {
                _startprogressbar = value;
                RaisePropertyChanged("StartProgressBar");
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                RaisePropertyChanged("Progress");
            }
        }

        public bool OnNuvem
        {
            get { return _onnuvem; }
            set
            {
                _onnuvem = value;

                RaisePropertyChanged("OnNuvem");

                if (_onnuvem == false)
                {
                    LocalBackupVisibility = Visibility.Visible;
                    BackupPath = XmlFile.Value("sim.apps", "App", "Backup", "Path");
                    bgWorker.RunWorkerAsync();
                    OnNuvemString = "BACKUP LOCAL";
                }
                else
                {
                    LocalBackupVisibility = Visibility.Collapsed;
                    BackupPath = _nuvemftp;
                    bgWorker.RunWorkerAsync();
                    OnNuvemString = "BACKUP NUVEM";
                }                
            }
        }

        public string OnNuvemString
        {
            get { return _onnuvemstring; }
            set
            {
                _onnuvemstring = value;
                RaisePropertyChanged("OnNuvemString");
            }
        }

        #endregion

        #region Commands

        public ICommand CommandBackup
        {
            get
            {
                if (_commandbackup == null)
                    _commandbackup = new DelegateCommand(ExecuteCommandBackup, null);
                return _commandbackup;
            }
        }

        public ICommand CommandSaveDB
        {
            get
            {
                if (_commandsavedatabase == null)
                    _commandsavedatabase = new DelegateCommand(ExecuteCommandSaveDB, null);
                return _commandsavedatabase;
            }
        }

        public ICommand CommandRestore
        {
            get
            {
                if (_commandrestore == null)
                    _commandrestore = new DelegateCommand(ExecuteCommandRestore, null);

                return _commandrestore;
            }
        }

        public ICommand CommandDelete
        {
            get
            {
                if (_commanddelete == null)
                    _commanddelete = new DelegateCommand(ExecuteCommandDelete, null);

                return _commanddelete;
            }
        }

        public ICommand CommandDir
        {
            get {
                if (_commanddir == null)
                    _commanddir = new DelegateCommand(ExecuteCommandDir, null);
                return _commanddir;
            }
        }

        #endregion

        #region Methods

        private void ExecuteCommandSaveDB(object obj)
        {
            if (ButtonConnection == "Alterar Connection String")
            {
                ButtonConnection = "Gravar Alterações";
                TextboxEnabled = true;
            }
            else
            {
                ButtonConnection = "Alterar Connection String";
                TextboxEnabled = false;
                XmlFile.SaveXml("sim.data", ConnectionString, "Rede");
            }
        }

        private void ExecuteCommandBackup(object obj)
        {
            if (AccountOn.Acesso == (int)AccountAccess.Master)
                if (MessageBox.Show("Iniciar Backup da Dase de Dados?", "Sim.App.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _inbackup = true;
                    _indelete = false;
                    _inrestore = false;
                    LabelBalckbox = "Backup em andamento!";
                    bgWorker.RunWorkerAsync();
                }
        }

        private void ExecuteCommandRestore(object obj)
        {
            SelectedItem = (string)obj;
            if (AccountOn.Acesso == (int)AccountAccess.Master)
                if (MessageBox.Show("Restaurar Backup " + SelectedItem + "?", "Sim.App.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _inbackup = false;
                    _indelete = false;
                    _inrestore = true;
                    LabelBalckbox = "Restaurando Backup!";
                    bgWorker.RunWorkerAsync();
                }
        }

        private void ExecuteCommandDelete(object obj)
        {
            SelectedItem = (string)obj;
            if (AccountOn.Acesso == (int)AccountAccess.Master)
                if (MessageBox.Show("Excluir Backup " + SelectedItem + "?", "Sim.App.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _indelete = true;
                    _inbackup = false;
                    _inrestore = false;
                    bgWorker.RunWorkerAsync();
                }
        }

        private void ExecuteCommandDir(object obj)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                BackupPath = dialog.SelectedPath;

            if (!System.IO.Directory.Exists(BackupPath + @"\sim_backups"))
            {
                System.IO.Directory.CreateDirectory(BackupPath + @"\sim_backups");
                string t = BackupPath + @"\sim_backups";
                t = t.Replace(@"\\", @"\");
                BackupPath = t;
            }
            
            XmlFile.SaveXml("sim.apps", BackupPath, "Path");
            BackupPath = XmlFile.Value("sim.apps", "App", "Backup", "Path");
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            StartProgressBar = true;
            Blackbox = Visibility.Visible;

            #region Backup

            if (_inbackup == true)
            {
                string backupfolder = AppDomain.CurrentDomain.BaseDirectory + @"backup\";
                string uploadfolder = AppDomain.CurrentDomain.BaseDirectory + @"upload\";

                try
                {
                    //System.IO.Directory.Delete(_root, true);

                    //ZipUnzip.ProgressDelegate txt;
                    
                    string file = @"" +
                        DateTime.Now.Year.ToString("0000") +
                        DateTime.Now.Month.ToString("00") +
                        DateTime.Now.Day.ToString("00") +
                        "_" +
                        DateTime.Now.Hour.ToString("00") +
                        DateTime.Now.Minute.ToString("00") +
                        DateTime.Now.Second.ToString("00") +
                        ".gz";

                    string backupfile = uploadfolder + file;

                    string db1file = ConnectionString + "SimDataBase1.mdb";
                    string db2file = ConnectionString + "SimDataBase2.mdb";
                    string db3file = ConnectionString + "SimDataBase3.mdb";
                    string db4file = ConnectionString + "SimDataBase4.mdb";

                    if (ConnectionString == @"|DataDirectory|\DataBase\")
                    {
                        db1file = AppDomain.CurrentDomain.BaseDirectory + @"DataBase\SimDataBase1.mdb";
                        db2file = AppDomain.CurrentDomain.BaseDirectory + @"DataBase\SimDataBase2.mdb";
                        db3file = AppDomain.CurrentDomain.BaseDirectory + @"DataBase\SimDataBase3.mdb";
                        db4file = AppDomain.CurrentDomain.BaseDirectory + @"DataBase\SimDataBase4.mdb";
                    }
                        
                    if (!System.IO.Directory.Exists(backupfolder))
                        System.IO.Directory.CreateDirectory(backupfolder);

                    if (!System.IO.Directory.Exists(uploadfolder))
                        System.IO.Directory.CreateDirectory(uploadfolder);

                    System.IO.File.Copy(db1file, backupfolder + "SimDataBase1.mdb", true);
                    System.IO.File.Copy(db2file, backupfolder + "SimDataBase2.mdb", true);
                    System.IO.File.Copy(db3file, backupfolder + "SimDataBase3.mdb", true);
                    System.IO.File.Copy(db4file, backupfolder + "SimDataBase4.mdb", true);

                    //MessageBox.Show(uploadfolder);

                    ZipUnzip.CompressDirectory(backupfolder, backupfile, null);

                    if (OnNuvem == true)
                        new Ftp().UploadFileFTP(backupfile, _nuvemftp + file, bgWorker);
                    else
                        System.IO.File.Copy(backupfile, BackupPath + @"\" + file, true);


                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    System.Windows.MessageBox.Show(ex.Message,
                        "Sim.App.DataTools",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Information);
                }

                System.IO.Directory.Delete(backupfolder, true);
                System.IO.Directory.Delete(uploadfolder, true);
            }

            #endregion

            #region DeleteBackup
            if (_indelete == true)
            {
                LabelBalckbox = "Excluidno Backup " + SelectedItem + "!";

                if (OnNuvem == true)
                    System.IO.File.Delete(BackupPath + @"\" + SelectedItem);
                else
                    new Ftp().DeleteFileFTP(new Uri(_nuvemftp + SelectedItem));
            }
            #endregion

            #region RestoreBackup
            if (_inrestore == true)
            {
                string downloadfolder = AppDomain.CurrentDomain.BaseDirectory + @"download\";

                try
                {

                    string backupfile = downloadfolder + SelectedItem;

                    if (!System.IO.Directory.Exists(downloadfolder))
                        System.IO.Directory.CreateDirectory(downloadfolder);

                    string restorefolder = ConnectionString;

                    if (ConnectionString == @"|DataDirectory|\DataBase\")
                    {
                        restorefolder = AppDomain.CurrentDomain.BaseDirectory + @"DataBase\";
                    }

                    if (OnNuvem == true)
                    {
                        new Ftp().DownloadFileFTP(_nuvemftp + SelectedItem, downloadfolder + SelectedItem, bgWorker);
                        ZipUnzip.DecompressDirectory(downloadfolder + SelectedItem, restorefolder, null);
                    }
                    else
                    {
                        //System.IO.File.Copy(backupfile, BackupPath + @"\" + SelectedItem, true);
                        ZipUnzip.DecompressDirectory(BackupPath + @"\" + SelectedItem, restorefolder, null);
                    }                    

                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    System.Windows.MessageBox.Show(ex.Message,
                        "Sim.App.Publisher",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Information);

                }
                System.IO.Directory.Delete(downloadfolder, true);
            }
            #endregion

            try
            {
                if (OnNuvem == false)
                    BackupList = Files.ListFiles(BackupPath, "*.*");
                else
                    BackupList = Ftp.GetFilesInFtpDirectory(_nuvemftp);
            }
            catch
            {
                BackupList = null;
            }

        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _inbackup = false;
            _indelete = false;
            _inrestore = false;
            Blackbox = Visibility.Collapsed;
            StartProgressBar = false;
        }

        #endregion

        #region Constructor

        public vmToolsDB()
        {
            Mvvm.Helpers.Observers.GlobalNavigation.Pagina = "BASE DE DADOS";

            Blackbox = Visibility.Collapsed;
            ButtonConnection = "Alterar Connection String";

            try
            {
                BackupPath = XmlFile.Value("sim.apps", "App", "Backup", "Path");
                ConnectionString = XmlFile.Listar("sim.data", "Data", "Conexao", "Rede")[0];

                if (BackupPath == string.Empty)
                {
                    XmlFile.NovoElemento("sim.apps", "Backup", "Path", "...");
                    BackupPath = XmlFile.Value("sim.apps", "App", "Backup", "Path");
                }
            }
            catch
            {
                ConnectionString = string.Empty;
                BackupPath = string.Empty;
            }

            TextboxEnabled = false;

            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);

            StartProgressBar = true;
            OnNuvem = false;

            //bgWorker.RunWorkerAsync();
        }

        #endregion
    }
}
