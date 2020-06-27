using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sim.Modulos.Portarias.ViewModel
{
    using Mvvm.Helpers.Notifiers;
    using Model;
    using Accounts;

    class vmMainpage : NotifyProperty
    {
        #region Declarations
        private ObservableCollection<mPortaria> _lastdocs = new ObservableCollection<mPortaria>();
        private mData mdata = new mData();
        private bool _acesso = false;
        #endregion

        #region Properties
        public ObservableCollection<mPortaria> LastDocs
        {
            get { return _lastdocs; }
            set
            {
                _lastdocs = value;
                RaisePropertyChanged("LastDocs");
            }
        }

        public bool AcessoOK
        {
            get { return _acesso; }
            set
            {
                _acesso = value;
                RaisePropertyChanged("AcessoOK");
            }
        }
        #endregion

        #region Constructor
        public vmMainpage()
        {
            Mvvm.Helpers.Observers.GlobalNavigation.SubModulo = "PORTARIAS";
            Mvvm.Helpers.Observers.GlobalNavigation.Pagina = string.Empty;

            if (AccountOn.Acesso != 0)
            {

                if (AccountOn.Acesso == (int)AccountAccess.Master)
                    AcessoOK = true;

                else
                {

                    foreach (Accounts.Model.mSubModulos m in AccountOn.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.Portarias)
                        {
                            if (m.Acesso >= (int)SubModuloAccess.Operador)
                                AcessoOK = true;
                        }
                    }
                }
            }

            AsyncLastDocs();

        }
        #endregion

        #region Functions
        private void AsyncLastDocs()
        {

            Task.Factory.StartNew(() => mdata.LastRows())
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        LastDocs = task.Result;
                });

        }
        #endregion
    }
}
