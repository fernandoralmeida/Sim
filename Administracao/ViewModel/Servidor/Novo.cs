using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sim.Modulos.Administracao.ViewModel.Servidor
{

    using Sim.Modulos.Portarias.Model;
    using Sim.Mvvm.Helpers.Notifiers;
    using Sim.Mvvm.Helpers.Commands;
    using Mvvm.Helpers.Observers;
    using Model;

    public class Novo : NotifyProperty
    {
        #region Declaracoes
        public NavigationService ns;
        private Servidores _servidores = new Servidores();
        private Visibility _blackbox;
        private Visibility _msgbox;
        private ObservableCollection<string> _cargos = new ObservableCollection<string>();
        public bool _save_is_ok = false;
        #endregion

        #region Propriedades
        public Servidores Servidor
        {
            get { return _servidores; }
            set
            {
                _servidores = value;
                RaisePropertyChanged("Servidor");
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

        #endregion

        #region Comandos
        public ICommand CommandGravar => new RelayCommand(p => {
            if (Gravar().IsCompleted && _save_is_ok == true) ns.GoBack();
        });

        public ICommand CommandCancelar => new RelayCommand(p => {
            ns.GoBack();
        });
        #endregion

        #region Construtor

        public Novo()
        {
            GlobalNavigation.Pagina = "INCLUIR SERVIDOR";
            ns = GlobalNavigation.NavService;
            BlackBox = Visibility.Collapsed;
            ShowMsgBox = Visibility.Collapsed;
            Cargos = ListarCargos().Result;
            Servidor.Admissao = new DateTime(2020, 01, 01);
            Servidor.Ativo = true;
        }

        #endregion

        #region Metodos

        private Task Gravar()
        {
            var t = Task.Factory.StartNew(() =>
            {
                _save_is_ok = new RepositorioServidores().Gravar(Servidor);
            });
            t.Wait();
            return t;
        }

        public Task<ObservableCollection<string>> ListarCargos()
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
