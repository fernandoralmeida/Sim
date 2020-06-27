using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Sim.Modulos.Empreendedor.ViewModel.Opcoes
{

    using Model;
    using Mvvm.Helpers.Notifiers;

    class vmPJ_UsoLocal : NotifyProperty
    {
        #region Declarations

        private ObservableCollection<mTiposGenericos> _listar = new ObservableCollection<mTiposGenericos>();

        #endregion

        #region Properties

        public ObservableCollection<mTiposGenericos> Listar
        {
            get { return _listar; }
            set
            {
                _listar = value;
                RaisePropertyChanged("Listar");
            }
        }
        #endregion

        #region Constructor

        public vmPJ_UsoLocal()
        {
            Listar = new mData().Tipos(@"SELECT * FROM SDT_SE_PJ_UsoLocal WHERE (Ativo = True) ORDER BY Valor");
        }

        #endregion
    }
}
