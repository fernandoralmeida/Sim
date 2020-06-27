using System;
using System.Collections.Generic;

namespace Sim.Modulos.Accounts.Model
{

    public class mAccount : mUser
    {
        #region Declarations
        private string _thema;
        private string _color;
        private mContas _conta = new mContas();
        private List<mModulos> _modulos = new List<mModulos>();
        private List<mSubModulos> _submodulos = new List<mSubModulos>();
        private mRegistroAcesso _registro = new mRegistroAcesso();
        private bool _autenticado = false;
        #endregion

        #region Properties
        public string Thema
        {
            get { return _thema; }
            set
            {
                _thema = value;
                RaisePropertyChanged("Thema");
            }
        }

        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                RaisePropertyChanged("Color");
            }
        }

        public mContas Conta
        {
            get { return _conta; }
            set
            {
                _conta = value;
                RaisePropertyChanged("Conta");
            }
        }

        public List<mModulos> Modulos
        {
            get { return _modulos; }
            set
            {
                _modulos = value;
                RaisePropertyChanged("Modulos");
            }
        }

        public List<mSubModulos> SubModulos
        {
            get { return _submodulos; }
            set
            {
                _submodulos = value;
                RaisePropertyChanged("SubModulos");
            }
        }

        public mRegistroAcesso Registro
        {
            get { return _registro; }
            set
            {
                _registro = value;
                RaisePropertyChanged("Registro");
            }
        }

        public bool Autenticado
        {
            get { return _autenticado; }
            set
            {
                _autenticado = value;
                RaisePropertyChanged("Autenticado");
            }
        }
        #endregion

        #region Functions
        #endregion
    }
}
