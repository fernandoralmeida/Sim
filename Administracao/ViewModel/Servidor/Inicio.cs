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
    using Sim.Mvvm.Helpers.Notifiers;
    using Sim.Mvvm.Helpers.Commands;
    using Mvvm.Helpers.Observers;
    using Model;

    public class Inicio : NotifyProperty
    {
        #region Declaracoes
        private NavigationService ns;
        private ObservableCollection<Servidores> _servidores = new ObservableCollection<Servidores>();
        private Visibility _blackbox;
        private Visibility _msgbox;
        private ObservableCollection<string> _cargos = new ObservableCollection<string>();
        private bool _save_is_ok = false;
        #endregion

        #region Propriedades
        public ObservableCollection<Servidores> ListaServidores
        {
            get { return _servidores; }
            set
            {
                _servidores = value;
                RaisePropertyChanged("ListaServidores");
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
        public ICommand CommandEdit => new RelayCommand(p => {
            ns.Navigate(new Uri("/Sim.Modulo.Administracao;component/View/Servidor/Editar.xaml", UriKind.RelativeOrAbsolute));
            GlobalNavigation.Parametro = p.ToString();//if (Gravar().IsCompleted && _save_is_ok == true) ns.GoBack();
        });

        public ICommand CommandCancelar => new RelayCommand(p => {
            ns.GoBack();
        });
        #endregion

        #region Construtor

        public Inicio()
        {
            GlobalNavigation.SubModulo = "SERVIDORES";
            GlobalNavigation.Pagina = string.Empty;
            ns = GlobalNavigation.NavService;
            BlackBox = Visibility.Collapsed;
            ShowMsgBox = Visibility.Collapsed;
            ListaServidores = ListarCargos().Result;
        }

        #endregion

        #region Metodos

        private Task<ObservableCollection<Servidores>> ListarCargos()
        {
            var t = Task<ObservableCollection<Servidores>>.Factory.StartNew(() => new RepositorioServidores().UltimosCadastros());
            {

            };
            t.Wait();
            return t;
        }
        #endregion
    }
}
