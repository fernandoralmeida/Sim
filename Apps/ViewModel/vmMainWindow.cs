using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Sim.App.ViewModel
{
    using Sim.Modulos.Accounts.Model;
    using Sim.Modulos.Accounts;
    using Sim.Mvvm.Helpers.Observers;
    using Sim.Mvvm.Helpers.Commands;
    using Sim.Mvvm.Helpers.Notifiers;
    using Sim.Common.Helpers;
    using Model;
    using System.Windows.Shapes;
    using System.Diagnostics;
    using System.Windows.Threading;

    class vmMainWindow : NotifyProperty
    {

        #region Declarations

        //private ContentControl _maincontent = new ContentControl();
        //private Menu _mn = new Menu();
        NavigationService ns;
        private Uri _iconuser;
        private string _title;
        private string _labeltext;
        private Uri _pageselected;
        
        private string _sysname;
        private string _modulo;
        private string _submodulo;
        private string _pagina;
        private string _urimodulo;
        private string _urisubmodulo;

        private double _vw;

        private Visibility _menuon;
        private Visibility _browseback;
        private Visibility _showmsg;
        private Visibility _isadmin;
        private Visibility _menuonoff;

        private ICommand _commandbrowseback;
        private ICommand _commandgopage;        
        
        private ICommand _commandmsgyes;
        private ICommand _commandmsgnot;

        private ICommand _commandnavigate;
        private ICommand _commandlogoff;

        private ICommand _commandmenuonoff;
        #endregion

        #region Properties

        public Uri SelectedPage
        {
            get { return _pageselected; }
            set
            {
                _pageselected = value;
                RaisePropertyChanged("SelectedPage");
            }
        }
        public Uri IconUser
        {
            get { return _iconuser; }
            set
            {
                _iconuser = value;
                RaisePropertyChanged("IconUser");
            }
        }

        public string Operador
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Operador");
            }
        }

        public string LabelText
        {
            get { return _labeltext; }
            set
            {
                _labeltext = value;
                RaisePropertyChanged("LabelText");
            }
        }

        public string SysName
        {
            get { return _sysname; }
            set
            {
                _sysname = value;
                RaisePropertyChanged("SysName");
            }
        }

        public string Modulo
        {
            get { return _modulo; }
            set
            {
                _modulo = value;
                RaisePropertyChanged("Modulo");
            }
        }

        public string SubModulo
        {
            get { return _submodulo; }
            set
            {
                _submodulo = value;
                RaisePropertyChanged("SubModulo");
            }
        }

        public string Pagina
        {
            get { return _pagina; }
            set
            {
                _pagina = value;
                RaisePropertyChanged("Pagina");
            }
        }

        public string UriModulo
        {
            get { return _urimodulo; }
            set
            {
                _urimodulo = value;
                RaisePropertyChanged("UriModulo");
            }
        }

        public string UriSubModulo
        {
            get { return _urisubmodulo; }
            set
            {
                _urisubmodulo = value;
                RaisePropertyChanged("UriSubModulo");
            }
        }

        public Visibility IsAdmin
        {
            get { return _isadmin; }
            set
            {
                _isadmin = value;
                RaisePropertyChanged("IsAdmin");
            }
        }

        public Visibility MenuOn
        {
            get { return _menuon; }
            set
            {
                _menuon = value;
                RaisePropertyChanged("MenuOn");
            }
        }

        public Visibility MenuOnOff
        {
            get
            {
                return _menuonoff;
            }
            set
            {
                _menuonoff = value;
                RaisePropertyChanged("MenuOnOff");
            }
        }

        public Visibility BrowseBack
        {
            get { return _browseback; }
            set
            {
                _browseback = value;
                RaisePropertyChanged("BrowseBack");
            }
        }

        public Visibility ShowMSG
        {
            get { return _showmsg; }
            set
            {
                _showmsg = value;
                RaisePropertyChanged("ShowMSG");
            }
        }

        public double vW
        {
            get { return _vw; }
            set { _vw = value; RaisePropertyChanged("vW"); }
        }

        #endregion

        #region Commands

        public ICommand CommandLogOff
        {
            get {
                if (_commandlogoff == null)
                    _commandlogoff = new RelayCommand(p =>
                    {
                        ShowMSG = Visibility.Visible;
                        System.Media.SystemSounds.Exclamation.Play();
                    });
                
                return _commandlogoff;
            }
        }

        public ICommand CommandBrowseBack
        {
            get
            {
                if (_commandbrowseback == null)
                    _commandbrowseback = new RelayCommand(p=> {
                        if (ns.CanGoBack == true)
                            ns.GoBack();
                    });
                return _commandbrowseback;
            }
        }

        public ICommand CommandMsgYes
        {
            get
            {
                if (_commandmsgyes == null)
                    _commandmsgyes = new RelayCommand(p=>
                    {
                        new AccountOn().LogOut();
                        ShowMSG = Visibility.Collapsed;
                        ns = GlobalNavigation.NavService;
                        ns.RemoveBackEntry();
                        
                    });
                return _commandmsgyes;
            }
        }

        public ICommand CommandMsgNot
        {
            get
            {
                if (_commandmsgnot == null)
                    _commandmsgnot = new RelayCommand(p=>
                    {
                        ShowMSG = Visibility.Collapsed;
                    });
                return _commandmsgnot;
            }
        }

        public ICommand CommandNavigate
        {
            get
            {
                if (_commandnavigate == null)
                    _commandnavigate = new RelayCommand(p => { ns.Navigate(new Uri(p.ToString(), UriKind.Relative)); });
                return _commandnavigate;
            }
        }

        public ICommand CommandGoPage
        {
            get
            {
                _commandgopage = new RelayCommand(p =>
                {
                    if ((p != null) && (AccountOn.Autenticado == true))
                    {
                        ns.Navigate(new Uri(p.ToString(), UriKind.RelativeOrAbsolute));
                    }
                });

                return _commandgopage;
            }
        }
        
        public ICommand CommandOnOff
        {
            get
            {
                if (_commandmenuonoff == null)
                    _commandmenuonoff = new RelayCommand(p =>
                      {

                          if (MenuOnOff == Visibility.Collapsed)
                          {
                              MenuOnOff = Visibility.Visible;
                              vW = 48;
                          }
                          else
                          {
                              MenuOnOff = Visibility.Collapsed;
                              vW = 20;
                          }

                      });

                return _commandmenuonoff;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public vmMainWindow()
        {
            ns = GlobalNavigation.NavService;
            ns.Navigated += Ns_Navigated;
            ShowMSG = Visibility.Collapsed;
            GlobalNotifyProperty.GlobalPropertyChanged += new PropertyChangedEventHandler(OnGlobalPropertyChanged);
            AccountOn.Autenticado = false;            

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Folders.Create(Folders.SimApp, Folders.Pdf);

                string simnewupadate = string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "Sim.App.New.Updater.exe");
                string simoldupadate = string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "Sim.App.Updater.exe");

                if (System.IO.File.Exists(simnewupadate))
                {
                    System.IO.File.Delete(simoldupadate);
                    System.IO.File.Copy(simnewupadate, simoldupadate);
                    System.IO.File.Delete(simnewupadate);
                }

                SysName = "INICIO";

            });

            //NavigationService.Navigate(new Uri("../View/pToolsTheme.xaml", UriKind.Relative));
            //SelectedPage = new Uri("../View/pToolsTheme.xaml", UriKind.Relative);
            //ExecuteUpdate();
            //DebugLogin();
        }

        private void Ns_Navigated(object sender, NavigationEventArgs e)
        {

            //MessageBox.Show(e.Uri.ToString() + "\n" + GoSysName);

            if ((e.Uri.ToString() == "Sim.App.Desktop;component/View/pModulos.xaml") ||
                (e.Uri.ToString() == "Sim.Modulo.Desenvolvimento;component/View/pMainpage.xaml") ||
                (e.Uri.ToString() == "Sim.Modulo.Governo;component/View/pMainpage.xaml"))
            {
                while (ns.CanGoBack)
                    ns.RemoveBackEntry();
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Observa Propriedades Globais
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGlobalPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Autenticado")
            {
                if (AccountOn.Autenticado == false)
                {
                    GlobalNavigation.Reiniciar();
                    MenuOn = Visibility.Collapsed;
                    IsAdmin = Visibility.Collapsed;
                    MenuOnOff = Visibility.Collapsed;
                    vW = 20;
                    ns.Navigate(new Uri("/Sim.Modulo.Autenticacao;component/View/vLogin.xaml", UriKind.Relative));
                    Color paleta = new Color();
                    //paleta = (Color)ColorConverter.ConvertFromString("##00FFFF");
                    paleta = Color.FromRgb(0x1b, 0xa1, 0xe2);
                    new ModernUI.Presentation.ThemeManager().Apply(paleta, "Light");
                }

                else
                {

                    Color paleta = new Color();
                    paleta = (Color)ColorConverter.ConvertFromString(AccountOn.Color);
                    new ModernUI.Presentation.ThemeManager().Apply(paleta, AccountOn.Thema);
                    MenuOn = Visibility.Visible;
                    ns.Navigate(new Uri("/Sim.App.Desktop;component/View/pModulos.xaml", UriKind.Relative));                    
                    if (AccountOn.Acesso >= (int)AccountAccess.Administrador)
                        IsAdmin = Visibility.Visible;     
                                          
                }

                if (AccountOn.Sexo == "F")
                    IconUser = mIcons.IcoWom;

                if (AccountOn.Sexo == "M")
                    IconUser = mIcons.IcoMan;

                Operador = AccountOn.Nome;

            }

            if (e.PropertyName == "Modulo")
                Modulo = GlobalNavigation.Modulo;

            if (e.PropertyName == "UriModulo")
                UriModulo = GlobalNavigation.UriModulo;

            if (e.PropertyName == "UriSubModulo")
                UriSubModulo = GlobalNavigation.UriSubModulo;

            if (e.PropertyName == "SubModulo")
                SubModulo = GlobalNavigation.SubModulo;
            

            if (e.PropertyName == "Pagina")
                Pagina = GlobalNavigation.Pagina;

            if (e.PropertyName == "BrowseBack")
                BrowseBack = GlobalNavigation.BrowseBack;

        }

        #endregion

        #region Functions
        /// <summary>
        /// Execute Sim Update
        /// </summary>
        private void ExecuteUpdate()
        {
            Process[] pname = Process.GetProcessesByName("Sim.App.Updater");

            if (pname.Length == 0)
            {
                ProcessStartInfo pInfo = new ProcessStartInfo("Sim.App.Updater.exe", ":h");
                pInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Process p = Process.Start(pInfo);
            }
        }
        /// <summary>
        /// Login automatico somente para o modo debug
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        private void DebugLogin()
        {
            new mData().AutenticarUsuario("sim.master", "sim.master");
            SysName = "Sim.App.Desktop [MODE:BETA.TESTER]";
        }
        #endregion

    }
}
