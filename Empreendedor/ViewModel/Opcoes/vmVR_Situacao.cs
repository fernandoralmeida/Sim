using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Sim.Modulos.Empreendedor.ViewModel.Opcoes
{

    using Model;
    using Mvvm.Helpers.Notifiers;

    class vmVR_Situacao : NotifyProperty
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

        public vmVR_Situacao()
        {
            Listar = new mData().Tipos(@"SELECT * FROM SDT_SE_Viabilidade_Situacao WHERE (Ativo = True) ORDER BY Valor");
        }

        #endregion
    }
}
