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

    public class Secretarias : NotifyProperty
    {
        #region Declaracoes
        ObservableCollection<Model.Secretarias> _setores = new ObservableCollection<Model.Secretarias>();
        Model.Secretarias _sec = new Model.Secretarias();
        Visibility _insetON = Visibility.Collapsed;
        #endregion

        #region Propriedades
        public ObservableCollection<Model.Secretarias> ListaSec 
        { 
            get { return _setores; }
            set { _setores = value; RaisePropertyChanged("ListaSec"); }
        }

        public Model.Secretarias Sec
        {
            get { return _sec; }
            set { _sec = value; RaisePropertyChanged("Sec"); }
        }

        public Visibility InsetON
        {
            get { return _insetON; }
            set { _insetON = value; RaisePropertyChanged("InsetON"); }
        }
        #endregion

        #region Comandos
        public ICommand CommandRefresh => new RelayCommand(p => {
            if (Sec.Secretaria == string.Empty || Sec.Secretaria == null || Sec.Secretaria == "...")
                Listar("%");
            else
                Listar(Sec.Secretaria);
        });

        public ICommand CommandShow => new RelayCommand(p => {
            InsetON = Visibility.Visible;
        });

        public ICommand CommandSave => new RelayCommand(p => {

            Sec.Ativo = true;
            if (Gravar().IsCompleted == true)
            {
                InsetON = Visibility.Collapsed;
                Listar(Sec.Secretaria);
            }
        });

        public ICommand CommandAtivoYesNo => new RelayCommand(p => {

            var objetos = (object[])p;
            var bloqueado = (System.Windows.Controls.CheckBox)objetos[1];

            var cod = (int)objetos[0];
            var yesno = (bool)bloqueado.IsChecked;

            if (AtivarYesNo(cod, yesno).IsCompleted == true)
            {
                Listar(Sec.Secretaria);
            }
        });

        public ICommand CommandCancelar => new RelayCommand(p => {
            InsetON = Visibility.Collapsed;
        });
        #endregion

        #region Construtor
        public Secretarias()
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
            var t = Task<ObservableCollection<Model.Secretarias>>.Factory.StartNew(() => {
                return ListaSec = new RepositorioSecretarias().Secretarias(_param);
            });
            t.Wait();
            return t;
        }

        private Task Gravar()
        {
            var t = Task.Factory.StartNew(() =>
            {
                new RepositorioSecretarias().Gravar(Sec);
            });
            t.Wait();
            return t;
        }


        private Task AtivarYesNo(int _cod, bool _yesno)
        {
            var t = Task.Factory.StartNew(() =>
            {
                new RepositorioSecretarias().AtivarYesNo(_cod, _yesno);
            });
            t.Wait();
            return t;
        }
        #endregion
    }
}
