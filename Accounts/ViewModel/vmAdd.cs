using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Threading.Tasks;

namespace Sim.Modulos.Accounts.ViewModel
{

    using Sim.Modulos.Accounts.Model;
    using Sim.Mvvm.Helpers.Observers;
    using Sim.Mvvm.Helpers.Commands;
    using Sim.Mvvm.Helpers.Notifiers;
    using System.Collections.ObjectModel;    

    class vmAdd : NotifyProperty
    {
        #region Declarations
        public NavigationService ns;
        public bool _retorn;
        private mUser _user = new mUser();
        private mOpcoes _opt = new mOpcoes();
        private mContas _conta = new mContas();
        private mModulos _modulo = new mModulos();
        private mSubModulos _submodulo = new mSubModulos();
        private List<mGenerico> _contas = new List<mGenerico>();
        private List<mGenerico> _modulos = new List<mGenerico>();
        private List<mGenerico> _submodulos = new List<mGenerico>();
        private List<mGenerico> _acessosubmodulo = new List<mGenerico>();
        private ObservableCollection<mModuloGenerico> _listamodulos = new ObservableCollection<mModuloGenerico>();
        private ObservableCollection<mModuloGenerico> _listasubmodulos = new ObservableCollection<mModuloGenerico>();
        private mGenero _generos = new mGenero();

        private SolidColorBrush _msgcolor;

        private int _selectedrow;
        private int _rpgrss;

        private string _errormsg;

        private Visibility _showmsg;

        private ICommand _commandaddmd;
        private ICommand _commandaddsmd;
        private ICommand _commandremovemodulo;
        private ICommand _commandremovesubmodulo;
        private ICommand _commandgravar;
        private ICommand _commandreturn;
        #endregion

        #region Properties

        public List<mGenero> Generos
        {
            get { return _generos.Generos(); }
        }

