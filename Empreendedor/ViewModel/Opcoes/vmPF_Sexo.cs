using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Sim.Modulos.Empreendedor.ViewModel.Opcoes
{

    using Model;
    using Mvvm.Helpers.Notifiers;

    class vmPF_Sexo : NotifyProperty
    {

        #region Declarations

        private ObservableCollection<mTiposGenericos> _lista = new ObservableCollection<mTiposGenericos>();

        #endregion

        #region Properties

        public ObservableCollection<mTiposGenericos> Listar
        {
            get { return _lista; }
            set
            {
                _lista = value;
                RaisePropertyChanged("Listar");
            }
        }
        #endregion
                
        #region Contructor

        public vmPF_Sexo()
        {
            Listar = new mData().Tipos(@"SELECT * FROM SDT_SE_PF_Sexo WHERE (Ativo = True) ORDER BY Valor");
        }

        #endregion
    }
}
