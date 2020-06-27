using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
using System.IO;
using System.Diagnostics;

namespace Sim.Updater.ViewModel
{
    using Helpers;
    using Common;
    using Model;
    using System.Threading.Tasks;

    class vmUpdate : Notify
    {
        #region Declarations
        
        private DispatcherTimer d_timer = new DispatcherTimer();
        private BackgroundWorker bgWorker = new BackgroundWorker();

        private bool _hasupdate = false;
        private bool _hasdownload = false;
        private bool _fileexist = false;

        private string _textupdatemessage;
        private string _textbuttonupdate;
        private string _textprogress;
        private string _fulltextprogress;

        private bool _buttonenabled = true;

        private int _reportprogress;
        
        private ICommand _commandstart;
        //private ICommand _commandnewstart;

        #endregion

        #region Properties

        public int ReportProgress
        {
            get { return _reportprogress; }
            set
            {
                if (_reportprogress != value)
                {
                    _reportprogress = value;
                    RaisePropertyChanged("ReportProgress");
                }
            }
        }

        public string TextProgress
        {
            get { return _textprogress; }
            set
            {
                _textprogress = value;
                RaisePropertyChanged("TextProgress");
            }
        }

        public string FullTextProgress
        {
            get { return _fulltextprogress; }
            set
            {
                _fulltextprogress = value;
                RaisePropertyChanged("FullTextProgress");
            }
        }

        public string TextButtonUpdate
        {
            get { return _textbuttonupdate; }
            set
            {
                _textbuttonupdate = value;
                RaisePropertyChanged("TextButtonUpdate");
            }
        }

        public string TextUpdateMessage
        {
            get { return _textupdatemessage; }
            set
            {
                _textupdatemessage = value;
                RaisePropertyChanged("TextUpdateMessage");
            }
        }

        public bool ButtonEnabled
        {
            get { return _buttonenabled; }
            set
            {
                _buttonenabled = value;
                RaisePropertyChanged("ButtonEnabled");
            }
        }

        #endregion

        #region Commands
        public ICommand CommandStart
        {
            get
            {
                if (_commandstart == null)
                {
                    _commandstart = new DelegateCommand(ExecuteCommandStart, null);
                }
                return _commandstart;
            }
        }

        #endregion

        #region Constructor
        public vmUpdate()
        {
            d_timer.Tick += D_timer_Tick;
            d_timer.Interval = new TimeSpan(1, 0, 0);
            d_timer.Start();

            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);

            TextButtonUpdate = "Verificar";
            TextUpdateMessage = "O SIM etá atualizado!";
            TextProgress = "";
            FullTextProgress = "";
            ReportProgress = 0;

            ExecuteCommandStart(null);
        }


        #endregion

        #region Methods

        private void D_timer_Tick(object sender, EventArgs e)
        {
            if (File.Exists(@"sim_update.gz"))
            {
                _fileexist = true;
                d_timer.Stop();
            }
            else
            {
                _fileexist = false;
                bgWorker.RunWorkerAsync();
            }
        }

