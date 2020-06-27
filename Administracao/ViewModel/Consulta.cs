﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sim.Modulos.Administracao.ViewModel
{
    using Sim.Modulos.Portarias.Model;
    using Sim.Mvvm.Helpers.Notifiers;
    using Sim.Mvvm.Helpers.Commands;
    using Mvvm.Helpers.Observers;
    using Model;

    class Consulta : NotifyProperty
    {
        #region Declarações
        private NavigationService ns;
        private ObservableCollection<Avaliacoes> _consultaavaliacoes = new ObservableCollection<Avaliacoes>();
        private Visibility _blackbox = Visibility.Collapsed;
        private Visibility _msgbox = Visibility.Collapsed;
        private string _getnome;
        private string _getsecretaria;
        private DateTime _datai = new DateTime(2020, 01, 01);
        private List<string> _param = new List<string>();
        #endregion

        #region Propriedades
        public ObservableCollection<Avaliacoes> ConsultaAvaliacoes
        {
            get { return _consultaavaliacoes; }
            set
            {
                _consultaavaliacoes = value;
                RaisePropertyChanged("ConsultaAvaliacoes");
            }
        }

        public string GetNome
        {
            get { return _getnome; }
            set
            {
                _getnome = value;
                RaisePropertyChanged("GetNome");
            }
        }

        public string GetSecretaria
        {
            get { return _getsecretaria; }
            set
            {
                _getsecretaria = value;
                RaisePropertyChanged("GetSecretaria");
            }
        }

        public DateTime DataI
        {
            get { return _datai; }
            set
            {
                _datai = value;
                RaisePropertyChanged("DataI");
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

        public Visibility ShowMsgBox
        {
            get { return _msgbox; }
            set
            {
                _msgbox = value;
                RaisePropertyChanged("ShowMsgBox");
            }
        }
        #endregion

        #region Commandos
        public ICommand CommandFiltrar => new RelayCommand(p => {
            Listar().Wait();
        });

        public ICommand CommandListar => new RelayCommand(p => { });

        public ICommand CommandEdit => new RelayCommand(p => {            
            ns.Navigate(new Uri("/Sim.Modulo.Administracao;component/View/Editar.xaml", UriKind.RelativeOrAbsolute));
            GlobalNavigation.Parametro = p.ToString();
        });
        #endregion

        #region Construtor
        public Consulta()
        {
            GlobalNavigation.Pagina = "CONSULTA";
            ns = GlobalNavigation.NavService;
        }
        #endregion

        #region Methodos

        private List<string> Parametros()
        {
            var l = new List<string>();

            l.Add(DataI.ToShortDateString());

            if (GetNome == string.Empty || GetNome == null)
                l.Add("%");
            else
                l.Add(GetNome);

            if (GetSecretaria == string.Empty || GetSecretaria == null || GetSecretaria == "...")
                l.Add("%");
            else
                l.Add(GetSecretaria);

            return l;
        }

        private Task Listar()
        {
            var t = Task<ObservableCollection<Avaliacoes>>.Factory.StartNew(() => {
                return ConsultaAvaliacoes = new RepositorioAvaliacoes().Consulta(Parametros());
            });
            t.Wait();
            return t;
        }
        #endregion
    }
}
