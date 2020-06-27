using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Sim.Modulos.Empreendedor.ViewModel
{
    using Model;
    using Mvvm.Helpers.Notifiers;
    using Mvvm.Helpers.Commands;
    using Mvvm.Helpers.Observers;
    using Accounts;

    class vmPessoas : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        private mData mdata = new mData();
        private ObservableCollection<mPF_Ext> _listapf = new ObservableCollection<mPF_Ext>();

        private FlowDocument flwdoc = new FlowDocument();

        private bool _starprogress;
        private bool _isenable;

        private string _getcpf = string.Empty;

        private Visibility _blackbox;
        private ICommand _commandgocnpj;
        private ICommand _commandgetcpf;
        #endregion

        #region Properties

        public ObservableCollection<mPF_Ext> ListarPF
        {
            get { return _listapf; }
            set
            {
                _listapf = value;
                RaisePropertyChanged("ListarPF");
            }
        }

        public string GetCPF
        {
            get { return _getcpf; }
            set
            {
                _getcpf = value.Trim();
                RaisePropertyChanged("GetCPF");
            }
        }

        public bool StartProgress
        {
            get { return _starprogress; }
            set
            {
                _starprogress = value;
                RaisePropertyChanged("StartProgress");
            }
        }

        public bool IsEnable
        {
            get { return _isenable; }
            set
            {
                _isenable = value;
                RaisePropertyChanged("IsEnable");
            }
        }

        public Visibility BlackBox
        {
            get { return _blackbox; }
            set
            {
                _blackbox = value;
                RaisePropertyChanged("BlackBox");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandGoCnpj
        {
            get {

                _commandgocnpj = new RelayCommand(p=> {

                    AreaTransferencia.CNPJ = (string)p;
                    ns.Navigate(new Uri("/Sim.Modulo.Empreendedor;component/View/pAddPJ.xaml", UriKind.Relative));                    

                });

                return _commandgocnpj; }
        }

        public ICommand CommandGetCPF
        {
            get
            {

                _commandgetcpf = new RelayCommand(p => {

                    if (GetCPF != string.Empty && new mMascaras().Remove(GetCPF).Length == 11)
                    {
                        AreaTransferencia.CPF = GetCPF;
                        ns.Navigate(new Uri("/Sim.Modulo.Empreendedor;component/View/pAddPF.xaml", UriKind.Relative));
                    }

                });

                return _commandgetcpf;
            }
        }
        #endregion

        #region  Constructor
        public vmPessoas()
        {
            ns = GlobalNavigation.NavService;

            GlobalNavigation.Pagina = "PESSOAS";

            if (AccountOn.Acesso == (int)AccountAccess.Master)
                IsEnable = true;


            if (GlobalNavigation.SubModulo.ToLower() == "Sala do Empreendedor".ToLower())
            {

                foreach (Accounts.Model.mSubModulos m in AccountOn.Submodulos)
                {
                    if (m.SubModulo == (int)SubModulo.SalaEmpreendedor)
                    {
                        if (m.Acesso > (int)SubModuloAccess.Consulta)
                            IsEnable = true;
                    }
                }
            }

            if (GlobalNavigation.SubModulo.ToLower() == "Sebrae Aqui".ToLower())
            {

                foreach (Accounts.Model.mSubModulos m in AccountOn.Submodulos)
                {
                    if (m.SubModulo == (int)SubModulo.SebraeAqui)
                    {
                        if (m.Acesso > (int)SubModuloAccess.Consulta)
                            IsEnable = true;
                    }
                }
            }

            AsyncListarPessoas();
        }


        #endregion

        #region Methods
        #endregion

        #region Function
        private void AsyncListarPessoas()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;

            Task.Factory.StartNew(() => mdata.PessoasNow())
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListarPF = task.Result;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                    else
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion
    }
}
