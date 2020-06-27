using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using System.ComponentModel;

namespace Sim.Modulos.Empreendedor.ViewModel.Agenda
{
    using Model;
    using Mvvm.Helpers.Notifiers;
    using Mvvm.Helpers.Commands;
    using Data.OnLine.Correios;
    using Mvvm.Helpers.Observers;

    class vmEdite : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        mAgenda _agd = new mAgenda();

        private Visibility _blackbox;

        private bool _starprogress;

        private ICommand _commandgravaragenda;
        private ICommand _commandcancelar;
        #endregion

        #region Properties
        public ObservableCollection<mTiposGenericos> Tipos
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Tipo"); }
        }

        public ObservableCollection<mTiposGenericos> Setores
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Setores WHERE (Ativo = True) ORDER BY Setor"); }
        }

        public ObservableCollection<mTiposGenericos> Estados
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Agenda_Estado WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public mAgenda Agenda
        {
            get { return _agd; }
            set
            {
                _agd = value;
                RaisePropertyChanged("Agenda");
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

        public bool StartProgress
        {
            get { return _starprogress; }
            set
            {
                _starprogress = value;
                RaisePropertyChanged("StartProgress");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandGravarAgenda
        {
            get
            {
                if (_commandgravaragenda == null)
                    _commandgravaragenda = new RelayCommand(p =>
                    {
                        AsyncGravarAgenda();
                    });
                return _commandgravaragenda;
            }
        }

        public ICommand CommandCancelar
        {
            get
            {
                if (_commandcancelar == null)
                    _commandcancelar = new RelayCommand(p =>
                    {
                        ns.GoBack();
                    });
                return _commandcancelar;
            }
        }
        #endregion

        #region Constructor
        public vmEdite()
        {
            ns = GlobalNavigation.NavService;
            BlackBox = Visibility.Collapsed;
            StartProgress = false;
            AsyncShowAgenda(AreaTransferencia.Parametro);
        }
        #endregion

        #region Methods

        #endregion

        #region Functions

        private void AsyncGravarAgenda()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            Task<bool>.Factory.StartNew(() => new mData().EditarEvento(Agenda.Codigo ,Agenda))
                .ContinueWith(task =>
                {
                    if (task.Result)
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                        ns.GoBack();
                    }
                    else
                    {

                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                        MessageBox.Show("Erro inesperado, contate o Administrador!", "Sim.Alerta!");
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncShowAgenda(string _param)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            Task.Factory.StartNew(() => new mData().Evento(_param))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Agenda = task.Result;
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion
    }
}
