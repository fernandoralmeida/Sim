using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Sim.Modulos.Portarias.ViewModel.vmPages
{
    using Mvvm.Helpers.Commands;
    using Mvvm.Helpers.Notifiers;
    using Model;
    using Sql;

    class vmLinkPdf : NotifyProperty
    {
        #region Declarations

        private bool _textboxenabled;
        private bool _textboxenabledjo;
        private bool _isfocusedpdf;
        private bool _isfocusedpdfjo;
        
        private string _rootpdf;
        private string _rootpdfrotulo;
        private string _pathjornaloficial;
        private string _buttonjornaloficial;

        private ICommand _commandupdaterootpdf;
        private ICommand _commandjornaloficial;
        #endregion

        #region Properties


        public bool TextBoxEnabled
        {
            get { return _textboxenabled; }
            set
            {
                _textboxenabled = value;
                RaisePropertyChanged("TextBoxEnabled");
            }
        }

        public bool TextBoxEnabledJO
        {
            get { return _textboxenabledjo; }
            set
            {
                _textboxenabledjo = value;
                RaisePropertyChanged("TextBoxEnabledJO");
            }
        }

        public bool IsFocusedPdf
        {
            get { return _isfocusedpdf; }
            set
            {
                _isfocusedpdf = value;
                RaisePropertyChanged("IsFocusedPdf");
            }
        }

        public bool IsFocusedPdfJo
        {
            get { return _isfocusedpdfjo; }
            set
            {
                _isfocusedpdfjo = value;
                RaisePropertyChanged("IsFocusedPdfJo");
            }
        }

        public string RootPDF
        {
            get { return _rootpdf; }
            set
            {
                _rootpdf = value;
                RaisePropertyChanged("RootPDF");
            }
        }

        public string PathJornalOficial
        {
            get { return _pathjornaloficial; }
            set
            {
                _pathjornaloficial = value;
                RaisePropertyChanged("PathJornalOficial");
            }
        }

        public string RootPDFRotulo
        {
            get { return _rootpdfrotulo; }
            set
            {
                _rootpdfrotulo = value;
                RaisePropertyChanged("RootPDFRotulo");
            }
        }

        public string ButtonJornalOficial
        {
            get { return _buttonjornaloficial; }
            set
            {
                _buttonjornaloficial = value;
                RaisePropertyChanged("ButtonJornalOficial");
            }
        }
        
        #endregion

        #region Commands

        public ICommand CommandUpdateRootPDF
        {
            get
            {
                if (_commandupdaterootpdf == null)
                    _commandupdaterootpdf = new DelegateCommand(ExecuteCommandUpdateRootPDF, null);

                return _commandupdaterootpdf;
            }
        }

        public ICommand CommandJornalOficial
        {
            get
            {
                if (_commandjornaloficial == null)
                    _commandjornaloficial = new DelegateCommand(ExecuteCommandJornalOficial, null);

                return _commandjornaloficial;
            }
        }

        #endregion

        #region Start Instance

        public vmLinkPdf()
        {
            RootPDF = mXml.Listar("sim.apps", "App", "PDF", "Portaria")[0];
            PathJornalOficial = mXml.Listar("sim.apps", "App", "PDF", "JOficial")[0];
            RootPDFRotulo = "Alterar Caminho";
            ButtonJornalOficial = "Alterar Caminho";
            TextBoxEnabled = false;
            TextBoxEnabledJO = false;
            IsFocusedPdf = false;
            IsFocusedPdfJo = false;
        }

        #endregion

        #region Methods

        private void ExecuteCommandUpdateRootPDF(object obj)
        {
            if (RootPDFRotulo == "Alterar Caminho")
            {
                TextBoxEnabled = true;
                RootPDFRotulo = "Salvar";
                IsFocusedPdf = true;
            }
            else
            {
                RootPDFRotulo = "Alterar Caminho";
                TextBoxEnabled = false;
                mXml.SaveXml(RootPDF, "Portaria");
                IsFocusedPdf = false;
            }
        }

        private void ExecuteCommandJornalOficial(object obj)
        {
            if (ButtonJornalOficial == "Alterar Caminho")
            {
                TextBoxEnabledJO = true;
                ButtonJornalOficial = "Salvar";
                IsFocusedPdfJo = true;
            }
            else
            {
                ButtonJornalOficial = "Alterar Caminho";
                TextBoxEnabledJO = false;
                mXml.SaveXml(PathJornalOficial, "JOficial");
                
                string tmp = mXml.Listar("sim.apps", "App", "PDF", "JOficial")[0];

                if (tmp == "...")
                    mXml.NovoElemento("sim.apps", "PDF", "JOficial", PathJornalOficial);

                IsFocusedPdfJo = false;
            }
        }

        #endregion
    }
}