        private void ExecuteCommandStart(object param)
        {
            if (TextButtonUpdate != "Iniciar SIM")
            {
                if (File.Exists(@"sim_update.gz"))
                {
                    _fileexist = true;
                    d_timer.Stop();
                    bgWorker.RunWorkerAsync();
                }
                else
                {
                    _fileexist = false;

                    if (!_hasupdate)
                        bgWorker.RunWorkerAsync();

                    else if (_hasdownload)
                        bgWorker.RunWorkerAsync();
                }
            }
            else if (TextButtonUpdate == "Iniciar SIM")
                ExecuteProcess();
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_fileexist)
            {
                ButtonEnabled = false;
                FullTextProgress = "Verificando";

                Thread.Sleep(2000);
                StopProcess();

                Thread.Sleep(2000);
                FullTextProgress = "";
                TextUpdateMessage = "Preparando atualização, aguarde!";
                ReportProgress = 0;

                Thread.Sleep(2000);
                mDir.NewClearDir(Folders.AppData_Sim);

                Thread.Sleep(2000);
                TextProgress = "Atualizando";
                TextUpdateMessage = "Atualizando o SIM, aguarde!";
                mUnzip.ToDirectory("sim_update.gz", Folders.AppData_Sim, bgWorker);

                Thread.Sleep(2000);
                mShortCut.Createlnk("Sim.App.Desktop", Directories.Desktop, Folders.AppData_Sim + Files.Sim_Exe);
                File.Copy(Files.File_Update_Xml_FullName_Temp, Folders.AppData_Sim + Files.Sim_Update_Xml, true);
                _fileexist = false;
                System.Media.SystemSounds.Exclamation.Play();

                FullTextProgress = "";
            }
            else
            {

                if (!_hasupdate)
                {
                    ButtonEnabled = false;
                    FullTextProgress = "Verificando";
                    _hasdownload = mUpdate.HasUpdate(Files.File_Update_Xml_FullName);
                    _hasupdate = _hasdownload;

                    if (_hasdownload)
                        d_timer.Stop();

                    FullTextProgress = "";
                }
                else
                {
                    TextProgress = "Baixando";
                    ButtonEnabled = false;
                    TextUpdateMessage = "Baixando atualização, aguarde!";
                    new mFTP().Download(Files.File_Update_FullName, Files.File_Update_FullName_Temp, bgWorker);

                    Thread.Sleep(2000);
                    StopProcess();

                    Thread.Sleep(2000);
                    FullTextProgress = "";
                    TextUpdateMessage = "Preparando atualização, aguarde!";
                    ReportProgress = 0;

                    Thread.Sleep(2000);
                    mDir.NewClearDir(Folders.AppData_Sim);

                    Thread.Sleep(2000);
                    TextProgress = "Atualizando";
                    TextUpdateMessage = "Atualizando o SIM, aguarde!";
                    mUnzip.ToDirectory(Files.File_Update_FullName_Temp, Folders.AppData_Sim, bgWorker);

                    Thread.Sleep(2000);
                    mShortCut.Createlnk("Sim.App.Desktop", Directories.Desktop, Folders.AppData_Sim + Files.Sim_Exe);
                    File.Copy(Files.File_Update_Xml_FullName_Temp, Folders.AppData_Sim + Files.Sim_Update_Xml, true);
                    _hasdownload = false;
                    System.Media.SystemSounds.Exclamation.Play();
                }
            }
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ReportProgress = e.ProgressPercentage;
            FullTextProgress = string.Format("{0} {1}%", TextProgress, e.ProgressPercentage);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Thread.Sleep(500);

            if (_hasdownload)
            {
                TextUpdateMessage = "Atualização disponível para o SIM!";
                TextButtonUpdate = "Atualizar";
                ButtonEnabled = true;
            }
            else
            {
                TextButtonUpdate = "Iniciar SIM";
                TextUpdateMessage = "O SIM etá atualizado!";
                _hasupdate = false;
                ButtonEnabled = true;
                FullTextProgress = "";
                ReportProgress = 0;
            }
        }

        #endregion

        #region Functions

        private void ExecuteProcess()
        {
            Process[] pname = Process.GetProcessesByName("Sim.App.Desktop");

            if (pname.Length == 0)
            {
                Application.Current.MainWindow.Close();
                ProcessStartInfo pInfo = new ProcessStartInfo(Folders.AppData_Sim + "Sim.App.Desktop.exe");
                pInfo.WorkingDirectory = Folders.AppData_Sim;
                Process p = Process.Start(pInfo);
            }
        }

        private void StopProcess()
        {
            Process[] pname = Process.GetProcessesByName("Sim.App.Desktop");

            if (pname.Length > 0)
                pname[0].CloseMainWindow();
        }

        #endregion
    }
}
