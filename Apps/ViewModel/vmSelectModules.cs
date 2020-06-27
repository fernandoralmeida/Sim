using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sim.App.ViewModel
{

    using Model;
    using Modulos.Accounts;
    using Modulos.Accounts.Model;
    using Mvvm.Helpers.Observers;
    using Mvvm.Helpers.Commands;
    using Mvvm.Helpers.Notifiers;
    using System.Diagnostics;
    using System.Windows.Navigation;

    /// <summary>
    /// 
    /// </summary>
    class vmSelectModules : NotifyProperty
    {
        /// <summary>
        /// Declarations
        /// </summary>
        NavigationService ns;
        private ObservableCollection<mButton> _btnm = new ObservableCollection<mButton>();
        private ObservableCollection<mButton> _sysbtnm = new ObservableCollection<mButton>();

        private Menu _mn = new Menu();

        private bool _hasupdate;

        private ICommand _exitcommand;
        private ICommand _logoffcommand;
        private ICommand _accountcommand;
        private ICommand _confcommand;
        private ICommand _commandgoverno;
        private ICommand _commanddesenv;
        private ICommand _commandupdate;

        /// <summary>
        /// Get User Name Autenticado
        /// </summary>
        public string Username
        {
            get { return AccountOn.Nome; }
        }
        /// <summary>
        /// get or set list of buttons
        /// </summary>
        public ObservableCollection<mButton> ButtonsModules
        {
            get { return _btnm; }
            set { _btnm = value; RaisePropertyChanged("ButtonsModules"); }
        }
        /// <summary>
        /// get or set list of buttons
        /// </summary>
        public ObservableCollection<mButton> SysModules
        {
            get { return _sysbtnm; }
            set { _sysbtnm = value; RaisePropertyChanged("SysModules"); }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool HasUpdate
        {
            get { return _hasupdate; }
            set
            {
                _hasupdate = value;
                RaisePropertyChanged("HasUpdate");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Menu mMenu
        {
            get { return _mn; }
            set
            {
                _mn = value;
                RaisePropertyChanged("mMenu");
            }
        }
        /// <summary>
        /// Command Logoff
        /// </summary>
        public ICommand LogoffCommand
        {
            get
            {
                if (_logoffcommand == null)
                {
                    _logoffcommand = new DelegateCommand(ExecuteLogoffCommand, null);
                }
                return _logoffcommand;
            }
        }
        /// <summary>
        /// Command Exit of System
        /// </summary>
        public ICommand ExitCommand
        {
            get
            {
                if (_exitcommand == null)
                {
                    _exitcommand = new DelegateCommand(ExecuteExitCommand, null);
                }
                return _exitcommand;
            }
        }
        /// <summary>
        /// Get Account Command
        /// </summary>
        public ICommand AccountCommand
        {
            get
            {
                if (_confcommand == null)
                    _accountcommand = new DelegateCommand(ExecuteAccountCommand, null);
                
                return _accountcommand;
            }
        }
        /// <summary>
        /// Get Config command
        /// </summary>
        public ICommand ConfigCommand
        {
            get
            {
                if (_confcommand == null)
                    _confcommand = new DelegateCommand(ExecuteConfCommand, null);
                
                return _confcommand;
            }
        }
        /// <summary>
        /// Get Governo Command
        /// </summary>
        public ICommand CommandGoverno
        {
            get
            {
                if (_commandgoverno == null)
                    _commandgoverno = new DelegateCommand(ExecuteCommandGoverno, null);

                return _commandgoverno;
            }
        }


        private void ExecuteCommandGoverno(object obj)
        {

            if (AccountOn.Acesso == (int)AccountAccess.Master)
            {
                ns.Navigate(new Uri("/Sim.Modulo.Governo;component/View/pMainpage.xaml", UriKind.RelativeOrAbsolute));
                GlobalNavigation.UriModulo = "/Sim.Modulo.Governo;component/View/pMainpage.xaml";
            }

            else
            {
                foreach (mModulos m in AccountOn.Modulos)
                {
                    if (m.Modulo == (int)Modulo.Governo && m.Acesso == true)
                    {
                        ns.Navigate(new Uri("/Sim.Modulo.Governo;component/View/pMainpage.xaml", UriKind.RelativeOrAbsolute));
                        GlobalNavigation.UriModulo = "/Sim.Modulo.Governo;component/View/pMainpage.xaml";
                    }
                }
            }
        }

        /// <summary>
        /// Start Update SIM
        /// </summary>
        /// <param name="obj"></param>
        public ICommand CommandUpdate
        {
            get
            {
                if (_commandupdate == null)
                    _commandupdate = new DelegateCommand(ExecuteCommandUpdate, null);
                return _commandupdate;
            }
        }

        private void ExecuteCommandUpdate(object obj)
        {
            StopProcess("Sim.App.Updater");
            ExecuteUpdate();
        }

        public ICommand CommandDesenv
        {
            get
            {
                if (_commanddesenv == null)
                    _commanddesenv = new DelegateCommand(ExecuteCommandDesenv, null);
                
                return _commanddesenv;
            }
        }

        private void ExecuteCommandDesenv(object obj)
        {


            if (AccountOn.Acesso == (int)AccountAccess.Master)
            {
                ns.Navigate(new Uri("/Sim.Modulo.Desenvolvimento;component/View/pMainpage.xaml", UriKind.RelativeOrAbsolute));
                GlobalNavigation.UriModulo = "/Sim.Modulo.Desenvolvimento;component/View/pMainpage.xaml";
            }

            else
            {
                foreach (mModulos m in AccountOn.Modulos)
                {
                    if (m.Modulo == (int)Modulo.Desenvolvimento && m.Acesso == true)
                    {
                        ns.Navigate(new Uri("/Sim.Modulo.Desenvolvimento;component/View/pMainpage.xaml", UriKind.RelativeOrAbsolute));
                        GlobalNavigation.UriModulo = "/Sim.Modulo.Desenvolvimento;component/View/pMainpage.xaml";
                    }
                }
            }            
        }

        /// <summary>
        /// Inicio
        /// </summary>
        public vmSelectModules()
        {
            GlobalNavigation.Reiniciar();
            ns = GlobalNavigation.NavService;
            ns.RemoveBackEntry();

            Task.Factory.StartNew(() => CreatButtons()).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    ButtonsModules = task.Result;
                    //SysModules = task.Result;
                }
            });                 
        }
        /// <summary>
        /// Cria botoes de acesso aos modulos
        /// </summary>
        public ObservableCollection<mButton> CreatButtons()
        {

            //AsyncUpdate();
            ObservableCollection<mButton> _mbtn = new ObservableCollection<mButton>();

            if (System.IO.File.Exists(string.Format(Common.Helpers.Folders.SimApp + @"Sim.Modulo.Governo.dll")))
            {
                var govLauncher = new mButton();
                govLauncher.CommandExecute = this.CommandGoverno;
                govLauncher.Rotulo="GOVERNO";
                _mbtn.Add(govLauncher);
            }

            if (System.IO.File.Exists(string.Format(Sim.Common.Helpers.Folders.SimApp + @"Sim.Modulo.Desenvolvimento.dll")))
            {
                var vrLauncher = new mButton();
                vrLauncher.CommandExecute = this.CommandDesenv;
                vrLauncher.Rotulo = "DESENVOLVIMENTO";
                _mbtn.Add(vrLauncher);
            }

            /*
            for (int i = 0; i < 3; i++)
            {
                var b = new mButton();
                b.Rotulo = i.ToString();
                b.CommandExecute = null;
                _mbtn.Add(b);
            }*/

            return _mbtn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        private void ExecuteAccountCommand(object param)
        {
            //GlobalViewContent.Content = new Modulos.Accounts.View.ucMainContent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        private void ExecuteConfCommand(object param)
        {
            //GlobalViewContent.Content = new View.ucSettings();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        private void ExecuteLogoffCommand(object param)
        {
            if (MessageBox.Show("Mudar Usuário?", "Sim.App.Alerta", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                new AccountOn().LogOut();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        private void ExecuteExitCommand(object param)
        {
            if (MessageBox.Show("Encerrar SIM?", "Sim.App.Alerta", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Execute Sim Update
        /// </summary>
        private void ExecuteUpdate()
        {
            Process[] pname = Process.GetProcessesByName("Sim.App.Updater");

            if (pname.Length == 0)
            {
                ProcessStartInfo pInfo = new ProcessStartInfo("Sim.App.Updater.exe", ":tm");
                pInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Process p = Process.Start(pInfo);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="processname"></param>
        private void StopProcess(string processname)
        {
            Process[] pname = Process.GetProcessesByName(processname);

            if (pname.Length > 0)
                pname[0].CloseMainWindow();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void AsyncUpdate()
        {
            
            string _url = @"ftp://ftp.portalsk.com/portalsk.com/web/sys/sim/install/new/sim_update.xml";

            Task.Factory.StartNew(() => mUpdate.HasUpdate(_url))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        HasUpdate = task.Result;

                    if (HasUpdate)
                    {
                        var upLauncher = new mButton();
                        upLauncher.CommandExecute = this.CommandUpdate;
                        upLauncher.Rotulo = "NOVA VERSÃO DISPONÍVEL!";
                        SysModules.Add(upLauncher);
                    }


                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

    }
}
