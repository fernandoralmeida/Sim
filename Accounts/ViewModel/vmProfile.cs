using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;

namespace Sim.Modulos.Accounts.ViewModel
{
    
    using Sim.Mvvm.Helpers.Observers;
    using Sim.Mvvm.Helpers.Commands;
    using Sim.Mvvm.Helpers.Notifiers;
    using Model;

    class vmProfile : NotifyProperty
    {

        #region Declarations
        private mUser _user = new mUser();
        private AccountOn _account = new AccountOn();

        private bool _ismaster;

        private Visibility _changepw;

        private ICommand _commandpwbox;
        private ICommand _commandchangepw;
        private ICommand _commandchangeperfil;
        private ICommand _commandreturn;
        #endregion

        #region Constructors
        public vmProfile()
        {
            GlobalNavigation.Pagina = "MEU PERFIL";
            GlobalNavigation.BrowseBack = Visibility.Visible;
            ChangePW = Visibility.Collapsed;

            IsEditable = true;

            if (AccountOn.Registro.CodigoAcesso.Contains("system_"))
                IsEditable = false;

            UserOn.Indice = AccountOn.Indice;
            UserOn.Identificador = AccountOn.Identificador;
            UserOn.Nome = AccountOn.Nome;
            UserOn.Sexo = AccountOn.Sexo;
            UserOn.Email = AccountOn.Email;
        }
        #endregion

        #region Properties
        public Visibility ChangePW
        {
            get { return _changepw; }
            set
            {
                _changepw = value;
                RaisePropertyChanged("ChangePW");
            }
        }

        public List<mGenerico> Contas
        {
            get { return new mData().TiposContasLista(); }
        }

        public List<mGenero> Generos
        {
            get { return new mGenero().Generos(); }
        }

        public AccountOn Account
        {
            get { return _account; }
            set
            {
                if (_account != value)
                {
                    _account = value;
                    RaisePropertyChanged("Account");
                }
            }
        }

        public mUser UserOn
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChanged("UserOn");
            }
        }

        public bool IsEditable
        {
            get { return _ismaster; }
            set
            {
                _ismaster = value;
                RaisePropertyChanged("IsMaster");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandPWBox
        {
            get
            {
                if (_commandpwbox == null)
                    _commandpwbox = new RelayCommand(p=> {
                        ChangePW = Visibility.Visible;
                    });

                return _commandpwbox;
            }
        }

        public ICommand CommandChangePW
        {
            get
            {
                if (_commandchangepw == null)
                    _commandchangepw = new RelayCommand(p=> {

                        object[] obj = (object[])p;

                        var opw = (PasswordBox)obj[0];
                        var npw = (PasswordBox)obj[1];
                        var idbox = (TextBox)obj[2];

                        if (new mData().ChangePW(idbox.Text, opw.Password, npw.Password))
                        {
                            MessageBox.Show("Senha alterada!", "Sim.Aviso!");
                            ChangePW = Visibility.Collapsed;
                        }

                    });

                return _commandchangepw;
            }
        }

        public ICommand CommandChangePerfil
        {
            get
            {
                if (_commandchangeperfil == null)
                    _commandchangeperfil = new RelayCommand(p => {

                        UserOn.Atualizado = DateTime.Now;
                        UserOn.Ativo = true;

                        if (new mData().UpdateUsuario(UserOn))
                            MessageBox.Show("Dados atualizados!", "Sim.Aviso!");

                    });

                return _commandchangeperfil;
            }
        }

        public ICommand CommandReturn
        {
            get
            {
                if (_commandreturn == null)
                    _commandreturn = new RelayCommand(p=> {

                        ChangePW = Visibility.Collapsed;

                    });

                return _commandreturn;
            }
        }
        #endregion

    }
}
