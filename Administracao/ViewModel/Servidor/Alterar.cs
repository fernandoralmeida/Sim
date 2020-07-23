
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

    using Sim.Mvvm.Helpers.Commands;
    using Sim.Modulos.Portarias.Model;
    using Sim.Mvvm.Helpers.Notifiers;
    using Mvvm.Helpers.Observers;
    using Model;

    public class Alterar : Novo
    {
        #region Propriedades
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
        #endregion

        #region Comandos
        public ICommand CommandAlterar => new RelayCommand(p => {

            if (AlterarDados().IsCompleted && _save_is_ok == true) ns.GoBack();

        });

        public ICommand CommandExcluir => new RelayCommand(p => {

            if (MessageBox.Show("Escluir Resgitro?", "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (Excluir().IsCompleted && _save_is_ok == true) ns.GoBack();
            }

        });
        #endregion

        #region Construtor
        public Alterar()
        {
            GlobalNavigation.Pagina = "ALTERAR SERVIDOR";
            ns = GlobalNavigation.NavService;
            BlackBox = Visibility.Collapsed;
            ShowMsgBox = Visibility.Collapsed;
            Cargos = ListarCargos().Result;
            GetServidor(GlobalNavigation.Parametro).Wait();
        }
        #endregion

        #region Metodos
        private Task AlterarDados()
        {
            var t = Task.Factory.StartNew(() =>
            {
                _save_is_ok = new RepositorioServidores().Alterar(Servidor);
            });
            t.Wait();
            return t;
        }

        private Task GetServidor(string p)
        {
            var t = Task.Factory.StartNew(() => {
                Servidor = new RepositorioServidores().Indice(p);
            });

            t.Wait();

            return t;
        }

        private Task Excluir()
        {
            var t = Task.Factory.StartNew(() =>
            {
                _save_is_ok = new RepositorioServidores().Excluir(Servidor.Indice);
            });
            t.Wait();
            return t;
        }
        #endregion
    }
}
