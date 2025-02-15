﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Sim.Modulos.ComercioAmbulante.ViewModel
{

    using Model;
    using Empreendedor.Model;
    using Mvvm.Helpers.Notifiers;
    using Mvvm.Helpers.Observers;
    using Mvvm.Helpers.Commands;

    public class vmProfile : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        private mAmbulante _ambulante = new mAmbulante();

        private mDataCM mdatacm = new mDataCM();

        private ObservableCollection<string> _atividades = new ObservableCollection<string>();
        private ObservableCollection<string> _periodos = new ObservableCollection<string>();

        private string _outros = string.Empty;
        private string _situacao = string.Empty;

        private bool _istenda;
        private bool _isveiculo;
        private bool _istrailer;
        private bool _iscarrinho;
        private bool _isoutros;

        private bool _starprogress;

        private Visibility _blackbox;
        private Visibility _temlicencaview;

        private ICommand _commandprint;
        private ICommand _commandcancelar;
        #endregion

        #region Properties
        public mAmbulante Ambulante
        {
            get { return _ambulante; }
            set
            {
                _ambulante = value;
                RaisePropertyChanged("Ambulante");
            }
        }
        
        public ObservableCollection<string> Atividades
        {
            get { return _atividades; }
            set
            {
                _atividades = value;
                RaisePropertyChanged("Atividades");
            }
        }

        public ObservableCollection<string> Periodos
        {
            get { return _periodos; }
            set
            {
                _periodos = value;
                RaisePropertyChanged("Periodos");
            }
        }

        public ObservableCollection<mTiposGenericos> Situações
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_CAmbulante_Situacao WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public string Outros
        {
            get { return _outros; }
            set
            {
                _outros = value;
                RaisePropertyChanged("Outros");
            }
        }

        public string Situacao
        {
            get { return _situacao; }
            set
            {
                _situacao = value;
                RaisePropertyChanged("Situacao");
            }
        }

        public bool IsTenda
        {
            get { return _istenda; }
            set
            {
                _istenda = value;
                RaisePropertyChanged("IsTenda");
            }
        }

        public bool IsVeiculo
        {
            get { return _isveiculo; }
            set
            {
                _isveiculo = value;
                RaisePropertyChanged("IsVeiculo");
            }
        }

        public bool IsTrailer
        {
            get { return _istrailer; }
            set
            {
                _istrailer = value;
                RaisePropertyChanged("IsTrailer");
            }
        }

        public bool IsCarrinho
        {
            get { return _iscarrinho; }
            set
            {
                _iscarrinho = value;
                RaisePropertyChanged("IsCarrinho");
            }
        }

        public bool IsOutros
        {
            get { return _isoutros; }
            set
            {
                _isoutros = value;
                RaisePropertyChanged("IsOutros");
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

        public Visibility BlackBox
        {
            get { return _blackbox; }
            set
            {
                _blackbox = value;
                RaisePropertyChanged("BlackBox");
            }
        }

        public Visibility TemLicencaView
        {
            get { return _temlicencaview; }
            set
            {
                _temlicencaview = value;
                RaisePropertyChanged("TemLicencaView");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandPrint
        {
            get
            {
                if (_commandprint == null)
                    _commandprint = new RelayCommand(p =>
                    {

                        FlowDocument _flow = (FlowDocument)p;

                        try
                        {
                            StartProgress = true;
                            BlackBox = Visibility.Visible;

                            // paginador
                            //_flow.PageHeight = 768;
                            //_flow.PageWidth = 1104;
                            //_flow.ColumnWidth = 1104;
                            IDocumentPaginatorSource idocument = _flow as IDocumentPaginatorSource;
                            idocument.DocumentPaginator.ComputePageCountAsync();

                            //Now print using PrintDialog
                            var pd = new PrintDialog();
                            //pd.UserPageRangeEnabled = true;
                            //pd.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;

                            if (pd.ShowDialog().Value)
                                pd.PrintDocument(idocument.DocumentPaginator, "Printing....");

                            StartProgress = false;
                            BlackBox = Visibility.Collapsed;
                        }
                        catch (Exception ex)
                        {
                            StartProgress = false;
                            BlackBox = Visibility.Collapsed;
                            MessageBox.Show(ex.Message);
                        }

                    });

                return _commandprint;
            }
        }

        public ICommand CommandSair
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

        #region Construtor
        public vmProfile()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "PERFIL";
            BlackBox = Visibility.Collapsed;
            StartProgress = false;
            AsyncMostrarDados(AreaTransferencia.CadastroAmbulante);
        }
        #endregion

        #region Functions
        private void AsyncMostrarDados(string _cca)
        {
            Task<mAmbulante>.Factory.StartNew(() => mdatacm.GetCAmbulante(_cca))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        try
                        {

                            if (task.Result != null)
                            {

                                Ambulante = task.Result;

                                Situacao = Situações[Ambulante.Situacao].Nome;

                                string[] _atv = task.Result.Atividades.Split(';');

                                foreach (string atv in _atv)
                                {
                                    if (atv != string.Empty)
                                        Atividades.Add(atv);
                                }

                                if (task.Result.TipoInstalacoes.Contains("TENDA;"))
                                    IsTenda = true;

                                if (task.Result.TipoInstalacoes.Contains("VEÍCULO;"))
                                    IsVeiculo = true;

                                if (task.Result.TipoInstalacoes.Contains("CARRINHO;"))
                                    IsCarrinho = true;

                                if (task.Result.TipoInstalacoes.Contains("TRAILER;"))
                                    IsTrailer = true;

                                if (task.Result.TipoInstalacoes.Contains("OUTROS"))
                                {
                                    IsOutros = true;

                                    string input = task.Result.TipoInstalacoes;
                                    string[] testing = System.Text.RegularExpressions.Regex.Matches(input, @"\[(.+?)\]")
                                                                .Cast<System.Text.RegularExpressions.Match>()
                                                                .Select(s => s.Groups[1].Value).ToArray();

                                    Outros = testing[0];
                                }

                                string[] _periodos = task.Result.PeridoTrabalho.Split('/');

                                foreach (string p in _periodos)
                                {
                                    if (p != string.Empty)
                                        Periodos.Add(p);
                                }

                                if (Ambulante.TemLicenca)
                                    TemLicencaView = Visibility.Visible;
                                else
                                    TemLicencaView = Visibility.Collapsed;

                            }                            

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(string.Format("Erro '{0}' inesperado, informe o Suporte!", ex.Message), "Sim.Alerta!");
                        }
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

    }
}
