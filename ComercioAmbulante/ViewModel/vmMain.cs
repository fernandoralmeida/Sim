using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Sim.Modulos.ComercioAmbulante.ViewModel
{
    
    using Mvvm.Helpers.Notifiers;
    using Mvvm.Helpers.Commands;
    using Mvvm.Helpers.Observers;
    using System.Windows.Documents;
    using System.Windows.Media;
    using Accounts;
    using Empreendedor.Model;
    using Model;

    class vmMain : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        private mData mdata = new mData();
        private mDataCM mdatacm = new mDataCM();
        private ObservableCollection<mAtendimento> _listaat = new ObservableCollection<mAtendimento>();
        private ObservableCollection<mAgenda> _listag = new ObservableCollection<mAgenda>();

        private FlowDocument _flowdoc = new FlowDocument();
        private FlowDocument _flowtemp = new FlowDocument();
        private mAtendimentoSebrae _atsebrae = new mAtendimentoSebrae();

        private DateTime _datai = DateTime.Now;
        private Visibility _blackbox;
        private Visibility _previewbox;
        private Visibility _agendaview;
        private Visibility _retvisible = Visibility.Collapsed;

        private string _preview = string.Empty;
        private bool _starprogress;

        private bool _viewcnpj = false;
        private bool _isadmin;
        private bool _isenable;
        private int _selectedrow;

        private ICommand _commandclosepreview;
        private ICommand _commandpreviewbox;
        private ICommand _commandpreviewbox2;
        private ICommand _commandatsebrae;
        private ICommand _commandrefreshdate;
        private ICommand _commandgocnpj;
        private ICommand _commandretcpf;
        private ICommand _commandnavigate;
        private ICommand _commandexpand;
        private ICommand _commandverevento;
        private ICommand _commandrefresh;
        #endregion

        #region Properties
        
        public ObservableCollection<mTiposGenericos> Tipos
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Tipo"); }
        }

        public FlowDocument FlowDoc
        {
            get { return _flowdoc; }
            set
            {
                _flowdoc = value;
                RaisePropertyChanged("FlowDoc");
            }
        }
        
        public mAtendimentoSebrae AtSebrae
        {
            get { return _atsebrae; }
            set
            {
                _atsebrae = value;
                RaisePropertyChanged("AtSebrae");
            }
        }
        
        public ObservableCollection<mAgenda> ListarAgenda
        {
            get { return _listag; }
            set
            {
                _listag = value;
                RaisePropertyChanged("ListarAgenda");
            }
        }

        public ObservableCollection<mAtendimento> ListarAtendimentos
        {
            get { return _listaat; }
            set
            {
                _listaat = value;
                RaisePropertyChanged("ListarAtendimentos");
            }
        }
        

        public DateTime DataI
        {
            get { return _datai; }
            set
            {
                _datai = value;
                CommandRefreshDate.Execute(null);
                RaisePropertyChanged("DataI");
            }
        }

        public Visibility AgendaView
        {
            get { return _agendaview; }
            set
            {
                _agendaview = value;
                RaisePropertyChanged("AgendaView");
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

        public Visibility PreviewBox
        {
            get { return _previewbox; }
            set
            {
                _previewbox = value;
                RaisePropertyChanged("PreviewBox");
            }
        }

        public Visibility RetVisible
        {
            get { return _retvisible; }
            set
            {
                _retvisible = value;
                RaisePropertyChanged("RetVisible");
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

        public bool IsAdmin
        {
            get { return _isadmin; }
            set
            {
                _isadmin = value;
                RaisePropertyChanged("IsAdmin");
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
        /*
        public bool IsAtivo
        {
            get
            { return _isatvo; }
            set
            {
                _isatvo = value;
                if (value == true)
                    AsyncListarAgendaAtivo();
                RaisePropertyChanged("IsAtivo");
            }
        }

        public bool IsFinalizado
        {
            get { return _isfinalizado; }
            set
            {
                _isfinalizado = value;
                if (value == true)
                    AsyncListarAgendaFinalizado();
                RaisePropertyChanged("IsFinalizado");
            }
        }

        public bool IsCancelado
        {
            get { return _iscancelado; }
            set
            {
                _iscancelado = value;

                if (value == true)
                    AsyncListarAgendaCancelada();

                RaisePropertyChanged("IsCancelado");
            }
        }
        
        public bool IsVencido
        {
            get { return _isvencido; }
            set
            {
                _isvencido = value;
                if (value == true)
                    AsyncListarAgendaVencida();
                RaisePropertyChanged("IsVencido");
            }
        }*/

        public string Preview
        {
            get { return _preview; }
            set
            {
                _preview = value;
                RaisePropertyChanged("Preview");
            }
        }

        public string PageName
        {
            get { return GlobalNavigation.Pagina; }
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
        #endregion

        #region Commands
        public ICommand CommandVerEvento
        {
            get
            {
                if (_commandverevento == null)
                    _commandverevento = new RelayCommand(p =>
                    {
                        //AreaTransferencia.Evento = p.ToString();
                        ns.Navigate(new Uri(@"/Sim.Modulo.Empreendedor;component/View/Agenda/pDetalhe.xaml", UriKind.RelativeOrAbsolute));
                    });

                return _commandverevento;
            }
        }

        public ICommand CommandNavigate
        {
            get
            {
                if (_commandnavigate == null)
                    _commandnavigate = new RelayCommand(p => {

                        ns.Navigate(new Uri(p.ToString(), UriKind.RelativeOrAbsolute));

                    });

                return _commandnavigate;
            }
        }

        public ICommand CommandExpand
        {
            get
            {
                if (_commandexpand == null)
                    _commandexpand = new RelayCommand(p => {

                        if (AgendaView == Visibility.Visible)
                            AgendaView = Visibility.Collapsed;
                        else
                            AgendaView = Visibility.Visible;
                    });

                return _commandexpand;
            }
        }

        public ICommand CommandGoCNPJ
        {
            get
            {
                if (_commandgocnpj == null)
                    _commandgocnpj = new DelegateCommand(ExecCommandGoCNPJ, null);
                return _commandgocnpj;
            }
        }

        private void ExecCommandGoCNPJ(object obj)
        {
            _viewcnpj = true;
            _flowtemp = FlowDoc;
            FlowDoc = null;
            FlowDoc = FlowPJ((string)obj);
        }

        public ICommand CommandRetCPF
        {
            get
            {
                if (_commandretcpf == null)
                    _commandretcpf = new DelegateCommand(ExecCommandRetCPF, null);
                return _commandretcpf;
            }
        }

        private void ExecCommandRetCPF(object obj)
        {
            _viewcnpj = false;
            FlowDoc = null;
            FlowDoc = _flowtemp;
            RetVisible = Visibility.Collapsed;
        }

        public ICommand CommandClosePreview
        {
            get
            {
                if (_commandclosepreview == null)
                    _commandclosepreview = new DelegateCommand(ExecCommandClosePreview, null);
                return _commandclosepreview;
            }
        }

        private void ExecCommandClosePreview(object obj)
        {
            PreviewBox = Visibility.Collapsed;
            _viewcnpj = false;
        }

        public ICommand CommandPreviewBox
        {
            get
            {
                if (_commandpreviewbox == null)
                    _commandpreviewbox = new DelegateCommand(ExecCommandPreviewBox, null);
                return _commandpreviewbox;
            }
        }

        private void ExecCommandPreviewBox(object obj)
        {
            try
            {
                FlowDoc = ApresentarDados(obj);
                PreviewBox = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }

        public ICommand CommandPreviewBox2
        {
            get
            {
                if (_commandpreviewbox2 == null)
                    _commandpreviewbox2 = new DelegateCommand(ExecCommandPreviewBox2, null);
                return _commandpreviewbox2;
            }
        }

        private void ExecCommandPreviewBox2(object obj)
        {
            try
            {
                FlowDoc = FlowAtendimento((string)obj);
                PreviewBox = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }
        public ICommand CommandAtSebrae
        {
            get
            {
                if (_commandatsebrae == null)
                    _commandatsebrae = new DelegateCommand(ExecCommandAtSebrae, null);
                return _commandatsebrae;
            }
        }

        private void ExecCommandAtSebrae(object obj)
        {
            //GravarAtendimentoSebrae();
        }

        public ICommand CommandRefreshDate
        {
            get
            {
                if (_commandrefreshdate == null)
                    _commandrefreshdate = new DelegateCommand(ExecCommandRefreshDate, null);

                return _commandrefreshdate;
            }
        }

        private void ExecCommandRefreshDate(object obj)
        {
            if (GlobalNavigation.Parametro != string.Empty)
                AsyncListarAtendimentoHoje(Parametros(GlobalNavigation.Parametro));
        }

        public ICommand CommandRefresh
        {
            get
            {
                if (_commandrefresh == null)
                    _commandrefresh = new RelayCommand(p => {

                        AsyncListarAgenda();

                    });

                return _commandrefresh;
            }
        }
        #endregion

        #region Constructor
        public vmMain()
        {
            GlobalNavigation.Pagina = string.Empty;
            ns = GlobalNavigation.NavService;
            BlackBox = Visibility.Collapsed;
            PreviewBox = Visibility.Collapsed;
            RetVisible = Visibility.Collapsed;
            StartProgress = false;
            AreaTransferencia.Limpar();

            //IsAtivo = true;

            //AsyncListarAgenda();

            if (GlobalNavigation.Parametro != string.Empty)
                AsyncListarAtendimentoHoje(Parametros(GlobalNavigation.Parametro));

            IsEnable = false;
            IsAdmin = false;

            if (AccountOn.Acesso != 0)
            {

                if (AccountOn.Acesso == (int)AccountAccess.Master)
                {
                    IsEnable = true;
                    IsAdmin = true;
                }

                if (GlobalNavigation.SubModulo.ToLower() == "COMÉRCIO AMBULANTE".ToLower())
                {

                    foreach (Accounts.Model.mSubModulos m in AccountOn.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.ComercioAmbulante)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Consulta)
                                IsEnable = true;

                            if (m.Acesso > (int)SubModuloAccess.Operador)
                                IsAdmin = true;
                        }
                    }
                }
            }

        }
        #endregion

        #region Functions

        private List<string> Parametros(string param)
        {

            List<string> _param = new List<string>() { };

            _param.Add(DataI.ToShortDateString());

            _param.Add(AccountOn.Identificador);

            _param.Add("CADASTRO DE COMÉRCIO AMBULANTE");

            return _param;
        }
        
        private void AsyncListarAtendimentoHoje(List<string> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;

            Task.Factory.StartNew(() => mdatacm.AtendimentosComercioAmbulanteNow(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListarAtendimentos = task.Result;
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

        private void AsyncListarAgenda()
        {

            List<string> _list = new List<string>();

            _list.Add(DateTime.Now.ToShortDateString());
            _list.Add("1");
            _list.Add("1");

            Task.Factory.StartNew(() => mdata.Agenda(_list))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        ListarAgenda = task.Result;
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncListarAgendaAtivo()
        {

            List<string> _list = new List<string>();

            _list.Add(DateTime.Now.ToShortDateString());
            _list.Add("1");

            Task.Factory.StartNew(() => mdata.AgendaAtivo(_list))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        ListarAgenda = task.Result;
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncListarAgendaVencida()
        {

            List<string> _list = new List<string>();

            _list.Add(DateTime.Now.ToShortDateString());
            _list.Add("1");

            Task.Factory.StartNew(() => mdata.AgendaVencida(_list))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        ListarAgenda = task.Result;
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncListarAgendaCancelada()
        {

            List<string> _list = new List<string>();

            _list.Add("3");

            Task.Factory.StartNew(() => mdata.AgendaCancelada(_list))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        ListarAgenda = task.Result;
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncListarAgendaFinalizado()
        {

            List<string> _list = new List<string>();

            _list.Add("2");

            Task.Factory.StartNew(() => mdata.AgendaFinalizado(_list))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        ListarAgenda = task.Result;
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private FlowDocument ApresentarDados(object cliente)
        {

            string _cliente = new mMascaras().Remove((string)cliente);

            FlowDocument flow = new FlowDocument();

            switch (_cliente.Length)
            {
                case 11:
                    flow = FlowPF(_cliente);
                    break;

                case 14:
                    flow = FlowPJ(_cliente);
                    break;
            }

            return flow;
        }

        private FlowDocument FlowPF(string cliente)
        {

            mPF_Ext pessoafisica = new mPF_Ext();
            pessoafisica = new mData().ExistPessoaFisica(cliente);

            AtSebrae.Atendimento = ListarAtendimentos[SelectedRow].Protocolo;
            AtSebrae.Cliente = string.Format(@"{0}/{1}/{2}/{3}",
                    new mMascaras().Remove(pessoafisica.CPF),
                    pessoafisica.Nome,
                    pessoafisica.Telefones,
                    pessoafisica.Email);
            AtSebrae.Ativo = true;
            AtSebrae.AtendimentoSAC = ListarAtendimentos[SelectedRow].AtendimentoSebrae;

            FlowDocument flow = new FlowDocument();
            flow.Foreground = (Brush)Application.Current.Resources["WindowText"];
            flow.ColumnGap = 0;
            //flow.Background = Brushes.White;
            //Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            //flow.ColumnWidth = 8.5 * 96.0;
            //flow.PageHeight = 11.5 * 96.0;
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 12;
            flow.PagePadding = new Thickness(10);

            Paragraph pr = new Paragraph();
            pr.Inlines.Add(new Run("CLIENTE:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}, {2} ", pessoafisica.Nome, new mMascaras().CPF(pessoafisica.CPF), pessoafisica.DataNascimento.ToShortDateString()));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CEP:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoafisica.CEP);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ENDEREÇO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}", pessoafisica.Logradouro, pessoafisica.Numero));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("BAIRRO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}-{2}", pessoafisica.Bairro, pessoafisica.Municipio, pessoafisica.UF));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CONTATO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoafisica.Telefones + " " + pessoafisica.Email);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("PERFIL:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoafisica.Perfil.PerfilString);
            pr.Inlines.Add(new LineBreak());

            if (pessoafisica.ColecaoVinculos.Count > 0)
                pr.Inlines.Add(new Run("EMPRESAS:") { FontSize = 10, Foreground = Brushes.Gray });


            foreach (mVinculos v in pessoafisica.ColecaoVinculos)
            {
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("CNPJ "));
                pr.Inlines.Add(new Hyperlink(new Run(new mMascaras().CNPJ(v.CNPJ))) { Command = CommandGoCNPJ, CommandParameter = v.CNPJ });
                pr.Inlines.Add(new Run(" - " + v.VinculoString));
            }

            flow.Blocks.Add(pr);


            return flow;
        }

        private FlowDocument FlowPJ(string cliente)
        {

            mPJ_Ext pessoajuridica = new mPJ_Ext();

            pessoajuridica = new mData().ExistPessoaJuridica(cliente);

            AtSebrae.Atendimento = ListarAtendimentos[SelectedRow].Protocolo;
            AtSebrae.Cliente = string.Format(@"{0}/{1}/{2}/{3}",
                    new mMascaras().Remove(pessoajuridica.CNPJ),
                    pessoajuridica.RazaoSocial,
                    pessoajuridica.Telefones,
                    pessoajuridica.Email);

            AtSebrae.Ativo = true;

            FlowDocument flow = new FlowDocument();
            flow.Foreground = (Brush)Application.Current.Resources["WindowText"];
            flow.ColumnGap = 0;
            //flow.Background = Brushes.White;
            //Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            //flow.ColumnWidth = 8.5 * 96.0;
            //flow.PageHeight = 11.5 * 96.0;
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 12;
            flow.PagePadding = new Thickness(10);

            Paragraph pr = new Paragraph();
            pr.Inlines.Add(new Run("EMPRESA:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}, {2} ", new mMascaras().CNPJ(pessoajuridica.CNPJ), pessoajuridica.MatrizFilial, pessoajuridica.Abertura.ToShortDateString()));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ROZÃO SOCIAL") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.RazaoSocial);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("NOME FANTASIA") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.NomeFantasia);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ATIVIDADE PRINCIPAL") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.AtividadePrincipal);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ATIVIDADES SECUNDÁRIAS") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.AtividadeSecundaria);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("NATUREZA JURÍDICA") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.NaturezaJuridica);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("SITUAÇÃO CADASTRAL") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.SituacaoCadastral + " " + pessoajuridica.DataSituacaoCadastral.ToShortDateString());
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CEP:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.CEP);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ENDEREÇO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}", pessoajuridica.Logradouro, pessoajuridica.Numero));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("BAIRRO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}-{2} ", pessoajuridica.Bairro, pessoajuridica.Municipio, pessoajuridica.UF));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CONTATO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.Telefones + " " + pessoajuridica.Email);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("SETOR") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());

            if (pessoajuridica.Segmentos.Agronegocio)
                pr.Inlines.Add("AGRONEGÓCIO ");

            if (pessoajuridica.Segmentos.Industria)
                pr.Inlines.Add("INDÚSTRIA ");

            if (pessoajuridica.Segmentos.Comercio)
                pr.Inlines.Add("COMÉRCIO ");

            if (pessoajuridica.Segmentos.Servicos)
                pr.Inlines.Add("SERVIÇOS ");

            if (_viewcnpj == true)
            {
                RetVisible = Visibility.Visible;
                /*
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Hyperlink(new Run("VOLTAR")) { Command = CommandRetCPF });
                */
            }

            flow.Blocks.Add(pr);

            return flow;

        }

        private void GravarAtendimentoSebrae()
        {
            try
            {
                if (mdata.GravarAtendimentoSebrae(AtSebrae))
                {
                    MessageBox.Show("Atendimento lançado!", "Sim.Aviso!");
                    ListarAtendimentos[SelectedRow].AtendimentoSebrae = AtSebrae.AtendimentoSAC;
                    //AsyncListarAtendimentoHoje(Parametros(GlobalNavigation.Parametro));
                    ExecCommandClosePreview(null);
                }

                //AsyncListarAtendimentoHoje(Parametros());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }

        private FlowDocument FlowAtendimento(string protocolo)
        {
            var atdo = new mAtendimento();

            atdo = new mData().Atendimento(protocolo);

            FlowDocument flow = new FlowDocument();
            flow.Foreground = (Brush)Application.Current.Resources["WindowText"];
            flow.ColumnGap = 0;
            //flow.Background = Brushes.White;
            //Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            //flow.ColumnWidth = 8.5 * 96.0;
            //flow.PageHeight = 768;
            //flow.PageWidth = 1104;
            //flow.ColumnWidth = 1104;
            //flow.PageHeight = 11.5 * 96.0;
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 12;
            flow.PagePadding = new Thickness(10);

            Paragraph pr = new Paragraph();
            pr.Inlines.Add(new Run("ATENDIMENTO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1} {2} ", atdo.Protocolo, atdo.Data.ToShortDateString(), atdo.Hora.ToShortTimeString()));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CLIENTE") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}", atdo.Cliente.NomeRazao));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CONTATO") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1}", atdo.Cliente.Telefones, atdo.Cliente.Email));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("TIPO E ORIGEM DO ATENDIMENTO") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1}", atdo.TipoString, atdo.OrigemString));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("DESCRIÇÃO") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}", atdo.Historico));
            pr.Inlines.Add(new LineBreak());

            flow.Blocks.Add(pr);

            return flow;
        }
        
        #endregion
    }
}
