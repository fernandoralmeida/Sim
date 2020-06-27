using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace Sim.Modulos.Governo.ViewModel
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
        private MenuItem M_leg = new MenuItem();
        private MenuItem M_pta = new MenuItem();
        private MenuItem M_cof = new MenuItem();
        private MenuItem M_close = new MenuItem();
        private MenuItem M_gov = new MenuItem();

        private ICommand _commandshowport;
        private ICommand _commandshowleg;
        private ICommand _commandclosemod;
        private ICommand _commandconfig;
        private ICommand _commanddenm;

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

        public ICommand CommandShowLeg
        {
            get
            {
                if (_commandshowleg == null)
                    _commandshowleg = new DelegateCommand(ExecuteCommandShowLeg, null);
                return _commandshowleg;
            }
        }

        private void ExecuteCommandShowLeg(object obj)
        {
            GlobalNavigation.UriSubModulo = "/Sim.Modulo.Legislacao;component/View/vMainPage.xaml";
            ns.Navigate(new Uri("/Sim.Modulo.Legislacao;component/View/vMainPage.xaml", UriKind.Relative));
        }

        public ICommand CommandShowPta
        {
            get
            {
                if (_commandshowport == null)
                    _commandshowport = new DelegateCommand(ExecuteCommandShowPta, null);

                return _commandshowport;
            }
        }

        private void ExecuteCommandShowPta(object obj)
        {
            GlobalNavigation.UriSubModulo = "/Sim.Modulo.Portarias;component/View/vMainPage.xaml";
            ns.Navigate(new Uri("/Sim.Modulo.Portarias;component/View/vMainPage.xaml", UriKind.Relative));
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
            GlobalNavigation.UriSubModulo = "/Sim.Modulo.Governo;component/View/pConfig.xaml";
            ns.Navigate(new Uri("/Sim.Modulo.Governo;component/View/pConfig.xaml", UriKind.Relative));
        }

        public ICommand CommandDenm
        {
            get
            {
                if (_commanddenm == null)
                    _commanddenm = new DelegateCommand(ExecuteCommandDenm, null);
                return _commanddenm;
            }
        }

        private void ExecuteCommandDenm(object obj)
        {
            GlobalNavigation.UriSubModulo = "/Sim.Modulo.Denominacoes;component/View/pMaster.xaml";
            ns.Navigate(new Uri("/Sim.Modulo.Denominacoes;component/View/pMaster.xaml", UriKind.Relative));
        }

        public ICommand GoAvaliacoes => new RelayCommand(p => 
        {
            GlobalNavigation.UriSubModulo = "/Sim.Modulo.Administracao;component/View/Avaliacao.xaml";
            ns.Navigate(new Uri("/Sim.Modulo.Administracao;component/View/Avaliacao.xaml", UriKind.RelativeOrAbsolute));
        });

        #endregion

        #region Constructors
        public vmMainContent()
        {
            //this.NewMenus();
            ns = GlobalNavigation.NavService;
            this.CreatButtons();
            GlobalNavigation.Modulo = "GOVERNO";
            GlobalNavigation.SubModulo = string.Empty;
            GlobalNavigation.Pagina = string.Empty;
            GlobalNavigation.BrowseBack = Visibility.Visible;
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

            M_gov.Icon = IC;
            M_gov.Header = "Governo";
            M_gov.ToolTip = "Modulo ativo!";

            M_close.Header = "Fechar Modulo";
            M_close.Command = CommandClose;
            M_gov.Items.Add(M_close);

            M_leg.Header = "Legislação";
            //M_leg.IsCheckable = true;
            M_leg.Command = CommandShowLeg;

            //H1.Items.Add(H11);
            //H1.Items.Add(H12);

            M_pta.Header = "Portarias";
            //M_pta.IsCheckable = true;
            M_pta.Command = CommandShowPta;

            M_cof.Header = "Opções";
            //M_cof.IsCheckable = true;
            M_cof.Command = CommandConfig;

            master.Items.Add(M_gov);
            master.Items.Add(M_leg);
            master.Items.Add(M_pta);
            master.Items.Add(M_cof);

            GlobalMenu.MenuContent = master;
        }

        private void NewMenus()
        {
            Menu master = new Menu();
            HeaderedContentControl IC = new HeaderedContentControl();
            IC.Template = Application.Current.Resources["MenuIcon"] as ControlTemplate;
            var sep = new Separator();

            //M_gov.Icon = IC;            
            M_gov.Header = "GOVERNO";
            M_gov.ToolTip = "Modulo ativo!";

            M_close.Header = "FECHAR";
            M_close.Command = CommandClose;
            M_close.ToolTip = "Fecha o Modulo";

            master.Items.Add(M_gov);
            //master.Items.Add(sep);
            master.Items.Add(M_close);
            GlobalMenu.MenuContent = master;
        }

        public void CreatButtons()
        {

            //if (AccountOn.Submodulos.Where(i => i.SubModulo == (int)SubModulo.Legislacao).ToList().Count == 1 || AccountOn.Acesso ==(int)AccountAccess.Master)
            //{
                var govLauncher = new mButton();
                govLauncher.CommandExecute = this.CommandShowLeg;
                govLauncher.Rotulo = "LEGISLAÇÃO";
                ButtonsModules.Add(govLauncher);
            //}

            //if (AccountOn.Submodulos.Where(i => i.SubModulo == (int)SubModulo.Portarias).ToList().Count == 1 || AccountOn.Acesso == (int)AccountAccess.Master) {
                var vrLauncher = new mButton();
                vrLauncher.CommandExecute = this.CommandShowPta;
                vrLauncher.Rotulo = "PORTARIAS";
                ButtonsModules.Add(vrLauncher);
            //}

            //if (AccountOn.Submodulos.Where(i => i.SubModulo == (int)SubModulo.Denominacoes).ToList().Count == 1 || AccountOn.Acesso == (int)AccountAccess.Master)
            //{
                var denm = new mButton();
                denm.CommandExecute = this.CommandDenm;
                denm.Rotulo = "DENOMINAÇÕES";
                ButtonsModules.Add(denm);
            //}

            var avaliacoes = new mButton();
            avaliacoes.CommandExecute = this.GoAvaliacoes;
            avaliacoes.Rotulo = "AVALIAÇÃO FUNCIONAL";
            ButtonsModules.Add(avaliacoes);

        }
        #endregion
    }
}
