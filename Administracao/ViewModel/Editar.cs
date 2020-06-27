using System;
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

    class Editar : NotifyProperty
    {
        #region Declaracoes
        private NavigationService ns;
        private Avaliacoes _avaliacao = new Avaliacoes();
        private Visibility _blackbox;
        private Visibility _msgbox;
        private ObservableCollection<string> _cargos = new ObservableCollection<string>();
        private DateTime __dataavaliacao;
        private bool _save_is_ok = false;
        #endregion

        #region Propriedades
        public Avaliacoes Avaliacao
        {
            get { return _avaliacao; }
            set
            {
                _avaliacao = value;
                RaisePropertyChanged("Avaliacao");
            }
        }

        public ObservableCollection<string> Cargos
        {
            get { return _cargos; }
            set
            {
                _cargos = value;
                RaisePropertyChanged("ListaCargos");
            }
        }

        public ObservableCollection<string> Situacao
        {
            get
            {
                return new ObservableCollection<string>()
                {
                    "...", "NORMAL", "AFASTADO"
                };
            }
        }

        public ObservableCollection<string> AnoParAnoImpar
        {
            get
            {
                return new ObservableCollection<string>()
                {
                    "...", "ANO PAR", "ANO ÍMPAR"
                };
            }
        }

        public ObservableCollection<string> Resultado
        {
            get
            {
                return new ObservableCollection<string>() {
                "...", "SUFICIENTE", "INSUFICIENTE"};
            }
        }

        public ObservableCollection<string> DescricaoResultado
        {
            get
            {
                return new ObservableCollection<string>() {
                "...", "APROVADO", "APROVADO COM PROGRESSÃO", "REPROVADO FAIXA III", "REPROVADO FAIXA IV"};
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

        public DateTime DataAvaliacao
        {
            get { return __dataavaliacao; }
            set
            {
                __dataavaliacao = value;
                Avaliacao.DataAvaliacao = value;
                RaisePropertyChanged("DataAvaliacao");
            }
        }
        #endregion

        #region Comandos
        public ICommand CommandGravar => new RelayCommand(p => {
            if (Gravar().IsCompleted && _save_is_ok == true) ns.GoBack();
        });

        public ICommand CommandCancelar => new RelayCommand(p => {
            ns.GoBack();
        });

        public ICommand CommandAutoPreencher => new RelayCommand(p => {

            if (Avaliacao.Pontos >= 0 && Avaliacao.Pontos <= 225)
            {
                Avaliacao.Resultado = "INSUFICIENTE";
                Avaliacao.DescricaoResultado = "REPROVADO FAIXA IV";
            }

            else if (Avaliacao.Pontos >= 226 && Avaliacao.Pontos <= 350)
            {
                Avaliacao.Resultado = "INSUFICIENTE";
                Avaliacao.DescricaoResultado = "REPROVADO FAIXA III";
            }

            else if (Avaliacao.Pontos >= 351 && Avaliacao.Pontos <= 500)
            {
                Avaliacao.Resultado = "SUFICIENTE";
                Avaliacao.DescricaoResultado = "APROVADO";
            }

            else if (Avaliacao.Pontos >= 501 && Avaliacao.Pontos <= 600)
            {
                Avaliacao.Resultado = "SUFICIENTE";
                Avaliacao.DescricaoResultado = "APROVADO COM PROGRESSÃO";
            }

        });

        public ICommand CommandExcluir => new RelayCommand(p => {

            if (MessageBox.Show("Escluir Resgitro?", "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (Excluir().IsCompleted && _save_is_ok == true) ns.GoBack();
            }

        });
        #endregion

        #region Construtor

        public Editar()
        {
            GlobalNavigation.Pagina = "EDITAR/EXCLUIR";
            ns = GlobalNavigation.NavService;
            BlackBox = Visibility.Collapsed;
            ShowMsgBox = Visibility.Collapsed;
            GetAvaliacao(GlobalNavigation.Parametro).Wait();
            Cargos = ListarCargos().Result;            
        }

        #endregion

        #region Metodos
        private Task GetAvaliacao(string param)
        {
            var t = Task.Factory.StartNew(() => {
                Avaliacao = new RepositorioAvaliacoes().Indice(Convert.ToInt32(param));                
            });

            t.Wait();

            return t;
        }

        private Task Gravar()
        {
            var t = Task.Factory.StartNew(() =>
            {
                _save_is_ok = new RepositorioAvaliacoes().Alterar(Avaliacao);
            });
            t.Wait();
            return t;
        }

        private Task Excluir()
        {
            var t = Task.Factory.StartNew(() =>
            {
                _save_is_ok = new RepositorioAvaliacoes().Excluir(Avaliacao.Indice);
            });
            t.Wait();
            return t;
        }

        private Task<ObservableCollection<string>> ListarCargos()
        {
            var t = Task<ObservableCollection<string>>.Factory.StartNew(() => new RepositorioCargos().TodosCargosAtivos());
            {

            };
            t.Wait();
            return t;
        }
        #endregion
    }
}
