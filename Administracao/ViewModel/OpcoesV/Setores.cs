using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public class Setores : NotifyProperty
    {
        #region Declaracoes
        ObservableCollection<Model.Setores> _setores = new ObservableCollection<Model.Setores>();
        Model.Setores _setor = new Model.Setores();
        Visibility _insetON = Visibility.Collapsed;
        #endregion

        #region Propriedades
        public ObservableCollection<Model.Setores> ListaSetores
        {
            get { return _setores; }
            set { _setores = value; RaisePropertyChanged("ListaSetores"); }
        }

        public Model.Setores Setor
        {
            get { return _setor; }
            set { _setor = value; RaisePropertyChanged("Setor"); }
        }

        public Visibility InsetON
        {
            get { return _insetON; }
            set { _insetON = value; RaisePropertyChanged("InsetON"); }
        }
        #endregion

        #region Comandos
        public ICommand CommandRefresh => new RelayCommand(p => {
            if (Setor.Secretaria == string.Empty || Setor.Secretaria == null || Setor.Secretaria == "...")
                Listar("%");
            else
                Listar(Setor.Secretaria);
        });

        public ICommand CommandShow => new RelayCommand(p => {
            InsetON = Visibility.Visible;
        });

        public ICommand CommandSave => new RelayCommand(p => {

            Setor.Ativo = true;
            if (Gravar().IsCompleted == true)
            {
                InsetON = Visibility.Collapsed;
                Listar(Setor.Secretaria);
            }
        });

        public ICommand CommandCancelar => new RelayCommand(p => {
            InsetON = Visibility.Collapsed;
        });
        #endregion

        #region Construtor
        public Setores()
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
            var t = Task<ObservableCollection<Model.Setores>>.Factory.StartNew(() => {
                return ListaSetores = new RepositorioSetores().Setores(_param);
            });
            t.Wait();
            return t;
        }

        private Task Gravar()
        {
            var t = Task.Factory.StartNew(() =>
            {
                new RepositorioSetores().Gravar(Setor);
            });
            t.Wait();
            return t;
        }
        #endregion


    }
}
