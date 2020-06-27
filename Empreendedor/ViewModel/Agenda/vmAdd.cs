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


    class vmAdd : NotifyProperty
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
                        Agenda.Ativo = true;
                        Agenda.Criacao = DateTime.Now;
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
        public vmAdd()
        {
            ns = GlobalNavigation.NavService;
            Agenda.Data = DateTime.Now;
            BlackBox = Visibility.Collapsed;
            StartProgress = false;
            Agenda.Codigo = Codigo();
        }
        #endregion

        #region Methods

        #endregion

        #region Functions
        private string Codigo()
        {
            string r = string.Empty;

            string a = DateTime.Now.Year.ToString("0000");
            string m = DateTime.Now.Month.ToString("00");
            string d = DateTime.Now.Day.ToString("00");

            string hs = DateTime.Now.Hour.ToString("00");
            string mn = DateTime.Now.Minute.ToString("00");
            string ss = DateTime.Now.Second.ToString("00");

            r = string.Format(@"EV{0}{1}{2}{3}{4}{5}",
                a, m, d, hs, mn, ss);

            return r;
        }

        private void AsyncGravarAgenda()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            Task<bool>.Factory.StartNew(() => new mData().GravarEvento(Agenda))
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
        #endregion
    }
}
