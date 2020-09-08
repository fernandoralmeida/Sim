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


    public class Novo : NotifyProperty
    {
        #region Declaracoes
        private NavigationService ns;
        private Avaliacoes _avaliacao = new Avaliacoes();
        private Visibility _blackbox;
        private Visibility _msgbox;
        private ObservableCollection<string> _cargos = new ObservableCollection<string>();
        private DateTime __dataavaliacao = DateTime.Now;
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
        #endregion

        #region Comandos
        public ICommand CommandGravar => new RelayCommand(p => {
            if (Gravar().IsCompleted && _save_is_ok == true) ns.GoBack();
        });

        public ICommand CommandCancelar => new RelayCommand(p => {
            ns.GoBack();
        });

        public ICommand CommandGetServidor => new RelayCommand(p => {
            GetServidor(p.ToString());
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
        #endregion

        #region Construtor

        public Novo()
        {
            GlobalNavigation.Pagina = "NOVO";
            ns = GlobalNavigation.NavService;
            BlackBox = Visibility.Collapsed;
            ShowMsgBox = Visibility.Collapsed;
            Cargos = ListarCargos().Result;
            Avaliacao.Servidor.Admissao = new DateTime(2020, 01, 01);
        }

        #endregion

        #region Metodos

        private Task Gravar()
        {
            var t = Task.Factory.StartNew(() =>
            {
                _save_is_ok = new RepositorioAvaliacoes().Gravar(Avaliacao);
            });
            t.Wait();
            return t;
        }

        private Task<ObservableCollection<string>> ListarCargos()
        {
            var t = Task<ObservableCollection<string>>.Factory.StartNew(() => new RepositorioCargos().TodosCargosAtivos()); {
                
            };
            t.Wait();
            return t;
        }

        private Task GetServidor(string p)
        {
            var t = Task.Factory.StartNew(() => {
                Avaliacao.Servidor = new RepositorioServidores().Servidor(p);
            });

            t.Wait();            

            return t;
        }
        #endregion
    }
}
