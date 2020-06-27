using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using System.Threading.Tasks;

namespace Sim.Modulos.Administracao.ViewModel
{

    using Mvvm.Helpers.Commands;
    using Mvvm.Helpers.Notifiers;
    using Mvvm.Helpers.Observers;
    using Model;


    class Avaliacao : NotifyProperty
    {

        public Avaliacao()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Modulo = "GOVERNO";
            GlobalNavigation.SubModulo = "AVALIAÇÃO FUNCIONAL";
            GlobalNavigation.Pagina = string.Empty;
            GlobalNavigation.BrowseBack = Visibility.Visible;
            BlackBox = Visibility.Collapsed;
            ShowMsgBox = Visibility.Collapsed;
            PreviewBox = Visibility.Collapsed;
            DataI = DateTime.Now;
        }

        #region Declarations
        private NavigationService ns;
        private ObservableCollection<Avaliacoes> listaravaliacoes = new ObservableCollection<Avaliacoes>();
        private Avaliacoes avls = new Avaliacoes();
        private Visibility _blackbox;
        private Visibility _msgbox;
        private Visibility _previwbox;
        private DateTime _datai;
        private int _pontos;
        #endregion

        #region Properties
        public ObservableCollection<Avaliacoes> ListarAvaliacoes
        {
            get { return listaravaliacoes; }
            set { listaravaliacoes = value;
                RaisePropertyChanged("ListarAvaliacoes");
            }
        }

        public Avaliacoes Avls { get { return avls; } set { avls = value; RaisePropertyChanged("Avls"); } }

        public int Pontos
        {
            get { return _pontos; }
            set
            {
                _pontos = value;                
                RaisePropertyChanged("Pontos");
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
        public Visibility PreviewBox
        {
            get { return _previwbox; }
            set
            {
                _previwbox = value;
                RaisePropertyChanged("PreviewBox");
            }
        }        
        public DateTime DataI
        {
            get { return _datai; }
            set { _datai = value;
                ListarAvaliacoes = ListarAvaliacoesHoje().Result;
                RaisePropertyChanged("DataI");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandDataNext => new RelayCommand(p=> { DataI = DataI.AddDays(1); });

        public ICommand CommandDataPrev => new RelayCommand(p => { DataI = DataI.AddDays(-1); });

        public ICommand CommandEdit => new RelayCommand(p => {

            PreviewBox = Visibility.Visible;

            var t = Task.Factory.StartNew(() => {
                Avls = new RepositorioAvaliacoes().Indice((int)p);
                Pontos = Avls.Pontos;
            });

            t.Wait();        
        });

        public ICommand CommandClosePreview => new RelayCommand(p => { PreviewBox = Visibility.Collapsed; });

        public ICommand CommandGravar => new RelayCommand(p => {


            Avls.Pontos = Pontos;

            var t = Task.Factory.StartNew(() => {
                
                new RepositorioAvaliacoes().Alterar(Avls);

            });

            t.Wait();

            if (t.IsCompleted)
            {
                PreviewBox = Visibility.Collapsed;
                ListarAvaliacoes = ListarAvaliacoesHoje().Result;
            }

        });

        public ICommand CommandAutoPreencher => new RelayCommand(p => {

            if (Pontos >= 0 && Pontos <= 225)
            {
                Avls.Resultado = "INSUFICIENTE";
                Avls.DescricaoResultado = "REPROVADO FAIXA IV";
            }

            else if (Pontos >= 226 && Pontos <= 350)
            {
                Avls.Resultado = "INSUFICIENTE";
                Avls.DescricaoResultado = "REPROVADO FAIXA III";
            }

            else if (Pontos >= 351 && Pontos <= 500)
            {
                Avls.Resultado = "SUFICIENTE";
                Avls.DescricaoResultado = "APROVADO";
            }

            else if (Pontos >= 501 && Pontos <= 600)
            {
                Avls.Resultado = "SUFICIENTE";
                Avls.DescricaoResultado = "APROVADO COM PROGRESSÃO";
            }

        });
        #endregion

        #region Functions
        private Task<ObservableCollection<Avaliacoes>> ListarAvaliacoesHoje()
        {
            var t = Task<ObservableCollection<Avaliacoes>>.Factory.StartNew(() => new RepositorioAvaliacoes().PorData(DataI));
            {

            };
            t.Wait();
            return t;
        }

        #endregion
    }
}
