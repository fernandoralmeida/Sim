using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace Sim.Modulos.Desenvolvimento.ViewModel
{

    using Mvvm.Helpers.Notifiers;
    using Mvvm.Helpers.Commands;
    using Mvvm.Helpers.Observers;
    using Model;
    using Accounts;

    class vmMainContent : NotifyProperty
    {
        #region Declarations
        private NavigationService ns;
        private ObservableCollection<mButton> _btn = new ObservableCollection<mButton>();
        private MenuItem M_vre = new MenuItem();
        private MenuItem M_sde = new MenuItem();
        private MenuItem M_cof = new MenuItem();
        private MenuItem M_close = new MenuItem();
        private MenuItem M_des = new MenuItem();

        private ICommand _commandshowvre;
        private ICommand _commandshowsde;
        private ICommand _commandclosemod;
        private ICommand _commandconfig;
        private ICommand _commandgocmamb;

        Uri _pageselected;

        #endregion

        #region Properties

        public Uri PageSelected
        {
            get { return _pageselected; }
            set
            {
                _pageselected = value;
                RaisePropertyChanged("PageSelected");
            }
        }

        public ObservableCollection<mButton> ButtonsModules
        {
            get { return _btn; }
            set { _btn = value; RaisePropertyChanged("ButtonsModules"); }
        }

        #endregion

        #region Commands

        public ICommand CommandShowVR
        {
            get
            {
                if (_commandshowvre == null)
                    _commandshowvre = new DelegateCommand(ExecuteCommandShowVR, null);
                return _commandshowvre;
            }
        }

        private void ExecuteCommandShowVR(object obj)
        {
            GlobalNavigation.Parametro = "1";
            GlobalNavigation.SubModulo = "SALA DO EMPREENDEDOR";
            GlobalNavigation.UriSubModulo = "/Sim.Modulo.Empreendedor;component/View/pMainSE.xaml";
            ns.Navigate( new Uri("/Sim.Modulo.Empreendedor;component/View/pMainSE.xaml", UriKind.Relative));            
        }

        public ICommand CommandShowSQ
        {
            get
            {
                if (_commandshowsde == null)
                    _commandshowsde = new DelegateCommand(ExecuteCommandShowSQ, null);

                return _commandshowsde;
            }
        }

        private void ExecuteCommandShowSQ(object obj)
        {
            GlobalNavigation.Parametro = "2";
            GlobalNavigation.SubModulo = "SEBRAE AQUI";
            GlobalNavigation.UriSubModulo = "/Sim.Modulo.Empreendedor;component/View/pMainSA.xaml";
            ns.Navigate(new Uri("/Sim.Modulo.Empreendedor;component/View/pMainSA.xaml", UriKind.Relative));
        }

        public ICommand CommandClose
        {
            get
            {
                if (_commandclosemod == null)
                    _commandclosemod = new DelegateCommand(ExecuteCommandClose, null);

                return _commandclosemod;
            }
        }

        private void ExecuteCommandClose(object obj)
        {
            GlobalViewContent.Clear();
        }

        public ICommand CommandConfig
        {
            get
            {
                if (_commandconfig == null)
                    _commandconfig = new DelegateCommand(ExecuteCommandConfig, null);

                return _commandconfig;
            }
        }

        private void ExecuteCommandConfig(object obj)
        {
            ns.Navigate(new Uri("/Sim.Modulo.Empreendedor;component/View/pOpcoes.xaml", UriKind.Relative));
        }

        public ICommand CommandGoComAmb
        {
            get
            {
                if (_commandgocmamb == null)
                    _commandgocmamb = new RelayCommand(p => {

                        GlobalNavigation.Parametro = "3";
                        GlobalNavigation.SubModulo = "COMÉRCIO AMBULANTE";
                        GlobalNavigation.UriSubModulo = "/Sim.Modulo.cAmbulante;component/View/pMain.xaml";
                        ns.Navigate(new Uri("/Sim.Modulo.cAmbulante;component/View/pMain.xaml", UriKind.Relative));

                    });

                return _commandgocmamb;
            }
        }

        #endregion

        #region Constructors
        public vmMainContent()
        {
            //this.NewMenus();
            ns = GlobalNavigation.NavService;
            this.CreatButtons();
            GlobalNavigation.Pagina = string.Empty;
            GlobalNavigation.Modulo = "DESENVOLVIMENTO";
            GlobalNavigation.SubModulo = string.Empty;
        }
        #endregion

        #region Methods

        #endregion

        #region Functions

        private void ShowMenus()
        {
            Menu master = new Menu();
            //master.FontSize = 10;
            //desenha icone
            HeaderedContentControl IC = new HeaderedContentControl();
            IC.Template = Application.Current.Resources["MenuIcon"] as ControlTemplate;

            //M_des.Icon = IC;
            M_des.Header = "Desenvolvimento";
            M_des.ToolTip = "Modulo ativo!";

            M_close.Header = "Fechar Modulo";
            M_close.Command = CommandClose;
            M_des.Items.Add(M_close);

            M_vre.Header = "Sala Empreendedor";
            //M_vre.IsCheckable = true;
            M_vre.Command = CommandShowVR;

            //H1.Items.Add(H11);
            //H1.Items.Add(H12);

            M_sde.Header = "Sebrae Aqui";
            //M_sde.IsCheckable = true;
            M_sde.Command = CommandShowSQ;

            M_cof.Header = "Opções";
            //M_cof.IsCheckable = true;
            M_cof.Command = CommandConfig;

            master.Items.Add(M_des);
            master.Items.Add(M_vre);
            master.Items.Add(M_sde);
            master.Items.Add(M_cof);

            GlobalMenu.MenuContent = master;
        }

        private void NewMenus()
        {
            Menu master = new Menu();
            HeaderedContentControl IC = new HeaderedContentControl();
            IC.Template = Application.Current.Resources["MenuIcon"] as ControlTemplate;
            var sep = new Separator();

            //M_des.Icon = IC;
            M_des.Header = "DESENVOLVIMENTO";
            M_des.ToolTip = "Modulo ativo!";

            M_close.Header = "FECHAR";
            M_close.Command = CommandClose;
            M_close.ToolTip = "Fecha o Modulo";
            //M_des.Items.Add(M_close);

            master.Items.Add(M_des);
            master.Items.Add(M_close);
            GlobalMenu.MenuContent = master;
        }

        public void CreatButtons()
        {
            if (AccountOn.Submodulos.Where(i => i.SubModulo == (int)SubModulo.SebraeAqui).ToList().Count == 1 || AccountOn.Acesso == (int)AccountAccess.Master)
            {

                ButtonsModules.Add(new mButton()
                {
                    CommandExecute = this.CommandShowSQ,
                    Rotulo = "SEBRAE AQUI"
                });
            }

            if (AccountOn.Submodulos.Where(i => i.SubModulo == (int)SubModulo.SalaEmpreendedor).ToList().Count == 1 || AccountOn.Acesso == (int)AccountAccess.Master)
            {

                ButtonsModules.Add(new mButton()
                {
                    CommandExecute = this.CommandShowVR,
                    Rotulo = "SALA DO EMPREENDEDOR"
                });
            }

            if (AccountOn.Submodulos.Where(i => i.SubModulo == (int)SubModulo.ComercioAmbulante).ToList().Count == 1 || AccountOn.Acesso == (int)AccountAccess.Master)
            {

                ButtonsModules.Add(new mButton()
                {
                    CommandExecute = this.CommandGoComAmb,
                    Rotulo = "COMÉRCIO AMBULANTE"
                });
            }

            //var opc = new mButton();
            //opc.CommandExecute = this.CommandConfig;
            //opc.Rotulo = "OPÇÕES";
            //ButtonsModules.Add(opc);
        }

        #endregion
    }
}
