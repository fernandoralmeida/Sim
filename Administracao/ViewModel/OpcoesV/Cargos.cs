using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Sim.Modulos.Administracao.ViewModel.OpcoesV
{
    using Sim.Mvvm.Helpers.Notifiers;
    using Sim.Mvvm.Helpers.Observers;
    using Model;
    using Sim.Mvvm.Helpers.Commands;

    public class Cargos : NotifyProperty
    {
        #region Declaracoes
        ObservableCollection<Model.Cargos> _cargos = new ObservableCollection<Model.Cargos>();
        Model.Cargos _cargo = new Model.Cargos();
        Visibility _insetON = Visibility.Collapsed;
        #endregion

        #region Propriedades
        public ObservableCollection<Model.Cargos> ListaCargos
        {
            get { return _cargos; }
            set { _cargos = value; RaisePropertyChanged("ListaCargos"); }
        }

        public Model.Cargos Cargo
        {
            get { return _cargo; }
            set { _cargo = value; RaisePropertyChanged("Cargo"); }
        }

        public Visibility InsetON
        {
            get { return _insetON; }
            set { _insetON = value; RaisePropertyChanged("InsetON"); }
        }
        #endregion

        #region Comandos
        public ICommand CommandRefresh => new RelayCommand(p => {
            if (Cargo.Cargo == string.Empty || Cargo.Cargo == null || Cargo.Cargo == "...")
                Listar("%");
            else
                Listar(Cargo.Cargo);
        });

        public ICommand CommandShow => new RelayCommand(p => {
            InsetON = Visibility.Visible;
        });

        public ICommand CommandSave => new RelayCommand(p => {

            Cargo.Ativo = true;
            if (Gravar().IsCompleted == true)
            {
                InsetON = Visibility.Collapsed;
                Listar(Cargo.Cargo);
            }
        });

        public ICommand CommandAtivoYesNo => new RelayCommand(p => {

            var objetos = (object[])p;
            var bloqueado = (System.Windows.Controls.CheckBox)objetos[1];

            var cod = (int)objetos[0];
            var yesno = (bool)bloqueado.IsChecked;

            if (AtivarYesNo(cod, yesno).IsCompleted == true)
            {
                Listar(Cargo.Cargo);
            }
        });

        public ICommand CommandCancelar => new RelayCommand(p => {
            InsetON = Visibility.Collapsed;
        });
        #endregion

        #region Construtor
        public Cargos()
        {
            InsetON = Visibility.Collapsed;
            Listar("%");
        }
        #endregion

        #region Metodos e Funcoes
        /// <summary>
        /// Task que retorna lista dos setores
        /// </summary>
        /// <param name="_param">filtra setores por nome de secretaria</param>
        /// <returns></returns>
        private Task Listar(string _param)
        {
            var t = Task<ObservableCollection<Model.Cargos>>.Factory.StartNew(() => {
                return ListaCargos = new RepositorioCargos().Cargos(_param);
            });
            t.Wait();
            return t;
        }

        private Task Gravar()
        {
            var t = Task.Factory.StartNew(() =>
            {
                new RepositorioCargos().Gravar(Cargo);
            });
            t.Wait();
            return t;
        }


        private Task AtivarYesNo(int _cod, bool _yesno)
        {
            var t = Task.Factory.StartNew(() =>
            {
                new RepositorioCargos().AtivarYesNo(_cod, _yesno);
            });
            t.Wait();
            return t;
        }
        #endregion
    }
}