        public mUser User
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChanged("User");
            }
        }

        public mOpcoes Opcoes
        {
            get { return _opt; }
            set
            {
                _opt = value;
                RaisePropertyChanged("Opcoes");
            }
        }

        public mContas Conta
        {
            get { return _conta; }
            set
            {
                _conta = value;
                RaisePropertyChanged("Conta");
            }
        }

        public mModulos Modulo
        {
            get { return _modulo; }
            set
            {
                _modulo = value;
                RaisePropertyChanged("Modulo");
            }
        }

        public mSubModulos SubModulo
        {
            get { return _submodulo; }
            set
            {
                _submodulo = value;
                RaisePropertyChanged("SubModulo");
            }
        }

        public List<mGenerico> Contas
        {
            get { return new mData().TiposContasLista(); }
        }

        public List<mGenerico> Modulos
        {
            get { return new mData().TiposModulosLista(); }
        }

        public List<mGenerico> SubModulos
        {
            get { return new mData().TiposSubModulosLista(); }
        }

        public List<mGenerico> AcessoSubModulos
        {
            get { return new mData().AcessoSubModuloLista(); }
        }

        public ObservableCollection<mModuloGenerico> ListaModulos
        {
            get { return _listamodulos; }
            set
            {
                _listamodulos = value;
                RaisePropertyChanged("ListaModulos");
            }
        }

        public ObservableCollection<mModuloGenerico> ListaSubModulos
        {
            get { return _listasubmodulos; }
            set
            {
                _listasubmodulos = value;
                RaisePropertyChanged("ListaSubModulos");
            }
        }

        public SolidColorBrush MsgColor
        {
            get { return _msgcolor; }
            set
            {
                _msgcolor = value;
                RaisePropertyChanged("MsgColor");
            }
        }

        public int SelectedRow
        {
            get { return _selectedrow; }
            set
            {
                _selectedrow = value;
                RaisePropertyChanged("SelectedRow");
            }
        }

        public int ReportProgress
        {
            get { return _rpgrss; }
            set
            {
                _rpgrss = value;
                RaisePropertyChanged("ReportProgress");
            }
        }

        public string Message
        {
            get { return _errormsg; }
            set
            {
                _errormsg = value;
                RaisePropertyChanged("Message");
            }
        }

        public Visibility MsgBox
        {
            get { return _showmsg; }
            set
            {
                _showmsg = value;
                RaisePropertyChanged("MsgBox");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandAddMD
        {
            get
            {
                if (_commandaddmd == null)
                    _commandaddmd = new RelayCommand(p=> {

                        var findvalue = ListaModulos.Where(i => i.ValorModulo == Modulo.Modulo);

                        if (findvalue.ToList().Count == 0)
                        {
                            ListaModulos.Add(new mModuloGenerico()
                            {
                                Indice = 0,
                                Identificador = User.Identificador,
                                NomeModulo = Modulos[Modulo.Modulo - 1].Nome,
                                ValorModulo = Modulo.Modulo,
                                AcessoModulo = true,
                                AcessoNome = true.ToString(),
                                Ativo = true
                            });
                        }

                    });

                return _commandaddmd;
            }
        }

        public ICommand CommandAddSMD
        {
            get
            {
                if (_commandaddsmd == null)
                    _commandaddsmd = new RelayCommand(p => {

                        var findvalue = ListaSubModulos.Where(i => i.ValorModulo == SubModulo.SubModulo);

                        if (findvalue.ToList().Count == 0)
                            ListaSubModulos.Add(new mModuloGenerico()
                            {
                                Indice = 0,
                                Identificador = User.Identificador,
                                NomeModulo = SubModulos[SubModulo.SubModulo - 1].Nome,
                                ValorModulo = SubModulo.SubModulo,
                                ValorAcesso = SubModulo.Acesso,
                                AcessoNome = AcessoSubModulos[SubModulo.Acesso].Nome,
                                Ativo = true
                            });

                    });

                return _commandaddsmd;
            }
        }

        public ICommand CommandRemoveModulo
        {
            get
            {
                if (_commandremovemodulo == null)
                    _commandremovemodulo = new RelayCommand(p => {

                        try
                        {
                            ListaModulos.RemoveAt(SelectedRow);
                        }
                        catch
                        {
                            SelectedRow = 0;
                        }                        
                                                
                    });

                return _commandremovemodulo;
            }
        }

        public ICommand CommandRemoveSubModulo
        {
            get
            {
                if (_commandremovesubmodulo == null)
                    _commandremovesubmodulo = new RelayCommand(p => {

                        try
                        {
                            ListaSubModulos.RemoveAt(SelectedRow);
                        }
                        catch  { SelectedRow = 0; }                       

                    });

                return _commandremovesubmodulo;
            }
        }

        public ICommand CommandGravar
        {
            get
            {
                if (_commandgravar == null)
                    _commandgravar = new RelayCommand(p => {
                        
                        try
                        {

                            //MessageBox.Show(AccountOn.Acesso + " " + Conta.Conta);

                            if (AccountOn.Acesso > Conta.Conta || AccountOn.Registro.CodigoAcesso.Contains("system_"))
                            {
                                User.Cadastro = DateTime.Now;
                                User.Atualizado = DateTime.Now;
                                User.Ativo = true;

                                if (new mData().GravarUsuario(User))
                                {

                                    Opcoes = new mOpcoes()
                                    {
                                        Identificador = User.Identificador,
                                        Thema = "Light",
                                        Color = "#FF3399FF"
                                    };

                                    Conta = new mContas()
                                    {
                                        Identificador = User.Identificador,
                                        Conta = Conta.Conta,
                                        Ativo = true,
                                    };

                                    foreach (mModuloGenerico md in ListaModulos)
                                    {
                                        Modulo.Identificador = User.Identificador;
                                        Modulo.Modulo = md.ValorModulo;
                                        Modulo.Acesso = md.AcessoModulo;
                                        new mData().GravarModulos(Modulo);
                                    }

                                    foreach (mModuloGenerico smd in ListaSubModulos)
                                    {
                                        SubModulo.Identificador = User.Identificador;
                                        SubModulo.SubModulo = smd.ValorModulo;
                                        SubModulo.Acesso = smd.ValorAcesso;
                                        new mData().GravarSubModulos(SubModulo);
                                    }

                                    if (new mData().GravarOpcoes(Opcoes))

                                        if (new mData().GravarConta(Conta))
                                            _retorn = true;

                                    MsgColor = new SolidColorBrush(Colors.Green);
                                    AsyncMessageBox(string.Format("Usuário {1} ID {0} incluído com sucesso!", User.Identificador, User.Nome));
                                }
                            }
                            else
                            {
                                _retorn = false;
                                MsgColor = new SolidColorBrush(Colors.OrangeRed);
                                AsyncMessageBox(string.Format("Conta com nível de acesso inválido!\nSelecione uma opção inferior à [{0}].", AccountOn.Conta));
                            }

                        }
                        catch(Exception ex)
                        {
                            _retorn = false;
                            MsgColor = new SolidColorBrush(Colors.DarkRed);
                            AsyncMessageBox(ex.Message);
                        }

                    });

                return _commandgravar;
            }
        }

        public ICommand CommandReturn
        {
            get
            {
                if (_commandreturn == null)
                    _commandreturn = new RelayCommand(p => {

                        if (ns.CanGoBack)
                            ns.GoBack();

                    });
                return _commandreturn;
            }
        }
        #endregion

        #region Constructors
        public vmAdd()
        {
            ns = GlobalNavigation.NavService;
            MsgBox = Visibility.Collapsed;
        }
        #endregion

        #region Methods

        #endregion

        #region Functions
        /// <summary>
        /// Exibe MessageBox
        /// </summary>
        /// <param name="message">passa messagem por parametro</param>
        public void AsyncMessageBox(string message)
        {
            Message = message;
            MsgBox = Visibility.Visible;
            System.Media.SystemSounds.Exclamation.Play();
            Task.Factory.StartNew(() => { for (int x = 100; x > 0; x--) { ReportProgress = (100 * x) / 100; System.Threading.Thread.Sleep(5); } })
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        MsgBox = Visibility.Collapsed;
                        if (_retorn)
                            if (ns.CanGoBack)
                                ns.GoBack();
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion
    }
}
