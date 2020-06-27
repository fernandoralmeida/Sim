using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Sim.Modulos.Empreendedor.ViewModel.Opcoes
{

    using Model;
    using Mvvm.Helpers.Notifiers;

    class vmCnaeMei : NotifyProperty
    {

        #region Declarations

        private ObservableCollection<mCNAE> _lista = new ObservableCollection<mCNAE>();

        #endregion

        #region Properties

        public ObservableCollection<mCNAE> Listar
        {
            get { return _lista; }
            set
            {
                _lista = value;
                RaisePropertyChanged("Listar");
            }
        }
        #endregion

        #region Constructor

        public vmCnaeMei()
        {
            Listar = new mData().CNAES(@"SELECT * FROM SDT_SE_PJ_CNAE_MEI WHERE (Ativo = True) ORDER BY Ocupacao");
        }

        #endregion

    }
}
