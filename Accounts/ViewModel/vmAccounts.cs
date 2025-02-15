﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Sim.Modulos.Accounts.ViewModel
{    
    using Sim.Modulos.Accounts.Model;
    using Sim.Mvvm.Helpers.Commands;
    using Sim.Mvvm.Helpers.Notifiers;

    class vmAccounts : NotifyProperty
    {


        #region Declarations
        NavigationService ns;
        private ObservableCollection<mAccount> _accounts = new ObservableCollection<mAccount>();

        private bool _startprogress;

        private Visibility _blackbox;

        private ICommand _newuserCommand;
        private ICommand _editCommand;
        private ICommand _blockuser;
        private ICommand _delluser;
        private ICommand _resetpw;
        #endregion

        #region Constructor
        public vmAccounts()
        {
            BlackBox = Visibility.Collapsed;
            StartProgress = false;
            ns = Mvvm.Helpers.Observers.GlobalNavigation.NavService;
            Mvvm.Helpers.Observers.GlobalNavigation.Pagina = "CENTRAL DE CONTAS";
            AsyncAccountsList();
        }
        #endregion

        #region Properties
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

        public ObservableCollection<mAccount> AccountsList
        {
            get { return _accounts; }
            set
            {
                if (_accounts != value)
                {
                    _accounts = value;
                    RaisePropertyChanged("AccountsList");
                }
            }
        }
        #endregion

        #region Commands
        public ICommand CommandAdd
        {
            get
            {
                if (_newuserCommand == null)
                {
                    _newuserCommand = new RelayCommand(p=>
                    {
                        if (AccountOn.Acesso >= (int)AccountAccess.Administrador)
                            ns.Navigate(new Uri("/Sim.Modulo.Autenticacao;component/View/pAdd.xaml", UriKind.RelativeOrAbsolute));
                    });
                }
                return _newuserCommand;
            }
        }
        
        public ICommand CommandEdite
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new RelayCommand(p =>
                    {
                        if (AccountOn.Acesso >= (int)AccountAccess.Administrador)
                        {
                            mIDInternal.IDTransport = p.ToString();
                            ns.Navigate(new Uri("/Sim.Modulo.Autenticacao;component/View/pManager.xaml", UriKind.RelativeOrAbsolute));
                        }
                    });
                }
                return _editCommand;
            }
        }
        
        public ICommand CommandResetPW
        {
            get
            {
                if (_resetpw == null)
                    _resetpw = new RelayCommand(p=> {

                        if (AccountOn.Acesso >= (int)AccountAccess.Administrador)
                        {
                            try
                            {

                                string mss = string.Format("Reiniciar a senha do Usuário [{0}]?", p.ToString());
                                MessageBoxButton mbb = MessageBoxButton.YesNo;
                                MessageBoxImage mbi = MessageBoxImage.Warning;
                                MessageBoxResult mbr = MessageBoxResult.Yes;

                                if (MessageBox.Show(mss, "Sim.App.Alerta", mbb, mbi) == mbr)
                                {
                                    if (new mData().ResetPW(p.ToString(), p.ToString()))
                                        MessageBox.Show("Senha reiniciada","Sim.Aviso!");
                                }
                            }
                            catch (Exception ex)
                            { MessageBox.Show(ex.Message, "Sim.App.Alerta"); }
                        }
                    });

                return _resetpw;
            }
        }
        
        public ICommand CommandBlockAccount
        {
            get
            {
                if (_blockuser == null)
                    _blockuser = new RelayCommand(p=> {
                        if (AccountOn.Acesso >= (int)AccountAccess.Administrador)
                        {
                            try
                            {
                                string mss = string.Format("Bloquear Usuário [{0}]?", p.ToString());
                                MessageBoxButton mbb = MessageBoxButton.YesNo;
                                MessageBoxImage mbi = MessageBoxImage.Warning;
                                MessageBoxResult mbr = MessageBoxResult.Yes;

                                if (MessageBox.Show(mss, "Sim.App.Alerta", mbb, mbi) == mbr)
                                {
                                    if (new mData().BlockAccount(p.ToString()))
                                        MessageBox.Show("Conta Bloqueada!", "Sim.Aviso!");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    });

                return _blockuser;
            }
        }
        
        public ICommand CommandDeleteAccount
        {
            get
            {
                if (_delluser == null)
                    _delluser = new RelayCommand(p =>
                    {
                        if (AccountOn.Acesso >= (int)AccountAccess.Administrador)
                        {
                            string mss = string.Format("Apagar todos os dados do Usuário [{0}]?\n*Não é possível recuperar as informações!", p.ToString());
                            MessageBoxButton mbb = MessageBoxButton.YesNo;
                            MessageBoxImage mbi = MessageBoxImage.Warning;
                            MessageBoxResult mbr = MessageBoxResult.Yes;

                            try
                            {

                                if (MessageBox.Show(mss, "Sim.App.Alerta", mbb, mbi) == mbr)
                                {
                                    if (new mData().DeleteAccount(p.ToString()))
                                        MessageBox.Show("Conta e dados apagados", "Sim.Aviso");
                                }
                            }
                            catch (Exception ex)
                            {
                                mbb = MessageBoxButton.OK;
                                MessageBox.Show(ex.Message, "Sim.App.Aviso!", mbb, mbi);
                            }
                        }
                    });

                return _delluser;
            }
        }
        
        #endregion

        #region Functions
        private void AsyncAccountsList()
        {
            //BlackBox = Visibility.Visible;
            //StartProgress = true;

            Task.Factory.StartNew(() => new mData().Accountlist(AccountOn.Acesso))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        AccountsList = task.Result;
                        //BlackBox = Visibility.Collapsed;
                        //StartProgress = false;
                    }
                    else
                    {
                        //BlackBox = Visibility.Collapsed;
                        //StartProgress = false;
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

    }
}
