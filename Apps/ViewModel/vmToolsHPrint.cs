using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Sim.App.ViewModel
{

    using Sim.Mvvm.Helpers.Commands;
    using Sim.Mvvm.Helpers.Notifiers;
    using Sim.Common.Helpers;

    class vmToolsHPrint : NotifyProperty
    {
        #region Declarations

        private string _nameorg;
        private string _slogan;
        private string _deporg;
        private string _setororg;
        private string _rotulocommandrelatory;
        private bool _textboxenabledorg;

        private bool _isfocusedname;
        private bool _isfocusedslg;
        private bool _isfocuseddep;
        private bool _isfocusedsetor;

        private ICommand _commandupdaterelatory;

        #endregion

        #region Properties

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

        public string RotuloCommandRelatory
        {
            get { return _rotulocommandrelatory; }
            set
            {
                _rotulocommandrelatory = value;
                RaisePropertyChanged("RotuloCommandRelatory");
            }
        }

        public bool TextBoxEnabledOrg
        {
            get { return _textboxenabledorg; }
            set
            {
                _textboxenabledorg = value;
                RaisePropertyChanged("TextBoxEnabledOrg");
            }
        }

        public bool IsFocusedName
        {
            get { return _isfocusedname; }
            set
            {
                _isfocusedname = value;
                RaisePropertyChanged("IsFocusedName");
            }
        }

        public bool IsFocusedSlg
        {
            get { return _isfocusedslg; }
            set
            {
                _isfocusedslg = value;
                RaisePropertyChanged("IsFocusedSlg");
            }
        }

        public bool IsFocusedDep
        {
            get { return _isfocuseddep; }
            set
            {
                _isfocuseddep = value;
                RaisePropertyChanged("IsFocusedDep");
            }
        }

        public bool IsFocusedSetor
        {
            get { return _isfocusedsetor; }
            set
            {
                _isfocusedsetor = value;
                RaisePropertyChanged("IsFocusedSetor");
            }
        }
        #endregion

        #region Commands

        public ICommand CommandUpdateRelatory
        {
            get
            {
                if (_commandupdaterelatory == null)
                    _commandupdaterelatory = new DelegateCommand(ExecuteCommandUpdateRelatory, null);

                return _commandupdaterelatory;
            }
        }

        #endregion

        #region Methods

        private void ExecuteCommandUpdateRelatory(object obj)
        {
            if (RotuloCommandRelatory == "Alterar Cabeçalho de Impressão")
            {
                TextBoxEnabledOrg = true;
                RotuloCommandRelatory = "Salvar Cabeçalho de Impressão";
                IsFocusedName = true;
            }
            else
            {
                RotuloCommandRelatory = "Alterar Cabeçalho de Impressão";
                TextBoxEnabledOrg = false;
                XmlFile.SaveXml("sim.apps", NameOrg, "Name");
                XmlFile.SaveXml("sim.apps", SloganOrg, "Slogan");
                XmlFile.SaveXml("sim.apps", DepOrg, "Departamento");
                XmlFile.SaveXml("sim.apps", SetorOrg, "Setor");
                IsFocusedDep = false;
                IsFocusedName = false;
                IsFocusedSlg = false;
                IsFocusedSetor = false;
            }
        }

        #endregion

        #region Constructor

        public vmToolsHPrint()
        {
            Mvvm.Helpers.Observers.GlobalNavigation.Pagina = "IMPRESSÃO";
            NameOrg = XmlFile.Listar("sim.apps", "App", "Organizacao", "Name")[0];
            SloganOrg = XmlFile.Listar("sim.apps", "App", "Organizacao", "Slogan")[0];
            DepOrg = XmlFile.Listar("sim.apps", "App", "Organizacao", "Departamento")[0];
            SetorOrg = XmlFile.Listar("sim.apps", "App", "Organizacao", "Setor")[0];
            RotuloCommandRelatory = "Alterar Cabeçalho de Impressão";
            TextBoxEnabledOrg = false;
            IsFocusedDep = false;
            IsFocusedName = false;
            IsFocusedSlg = false;
            IsFocusedSetor = false;
        }

        #endregion
    }
}
