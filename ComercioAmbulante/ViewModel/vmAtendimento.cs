using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Sim.Modulos.ComercioAmbulante.ViewModel
{
    using Mvvm.Helpers.Notifiers;
    using Mvvm.Helpers.Commands;
    using Mvvm.Helpers.Observers;
    using Empreendedor.Model;
    using Accounts;
    using Model;
    using System.Windows.Media;

    class vmAtendimento : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        private mData mdata = new mData();
        private mAtendimento _atendimento = new mAtendimento();
        private ObservableCollection<mTiposGenericos> _origemat = new ObservableCollection<mTiposGenericos>();
        private ObservableCollection<mTiposGenericos> _servicos = new ObservableCollection<mTiposGenericos>();
        private ObservableCollection<mCliente> _pf = new ObservableCollection<mCliente>();
        private ObservableCollection<mCliente> _lpj = new ObservableCollection<mCliente>();
        private ObservableCollection<mCliente> _pj = new ObservableCollection<mCliente>();

        private ObservableCollection<string> _servicosrealizados = new ObservableCollection<string>();

        private string _protocolo = string.Empty;
        private string _servicoselecionado = string.Empty;
        private string _servicoremovido = string.Empty;

        private Visibility _cabecalho;
        private Visibility _corpo;
        private Visibility _viewlistapj;

        private Visibility _blackbox;

        private bool _startprogress;

        private ICommand _commandpesquisarinscricao;
        private ICommand _commandviabilidade;
        private ICommand _commandsave;
        private ICommand _commandcancel;
        private ICommand _commandcancelcliente;
        private ICommand _commandalterar;
        private ICommand _commandselectedcnpj;
        #endregion

        #region Properties
        
        public ObservableCollection<mTiposGenericos> OrigemAtendimento
        {
            get { return _origemat; }
            set
            {
                _origemat = value;
                RaisePropertyChanged("OrigemAtendimento");
            }
        }

        public ObservableCollection<mTiposGenericos> Servicos
        {
            get
            {                              
                return _servicos;
            }
            set
            {
                _servicos = value;
                RaisePropertyChanged("Servicos");
            }
        }

        public ObservableCollection<string> ServicosRealizados
        {
            get { return _servicosrealizados; }
            set
            {
                _servicosrealizados = value;
                RaisePropertyChanged("ServicosRealizados");
            }
        }

        public string ServicoSelecionado
        {
            get { return _servicoselecionado; }
            set
            {
                _servicoselecionado = value;

                if (_servicoselecionado != string.Empty && _servicoselecionado != null && _servicoselecionado != "...")
                {
                    if (!ServicosRealizados.Any(l => l == _servicoselecionado) || ServicosRealizados.Any(l => l == "INSCRIÇÃO"))
                    {

                        Atendimento.Tipo = 0;

                        switch (_servicoselecionado)
                        {
                            case "INSCRIÇÃO":
                                Atendimento.Tipo = 3;
                                break;

                            case "CADASTRO DE COMÉRCIO AMBULANTE":
                                Atendimento.Tipo = 12;
                                break;
                        }

                        ExecuteCommandViabilidade(null);

                        ServicosRealizados.Add(_servicoselecionado);
                    }
                }

                RaisePropertyChanged("ServicoSelecionado");
            }
        }

        public string ServicoRemovido
        {
            get { return _servicoremovido; }
            set
            {
                _servicoremovido = value;

                if (_servicoremovido != string.Empty)
                {
                    if (ServicosRealizados.Any(l => l == _servicoremovido))
                    {
                        ServicosRealizados.Remove(_servicoremovido);
                        ServicoSelecionado = "...";
                    }
                }

                RaisePropertyChanged("ServicoRemovido");
            }
        }

        public mAtendimento Atendimento
        {
            get { return _atendimento; }
            set
            {
                _atendimento = value;
                RaisePropertyChanged("Atendimento");
            }
        }

        public ObservableCollection<mCliente> PF
        {
            get { return _pf; }
            set
            {
                _pf = value;
                RaisePropertyChanged("PF");
            }
        }

        public ObservableCollection<mCliente> ListaPJ
        {
            get { return _lpj; }
            set
            {
                _lpj = value;
                RaisePropertyChanged("ListaPJ");
            }
        }

        public ObservableCollection<mCliente> PJ
        {
            get { return _pj; }
            set
            {
                _pj = value;
                RaisePropertyChanged("PJ");
            }
        }

        public string Protocolo
        {
            get { return _protocolo; }
            set
            {
                _protocolo = value;
                RaisePropertyChanged("Protocolo");
            }
        }

        public string PageName
        {
            get { return GlobalNavigation.Pagina; }
        }

        public Visibility Cabecalho
        {
            get { return _cabecalho; }
            set
            {
                _cabecalho = value;
                RaisePropertyChanged("Cabecalho");
            }
        }

        public Visibility Corpo
        {
            get { return _corpo; }
            set
            {
                _corpo = value;
                RaisePropertyChanged("Corpo");
            }
        }

        public Visibility ViewListaPJ
        {
            get { return _viewlistapj; }
            set
            {
                _viewlistapj = value;
                RaisePropertyChanged("ViewListaPJ");
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

        public bool StarProgress
        {
            get { return _startprogress; }
            set
            {
                _startprogress = value;
                RaisePropertyChanged("StarProgress");
            }
        }

        #endregion

        #region Commands
        public ICommand CommandPesquisarInscricao
        {
            get
            {
                if (_commandpesquisarinscricao == null)
                    _commandpesquisarinscricao = new DelegateCommand(ExecuteCommandPesquisarInscricao, null);
                return _commandpesquisarinscricao;
            }
        }

        private void ExecuteCommandPesquisarInscricao(object obj)
        {
            try
            {
                
                if (Atendimento.Cliente.Inscricao == string.Empty)
                    return;

                string identificador = new mMascaras().Remove(Atendimento.Cliente.Inscricao);

                PF.Clear();
                PJ.Clear();

                switch (identificador.Length)
                {
                    case 11:
                        ClientePF(new mData().ExistPessoaFisica(identificador));
                        break;

                    case 14:
                        ClientePJ(new mData().ExistPessoaJuridica(identificador));
                        break;
                }
                
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }

        public ICommand CommandAlterar
        {
            get
            {
                if (_commandalterar == null)
                    _commandalterar = new RelayCommand(p=> {

                        try
                        {
                            if (Atendimento.Cliente.Inscricao == string.Empty)
                                return;

                            string identificador = new mMascaras().Remove((string)p);

                            //Cliente.Clear();
                            //PJ.Clear();
                            //PF.Clear();

                            switch (identificador.Length)
                            {
                                case 11:
                                    ns.Navigate(new Uri("/Sim.Modulo.Empreendedor;component/View/pAddPF.xaml", UriKind.Relative));
                                    AreaTransferencia.CPF = (string)p;// Atendimento.Cliente.Inscricao;
                                    AreaTransferencia.CadPF = true;
                                    break;

                                case 14:
                                    ns.Navigate(new Uri("/Sim.Modulo.Empreendedor;component/View/pAddPJ.xaml", UriKind.Relative));
                                    AreaTransferencia.CNPJ = (string)p; // Atendimento.Cliente.Inscricao;
                                    AreaTransferencia.CadPJ = true;
                                    break;
                            }
                        }
                        catch (Exception ex)
                        { MessageBox.Show(ex.Message); }

                    });
                return _commandalterar;
            }
        }

        public ICommand CommandViabilidade
        {
            get
            {
                if (_commandviabilidade == null)
                    _commandviabilidade = new DelegateCommand(ExecuteCommandViabilidade, null);
                return _commandviabilidade;
            }
        }

        private void ExecuteCommandViabilidade(object obj)
        {           
            
            switch (Atendimento.Tipo)
            {

                case 3:
                    AreaTransferencia.InscricaoOK = false;
                    AreaTransferencia.CPF = Atendimento.Cliente.Inscricao;
                    ns.Navigate(new Uri("/Sim.Modulo.Empreendedor;component/View/Inscricoes/pAdd.xaml", UriKind.Relative));
                    break;

                case 12:
                    AreaTransferencia.CadAmbulanteOK = false;
                    AreaTransferencia.CPF = new mMascaras().Remove(Atendimento.Cliente.Inscricao).TrimEnd();

                    if (PJ.Count > 0)
                        AreaTransferencia.CNPJ = PJ[0].Inscricao;
                    else
                        AreaTransferencia.CNPJ = string.Empty;

                    ns.Navigate(new Uri("/Sim.Modulo.cAmbulante;component/View/pCAmbulante.xaml", UriKind.Relative));
                    break;
            }
            
        }

        public ICommand CommandSave
        {
            get
            {
                if (_commandsave == null)
                    _commandsave = new DelegateCommand(ExecuteCommandSave, null);
                return _commandsave;
            }
        }

        private void ExecuteCommandSave(object obj)
        {
            Gravar();
        }

        public ICommand CommandCancel
        {
            get
            {
                if (_commandcancel == null)
                    _commandcancel = new DelegateCommand(ExecuteCommandCancel, null);
                return _commandcancel;
            }
        }

        private void ExecuteCommandCancel(object obj)
        {
            if (MessageBox.Show(string.Format("Cancelar Atendimento {0}?", Protocolo), "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                if (ns.CanGoBack)
                    ns.GoBack();
            }
        }

        public ICommand CommandCancelCliente
        {
            get
            {
                if (_commandcancelcliente == null)
                    _commandcancelcliente = new DelegateCommand(ExecuteCommandCancelCliente, null);
                return _commandcancelcliente;
            }
        }

        private void ExecuteCommandCancelCliente(object obj)
        {
            
            if (MessageBox.Show(string.Format("Cancelar Atendimento do Cliente  {0}?", Atendimento.Cliente.NomeRazao), "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                Atendimento.Clear();
                PJ.Clear();
                PF.Clear();
                Atendimento.Operador = AccountOn.Identificador;
                AreaTransferencia.Limpar();
                Corpo = Visibility.Collapsed;
                Cabecalho = Visibility.Visible;
            }
            
        }

        public ICommand CommandSelectedCNPJ
        {
            get
            {
                
                if (_commandselectedcnpj == null)
                    _commandselectedcnpj = new RelayCommand(p =>
                    {

                        Task<mPJ_Ext>.Factory.StartNew(() => mdata.ExistPessoaJuridica((string)p)).ContinueWith(task_two =>
                        {
                            if (task_two.IsCompleted)
                            {
                                PJ.Add(new mCliente()
                                {
                                    Inscricao = task_two.Result.CNPJ,
                                    NomeRazao = task_two.Result.RazaoSocial,
                                    Telefones = task_two.Result.Telefones,
                                    Email = task_two.Result.Email
                                });
                                ViewListaPJ = Visibility.Collapsed;
                            }
                        },
                        System.Threading.CancellationToken.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.FromCurrentSynchronizationContext());
                    });
                    

                return _commandselectedcnpj;
            }
        }
        #endregion

        #region Constructor
        public vmAtendimento()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "NOVO ATENDIMENTO";
            GlobalNotifyProperty.GlobalPropertyChanged += GlobalNotifyProperty_GlobalPropertyChanged;
            GlobalNavigation.BrowseBack = Visibility.Collapsed;
            Protocolo = NProtocolo();
            Atendimento.Data = DateTime.Now;
            Atendimento.Operador = AccountOn.Identificador;
            Atendimento.Ativo = true;
            StarProgress = false;
            Cabecalho = Visibility.Visible;
            BlackBox = Visibility.Collapsed;
            Corpo = Visibility.Collapsed;
            ViewListaPJ = Visibility.Collapsed;
            AsyncOrigems();
            AsyncServicos();
        }

        #endregion

        #region Methods
        private void GlobalNotifyProperty_GlobalPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CadAmbulanteOK")
                if (AreaTransferencia.CadAmbulanteOK == true)
                {
                    if (Atendimento.Historico.Length > 0)
                        Atendimento.Historico = string.Concat(Atendimento.Historico, string.Format("\nCOMERCIANTE DE RUA CADASTRADO, PROTOCOLO Nº {0}", AreaTransferencia.CadastroAmbulante));

                    else
                        Atendimento.Historico = string.Format(@"COMERCIANTE DE RUA CADASTRADO, PROTOCOLO Nº {0}", AreaTransferencia.CadastroAmbulante);
                }

            if (e.PropertyName == "CadPF")
                if (AreaTransferencia.CadPF == false)
                    CommandPesquisarInscricao.Execute(null);

            if (e.PropertyName == "CadPJ")
                if (AreaTransferencia.CadPJ == false)
                    CommandPesquisarInscricao.Execute(null);


            if (e.PropertyName == "InscricaoOK")
                if (AreaTransferencia.InscricaoOK == true)
                {

                    mAgenda ag = new mAgenda();

                    Task.Factory.StartNew(() => new mData().Evento(AreaTransferencia.Evento))
                        .ContinueWith(task =>
                        {
                            if (task.IsCompleted)
                            {
                                ag.Clear();
                                ag.Indice = task.Result.Indice;
                                ag.Codigo = task.Result.Codigo;
                                ag.Evento = task.Result.Evento;
                                ag.Tipo = task.Result.Tipo;
                                ag.TipoString = task.Result.TipoString;
                                ag.Descricao = task.Result.Descricao;
                                ag.Vagas = task.Result.Vagas;
                                ag.Data = task.Result.Data;
                                ag.Hora = task.Result.Hora;
                                ag.Setor = task.Result.Setor;
                                ag.Estado = task.Result.Estado;
                                ag.Criacao = task.Result.Criacao;
                                ag.Ativo = task.Result.Ativo;

                                if (Atendimento.Historico.Length > 0)
                                    Atendimento.Historico = string.Concat(Atendimento.Historico, string.Format("\nINSCRIÇÃO {0} NA {1} {2} {3} {4}", AreaTransferencia.Inscricao[AreaTransferencia.Inscricao.Count - 1], ag.TipoString, ag.Evento, ag.Data.ToShortDateString(), ag.Hora.ToShortTimeString()));

                                else
                                    Atendimento.Historico = string.Format(@"INSCRIÇÃO {0} NA {1} {2} {3} {4}", AreaTransferencia.Inscricao[AreaTransferencia.Inscricao.Count - 1], ag.TipoString, ag.Evento, ag.Data.ToShortDateString(), ag.Hora.ToShortTimeString());

                            }
                        },
                        System.Threading.CancellationToken.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.FromCurrentSynchronizationContext());
                }               

        }
        #endregion

        #region Functions
        private string NProtocolo()
        {
            string r = string.Empty;

            string a = DateTime.Now.Year.ToString("0000");
            string m = DateTime.Now.Month.ToString("00");
            string d = DateTime.Now.Day.ToString("00");

            string hs = DateTime.Now.Hour.ToString("00");
            string mn = DateTime.Now.Minute.ToString("00");
            string ss = DateTime.Now.Second.ToString("00");

            r = string.Format(@"AT{0}{1}{2}{3}{4}{5}",
                a, m, d, hs, mn, ss);

            return r;
        }
                
        private void ClientePF(mPF_Ext obj)
        {
            try
            {

                string identificador = new mMascaras().Remove(Atendimento.Cliente.Inscricao);


                if (obj == null)
                {
                    if (MessageBox.Show("CLIENTE " + Atendimento.Cliente.Inscricao + " não encontrado! Cadastrá-lo agora?", "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        ns.Navigate(new Uri("/Sim.Modulo.Empreendedor;component/View/pAddPF.xaml", UriKind.Relative));
                        AreaTransferencia.CPF = Atendimento.Cliente.Inscricao;
                        AreaTransferencia.CadPF = true;
                    }

                    return;
                }

                AsyncMostrarCliente(obj.CPF);

                Atendimento.Cliente.Inscricao = new mMascaras().CPF(obj.CPF);
                Atendimento.Cliente.NomeRazao = obj.Nome;
                Atendimento.Cliente.Telefones = obj.Telefones;
                Atendimento.Cliente.Email = obj.Email;

                Corpo = Visibility.Visible;
                Cabecalho = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClientePJ(mPJ_Ext obj)
        {
            try
            {

                string identificador = new mMascaras().Remove(Atendimento.Cliente.Inscricao);


                if (obj == null)
                {
                    if (MessageBox.Show("CLIENTE " + Atendimento.Cliente.Inscricao + " não encontrado! Cadastrá-lo agora?", "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        ns.Navigate(new Uri("/Sim.Modulo.Empreendedor;component/View/pAddPJ.xaml", UriKind.Relative));
                        AreaTransferencia.CNPJ = Atendimento.Cliente.Inscricao;
                        AreaTransferencia.CadPJ = true;
                    }

                    return;
                }

                AsyncPJ(obj.CNPJ);

                Atendimento.Cliente.Inscricao = new mMascaras().CNPJ(obj.CNPJ);
                Atendimento.Cliente.NomeRazao = obj.RazaoSocial;
                Atendimento.Cliente.Telefones = obj.Telefones;
                Atendimento.Cliente.Email = obj.Email;

                Corpo = Visibility.Visible;
                Cabecalho = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AsyncOrigems()
        {
            Task.Factory.StartNew(() => new mData().Tipos(@"SELECT * FROM SDT_Atendimento_Origem WHERE (Ativo = True) ORDER BY Origem"))
                .ContinueWith(t => {
                    if (t.IsCompleted)
                    {
                        OrigemAtendimento = t.Result;
                        Atendimento.Origem = 2;
                        if (GlobalNavigation.Parametro == "1")
                        {
                            Atendimento.Origem = 1;
                        }
                    }
                });
        }

        private void AsyncServicos()
        {
            Task.Factory.StartNew(() => new mData().Tipos(@"SELECT * FROM SDT_Atendimento_Tipos WHERE ((Valor = 2) OR (Valor = 3) OR (Valor = 12)) AND (Ativo = True) ORDER BY Tipo")).ContinueWith(t => { if (t.IsCompleted) Servicos = t.Result; });
        }
        
        private void AsyncMostrarCliente(string cpf)
        {
            Task<mPF_Ext>.Factory.StartNew(() => mdata.ExistPessoaFisica(cpf))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        PF.Clear();
                        PF.Add(new mCliente()
                        {
                            Inscricao = task.Result.CPF,
                            NomeRazao = task.Result.Nome,
                            Telefones = task.Result.Telefones,
                            Email = task.Result.Email
                        });
                        ListaPJ.Clear();
                        foreach (mVinculos v in task.Result.ColecaoVinculos)
                        {
                            Task<mPJ_Ext>.Factory.StartNew(() => mdata.ExistPessoaJuridica(v.CNPJ)).ContinueWith(task_two =>
                            {
                                if (task_two.IsCompleted)
                                {

                                    if (task.Result.ColecaoVinculos.Count > 1)
                                    {
                                        ListaPJ.Add(new mCliente()
                                        {
                                            Inscricao = task_two.Result.CNPJ,
                                            NomeRazao = task_two.Result.RazaoSocial,
                                            Telefones = task_two.Result.Telefones,
                                            Email = task_two.Result.Email
                                        });
                                        ViewListaPJ = Visibility.Visible;
                                    }
                                    else
                                    {
                                        PJ.Add(new mCliente()
                                        {
                                            Inscricao = task_two.Result.CNPJ,
                                            NomeRazao = task_two.Result.RazaoSocial,
                                            Telefones = task_two.Result.Telefones,
                                            Email = task_two.Result.Email
                                        });
                                    }
                                }
                            },
                            System.Threading.CancellationToken.None,
                            TaskContinuationOptions.None,
                            TaskScheduler.FromCurrentSynchronizationContext());
                        }

                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncPJ(string cnpj)
        {
            Task<mPJ_Ext>.Factory.StartNew(() => mdata.ExistPessoaJuridica(cnpj)).ContinueWith(task_two =>
            {
                if (task_two.IsCompleted)
                {
                    PJ.Clear();
                    PJ.Add(new mCliente()
                    {
                        Inscricao = task_two.Result.CNPJ,
                        NomeRazao = task_two.Result.RazaoSocial,
                        Telefones = task_two.Result.Telefones,
                        Email = task_two.Result.Email
                    });
                }
            },
            System.Threading.CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncGravarAmbulante()
        {
            Task.Factory.StartNew(() => mdata.GravarAmbulanteAtendimento(AreaTransferencia.CadastroAmbulante, Atendimento.Protocolo)).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (!task.Result)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                        {
                            ColorMsgBox = new SolidColorBrush(Colors.OrangeRed);
                            AsyncMessageBox("Erro inesperado :( \nCadastro não gravado!", false);
                        }));
                    }
                }
            }, System.Threading.CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncGravarInscricao()
        {
            foreach (string str in AreaTransferencia.Inscricao)
            {
                mInscricao insc = new mInscricao();
                insc.Inscricao = str;
                insc.Atendimento = Atendimento.Protocolo;
                insc.Ativo = true;
                mdata.AtualizarAtendimentoInscricao(insc);
                Task.Factory.StartNew(() => mdata.AtualizarAtendimentoInscricao(insc)).ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        if (!task.Result)
                        {
                            Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                            {
                                ColorMsgBox = new SolidColorBrush(Colors.OrangeRed);
                                AsyncMessageBox("Erro inesperado :( \nInscricao não gravada!", false);
                            }));
                        }
                    }
                }, System.Threading.CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void AsyncGravarAtendimento()
        {
            Task.Factory.StartNew(() => mdata.GravarAtendimento(Atendimento)).ContinueWith(task =>
            {
                if (!task.Result)
                {
                    Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                    {

                        ColorMsgBox = new SolidColorBrush(Colors.OrangeRed);
                        AsyncMessageBox("Erro inesperado :( \nAtendimento não gravado!", false);
                    }));
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                    {
                        ColorMsgBox = new SolidColorBrush(Colors.Green);
                        AsyncMessageBox(string.Format("Atendimento {0} gravado com sucesso!", Atendimento.Protocolo), true);
                    }));
                }

            }, System.Threading.CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Gravar()
        {
            try
            {
                Atendimento.Protocolo = Protocolo;
                Atendimento.Data = DateTime.Now;

                //Ambulante
                if (ServicosRealizados.Any(l => l == "CADASTRO DE COMÉRCIO AMBULANTE"))//(Atendimento.Tipo == 12)
                    AsyncGravarAmbulante();

                //Inscrição
                if (ServicosRealizados.Any(l => l == "INSCRIÇÃO"))//(Atendimento.Tipo == 3)
                    AsyncGravarInscricao();

                StringBuilder sb = new StringBuilder();

                foreach (string sv in ServicosRealizados)
                {
                    if (sv != null && sv != string.Empty)
                        sb.Append(sv + ";");
                }

                Atendimento.TipoString = sb.ToString();

                AsyncGravarAtendimento();
            }
            catch (Exception ex)
            {
                ColorMsgBox = new SolidColorBrush(Colors.Red);
                TextMsgBox = ex.Message;
            }
        }
               
        #endregion

        #region MessageBox

        #region Declarations
        private SolidColorBrush _msgcolor;
        private Visibility _showmsg = Visibility.Collapsed;
        private string _msgbox;
        private int _reportprogress;
        #endregion

        #region Properties
        public Visibility ShowMsgBox
        {
            get { return _showmsg; }
            set
            {
                _showmsg = value;
                RaisePropertyChanged("ShowMsgBox");
            }
        }

        public string TextMsgBox
        {
            get { return _msgbox; }
            set
            {
                _msgbox = value;
                RaisePropertyChanged("TextMsgBox");
            }
        }

        public int ReportProgress
        {
            get { return _reportprogress; }
            set
            {
                _reportprogress = value;
                RaisePropertyChanged("ReportProgress");
            }
        }

        public SolidColorBrush ColorMsgBox
        {
            get { return _msgcolor; }
            set
            {
                _msgcolor = value;
                RaisePropertyChanged("ColorMsgBox");
            }
        }
        #endregion

        #region Functions
        private void Freezetime(int valor)
        {
            for (int x = 100; x > 0; x--)
            {
                ReportProgress = (100 * x) / 100;
                System.Threading.Thread.Sleep(valor);
            }
        }
        /// <summary>
        /// Exibe MessageBox
        /// </summary>
        /// <param name="message">passa messagem por parametro</param>
        private void AsyncMessageBox(string message, bool retornook)
        {
            BlackBox = Visibility.Collapsed;
            StarProgress = false;
            TextMsgBox = message;
            ShowMsgBox = Visibility.Visible;
            System.Media.SystemSounds.Exclamation.Play();

            Task.Factory.StartNew(() => Freezetime(10)).ContinueWith(taskmsg =>
            {
                if (taskmsg.IsCompleted)
                {
                    ShowMsgBox = Visibility.Collapsed;
                    if (retornook == true)
                    {
                        Atendimento.Clear();
                        PF.Clear();
                        PJ.Clear();
                        Protocolo = NProtocolo();
                        Atendimento.Operador = AccountOn.Identificador;
                        AreaTransferencia.Limpar();
                        ns.GoBack();
                    }
                }
            }, System.Threading.CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

        #endregion
    }
}
