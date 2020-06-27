using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Threading.Tasks;
using System.Threading;

namespace Sim.Modulos.Accounts.ViewModel
{

    using Sim.Modulos.Accounts.Model;
    using Sim.Mvvm.Helpers.Observers;
    using Sim.Mvvm.Helpers.Commands;
    using Sim.Mvvm.Helpers.Notifiers;
    using System.Collections.ObjectModel;

    class vmChange : vmAdd
    {
        #region Declarations
        private ICommand _commandgravarchange;
        private mAccount _mccount = new mAccount();
        private bool _startprogress;
        private Visibility _blackbox;
        private string _message;
        #endregion

        #region Properties
        public mAccount Account
        {
            get { return _mccount; }

            set
            {
                _mccount = value;
                RaisePropertyChanged("Account");
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
        public ICommand CommandGravarChange
        {
            get
            {
                if (_commandgravarchange == null)
                    _commandgravarchange = new RelayCommand(p=> {
                        AsyncUpdate();                 

                    });

                return _commandgravarchange;
            }
        }
        #endregion

        #region Construtor
        public vmChange()
        {
            ns = GlobalNavigation.NavService;
            MsgBox = Visibility.Collapsed;
            BlackBox = Visibility.Collapsed;
            StartProgress = false;

            if (mIDInternal.IDTransport != null || mIDInternal.IDTransport != string.Empty)
                AsyncAccounts();
        }
        #endregion

        #region Methods

        #endregion

        #region  Functions
        private bool Update()
        {
            try
            {
                //MessageBox.Show(AccountOn.Acesso + " " + Account.Conta.Conta);
                
                if (AccountOn.Acesso >= Account.Conta.Conta)
                {

                    var updateUser = new mUser();

                    updateUser.Indice = Account.Indice;
                    updateUser.Identificador = Account.Identificador;
                    updateUser.Nome = Account.Nome;
                    updateUser.Sexo = Account.Sexo;
                    updateUser.Email = Account.Email;
                    updateUser.Cadastro = Account.Cadastro;
                    updateUser.Atualizado = Account.Atualizado;
                    updateUser.Ativo = Account.Ativo;

                    if (new mData().UpdateUsuario(updateUser))
                    {

                        Opcoes = new mOpcoes()
                        {
                            Identificador = Account.Identificador,
                            Thema = Account.Thema,
                            Color = Account.Color
                        };

                        Conta = new mContas()
                        {
                            Identificador = Account.Identificador,
                            Conta = Account.Conta.Conta,
                            Ativo = true,
                        };

                        new mData().RemoveAcessoModulos(Account.Identificador);
                        foreach (mModuloGenerico md in ListaModulos)
                        {
                            Modulo.Identificador = Account.Identificador;
                            Modulo.Modulo = md.ValorModulo;
                            Modulo.Acesso = md.AcessoModulo;
                            new mData().GravarModulos(Modulo);
                        }

                        new mData().RemoveAcessoSubModulos(Account.Identificador);
                        foreach (mModuloGenerico smd in ListaSubModulos)
                        {
                            SubModulo.Identificador = Account.Identificador;
                            SubModulo.SubModulo = smd.ValorModulo;
                            SubModulo.Acesso = smd.ValorAcesso;
                            new mData().GravarSubModulos(SubModulo);
                        }

                        if (new mData().GravarOpcoes(Opcoes))

                            if (new mData().GravarConta(Conta))
                                _message = (string.Format("Dados do usuário {1} ID {0} alterados com sucesso!", User.Identificador, User.Nome));

                        return true;
                    }
                    else
                        return false;
                    
                }
                else
                {
                    _message = string.Format("Conta com nível de acesso inválido!\nSelecione uma opção inferior à [{0}].", AccountOn.Conta);
                    return false;                    
                }
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                return false;
            }
        }

        private void AsyncAccounts()
        {

            Task.Factory.StartNew(() => new mData().User(mIDInternal.IDTransport))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Account = task.Result;

                        foreach(mModulos m in Account.Modulos)
                        {
                            ListaModulos.Add(new mModuloGenerico()
                            {

                                Indice = m.Indice,
                                Identificador = m.Identificador,
                                NomeModulo = Modulos[m.Modulo - 1].Nome,
                                ValorModulo = m.Modulo,
                                AcessoModulo = m.Acesso,
                                AcessoNome = m.Acesso.ToString(),
                                Ativo = true
                            });
                        }

                        foreach (mSubModulos m in Account.SubModulos)
                        {
                            ListaSubModulos.Add(new mModuloGenerico()
                            {

                                Indice = m.Indice,
                                Identificador = m.Identificador,
                                NomeModulo = SubModulos[m.SubModulo - 1].Nome,
                                ValorModulo = m.SubModulo,
                                ValorAcesso = m.Acesso,
                                AcessoNome = AcessoSubModulos[m.Acesso].Nome,
                                Ativo = true
                            });
                        }
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncUpdate()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;

            Task.Factory.StartNew(() => Update())
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {

                        _retorn = task.Result;

                        if (task.Result)
                            MsgColor = new SolidColorBrush(Colors.Green);

                        else
                            MsgColor = new SolidColorBrush(Colors.DarkRed);

                        Application.Current.Dispatcher.BeginInvoke(new ThreadStart(() =>
                           {
                               AsyncMessageBox(_message);
                               BlackBox = Visibility.Collapsed;
                               StartProgress = false;
                           }));
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());

        }
        #endregion
    }
}
