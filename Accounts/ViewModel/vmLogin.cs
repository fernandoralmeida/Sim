#define RELEASE

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Threading;

namespace Sim.Modulos.Accounts.ViewModel
{
    using Model;
    using Sim.Mvvm.Helpers.Notifiers;
    using Sim.Mvvm.Helpers.Commands;


    class vmLogin : NotifyProperty
    {

        #region Delcarations
        private Uri _img = new Uri( "/Sim.Modulo.Autenticacao;component/Imgs/UserAccountsIcon.png", UriKind.RelativeOrAbsolute);

        private int _focus;
        private int _rpgrss;
        private string _getid = string.Empty;
        private string _getname = string.Empty;
        private string _errormsg = string.Empty;
        private bool _startprogress;

        private Visibility _viewid;
        private Visibility _viewpw;
        private Visibility _viewretun;
        private Visibility _showmsg;
        private Visibility _blackbox;

        private ICommand _commandlogin;
        private ICommand _commandvoltar;
        private ICommand _commandcancel;
        private ICommand _commandgetid;
        private ICommand _commandmsgok;

        #endregion

        #region Properties
        public Uri LoginImage
        {
            get { return _img; }
            set
            {
                _img = value;
                RaisePropertyChanged("LoginImage");
            }
        }

        public int FocusedElements
        {
            get { return _focus; }
            set { _focus = value;
                RaisePropertyChanged("FocusedElements");
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

        public string GetName
        {
            get { return _getname; }
            set
            {
                _getname = value;
                RaisePropertyChanged("GetName");
            }
        }

        public string GetID
        {
            get { return _getid; }
            set
            {
                _getid = value;
                RaisePropertyChanged("GetID");
            }
        }

        public string ErrorMsg
        {
            get { return _errormsg; }
            set
            {
                _errormsg = value;
                RaisePropertyChanged("ErrorMsg");
            }
        }

        public bool StartProgress
        {
            get { return _startprogress; }
            set
            {
                _startprogress = value;
                RaisePropertyChanged("StartProgress");
            }
        }

        public Visibility ViewID
        {
            get { return _viewid; }
            set
            {
                _viewid = value;
                RaisePropertyChanged("ViewID");
            }
        }

        public Visibility ViewPW
        {
            get { return _viewpw; }
            set
            {
                _viewpw = value;
                RaisePropertyChanged("ViewPW");
            }
        }

        public Visibility ViewReturn
        {
            get { return _viewretun; }
            set
            {
                _viewretun = value;
                RaisePropertyChanged("ViewReturn");
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
        public ICommand CommandGetID
        {
            get
            {
                if (_commandgetid == null)
                    _commandgetid = new DelegateCommand(ExecuteCommandGetID, null);
                return _commandgetid;
            }
        }


        public ICommand CommandLogin
        {
            get
            {
                if (_commandlogin == null)
                    _commandlogin = new DelegateCommand(ExecuteLoginCommand, null);

                return _commandlogin;
            }
        }

        public ICommand CommandVoltar
        {
            get
            {
                if (_commandvoltar == null)
                    _commandvoltar = new DelegateCommand(ExecuteCommandVoltar, null);
                return _commandvoltar;
            }
        }

        public ICommand CommandCancel
        {
            get
            {
                if (_commandcancel == null)
                    _commandcancel = new DelegateCommand(ExecuteCancelCommand, null);

                return _commandcancel;
            }
        }

        public ICommand CommandMsgOk
        {
            get
            {
                if (_commandmsgok == null)
                    _commandmsgok = new DelegateCommand(ExecuteCommandMsgOk, null);

                return _commandmsgok;
            }
        }

        private void ExecuteCommandMsgOk(object obj)
        {
            ShowMSG = Visibility.Collapsed;
            FocusedElements = 0;
        }
        #endregion

        #region Constructor

        public vmLogin()
        {
            StartProgress = false;
            BlackBox = Visibility.Collapsed;
            ShowMSG = Visibility.Collapsed;

            if (AccountOn.Autenticado == true)
                AccountOn.Autenticado = false;

            GetName = "SIM.LOGIN";
            ViewID = Visibility.Visible;
            ViewPW = Visibility.Collapsed;
            ViewReturn = Visibility.Collapsed;
            FocusedElements = 0;
        }

        #endregion

        #region Methods

        private void ExecuteCommandGetID(object obj)
        {
            var login = new mData();

            StartProgress = true;
            BlackBox = Visibility.Visible;

            var tbox = (TextBox)obj;
            GetID = tbox.Text;

            if (GetID == null || GetID == string.Empty)
            {
                BlackBox = Visibility.Collapsed;
                StartProgress = false;
                return;
            }

            try
            {
                Task.Factory.StartNew(() => login.Operador(GetID)).ContinueWith(t =>
                {

                    if (t.IsFaulted)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new ThreadStart(() =>
                        {
                            BlackBox = Visibility.Collapsed;
                            StartProgress = false;
                            AsyncMessageBox("Operador inválido!");
                        }));
                    }

                    if (t.IsCompleted)
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                        GetName = t.Result;
                        LoginImage = new Uri("/Sim.Modulo.Autenticacao;component/Imgs/keys.ico", UriKind.RelativeOrAbsolute);
                        ViewID = Visibility.Collapsed;
                        ViewPW = Visibility.Visible;
                        ViewReturn = Visibility.Visible;
                        FocusedElements = 1;
                    }

                }, CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch(Exception ex)
            {
                BlackBox = Visibility.Collapsed;
                StartProgress = false;
                AsyncMessageBox(ex.Message);
            }

        }

        private void ExecuteCommandVoltar(object obj)
        {
            GetName = "SIM.LOGIN";
            ViewID = Visibility.Visible;
            ViewPW = Visibility.Collapsed;
            ViewReturn = Visibility.Collapsed;
            LoginImage = new Uri("/Sim.Modulo.Autenticacao;component/Imgs/UserAccountsIcon.png", UriKind.RelativeOrAbsolute);
            FocusedElements = 0;
        }

        private void ExecuteLoginCommand(object param)
        {
            var login = new mData();
            StartProgress = true;
            BlackBox = Visibility.Visible;

            try
            {
                var objetos = (object)param;
                var pswd = (PasswordBox)objetos;

                if (pswd.Password == null || pswd.Password == string.Empty)
                {
                    StartProgress = false;
                    BlackBox = Visibility.Collapsed;
                    return;
                    throw new Exception("Digite sua senha!");                    
                }
                
                Task.Factory.StartNew(() => login.AutenticarUsuario(GetID, pswd.Password)).ContinueWith(t =>
                {

                    if (t.IsCompleted)
                    {
                        if (t.Result != null)
                        {                 
                            AccountOn.Indice = t.Result.Indice;
                            AccountOn.Identificador = t.Result.Identificador;
                            AccountOn.Color = t.Result.Color;
                            AccountOn.Thema = t.Result.Thema;
                            AccountOn.Acesso = t.Result.Conta.Conta;
                            AccountOn.Conta = t.Result.Conta.ContaAcesso;
                            AccountOn.Modulos = t.Result.Modulos;
                            AccountOn.Submodulos = t.Result.SubModulos;
                            AccountOn.Nome = t.Result.Nome;
                            AccountOn.Sexo = t.Result.Sexo.ToUpper();
                            AccountOn.Cadastro = t.Result.Cadastro;
                            AccountOn.Atualizado = t.Result.Atualizado;
                            AccountOn.Ativo = t.Result.Ativo;
                            AccountOn.Email = t.Result.Email;
                            AccountOn.Registro = t.Result.Registro;

                            AccountOn.Autenticado = t.Result.Autenticado;
                        }
                        else
                        {
                            Application.Current.Dispatcher.BeginInvoke(new ThreadStart(() =>
                            {
                                AsyncMessageBox("Senha incorreta!");
                            }));
                        }

                        StartProgress = false;
                        BlackBox = Visibility.Collapsed;
                    }

                }, CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());

            }
            catch (Exception ex)
            {
                AsyncMessageBox(ex.Message);
            }
        }

        private void ExecuteCancelCommand(object obj)
        {
            Application.Current.Shutdown();
        }

        private void AsyncMessageBox(string message)
        {
            ErrorMsg = message;
            ShowMSG = Visibility.Visible;
            System.Media.SystemSounds.Exclamation.Play();
            Task.Factory.StartNew(() => { for (int x = 100; x > 0; x--) { ReportProgress = (100 * x) / 100; Thread.Sleep(10);  } } )
                .ContinueWith(task =>
                {  
                    if (task.IsCompleted)
                    {
                        ShowMSG = Visibility.Collapsed;
                    }
                },
                CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

    }
}
