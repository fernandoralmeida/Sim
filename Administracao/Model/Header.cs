using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Modulos.Administracao.Model
{

    using Sim.Mvvm.Helpers.Notifiers;
    using Sim.Common.Helpers;

    class Header : NotifyProperty
    {
        private string _nameorg;
        private string _slogan;
        private string _deporg;
        private string _setororg;

        public string NameOrg
        {
            get { return _nameorg; }
            set
            {
                _nameorg = value;
                RaisePropertyChanged("NameOrg");
            }
        }

        public string SloganOrg
        {
            get { return _slogan; }
            set
            {
                _slogan = value;
                RaisePropertyChanged("SloganOrg");
            }
        }

        public string DepOrg
        {
            get { return _deporg; }
            set
            {
                _deporg = value;
                RaisePropertyChanged("DepOrg");
            }
        }

        public string SetorOrg
        {
            get { return _setororg; }
            set
            {
                _setororg = value;
                RaisePropertyChanged("SetorOrg");
            }
        }

        public Header()
        {
            NameOrg = XmlFile.Listar("sim.apps", "App", "Organizacao", "Name")[0];
            SloganOrg = XmlFile.Listar("sim.apps", "App", "Organizacao", "Slogan")[0];
            DepOrg = XmlFile.Listar("sim.apps", "App", "Organizacao", "Departamento")[0];
            SetorOrg = XmlFile.Listar("sim.apps", "App", "Organizacao", "Setor")[0];
        }
    }
}
