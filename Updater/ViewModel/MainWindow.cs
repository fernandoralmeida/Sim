using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
using System.Security.Permissions;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace Sim.Updater.ViewModel
{

    using Common;
    using Helpers;
    using Model;

    class MainWindow : Notify
    {
        #region Declarations

        private Uri _pagenav;

        private string _titulo;

        private WindowState _winstate = WindowState.Normal;
                
        private ICommand _commandminwin;
        private ICommand _commandclosewin;
        private ICommand _commandrestorewin;

        #endregion

        #region Properties

        public Uri PageNavigate
        {
            get { return _pagenav; }
            set
            {
                _pagenav = value;
                RaisePropertyChanged("PageNavigate");
            }
        }

        public WindowState WinState
        {
            get { return _winstate; }
            set
            {
                _winstate = value;
                RaisePropertyChanged("WinState");
            }
        }

        public string Titulo
        {
            get { return _titulo; }
            set
            {
                _titulo = value;
                RaisePropertyChanged("Titulo");
            }
        }
 
        #endregion

        #region Commands
        public ICommand CommandCloseWin
        {
            get
            {
                if (_commandclosewin == null)
                {
                    _commandclosewin = new DelegateCommand(ExecuteCommandCloseWin, null);
                }
                return _commandclosewin;
            }
        }


        public ICommand CommandMinWin
        {
            get
            {
                if (_commandminwin == null)
                {
                    _commandminwin = new DelegateCommand(ExecuteCommandMin, null);
                }
                return _commandminwin;
            }
        }

        public ICommand CommandRestoreWin
        {
            get
            {
                if (_commandrestorewin == null)
                {
                    _commandrestorewin = new DelegateCommand(ExecuteCommandRestoreWin, null);
                }
                return _commandrestorewin;
            }
        }

        #endregion

        #region Constructor
        public MainWindow()
        {
            if (File.Exists(Files.File_xml_APP) && File.Exists(Files.File_xml_DATA))
            {
                Titulo = "Sim.App.Updater";
                PageNavigate = new Uri(@"/Sim.App.Updater;component/View/pUpdate.xaml", UriKind.RelativeOrAbsolute);
            }
            else
            {
                Titulo = "Sim.App.Installer";
                PageNavigate = new Uri(@"/Sim.App.Updater;component/View/pInstall.xaml", UriKind.RelativeOrAbsolute);
            }

        }


        #endregion

        #region Methods

        private void ExecuteCommandCloseWin(object obj)
        {
            Application.Current.Shutdown();
        }

        private void ExecuteCommandMin(object obj)
        {
            WinState = WindowState.Minimized;
        }

        private void ExecuteCommandRestoreWin(object obj)
        {
            WinState = WindowState.Normal;
        }

        #endregion
    }
}
