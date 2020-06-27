using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace Sim.Modulos.Portarias.ViewModel
{
    using Sim.Mvvm.Helpers.Commands;
    using Sim.Mvvm.Helpers.Notifiers;

    class vmReportPreview
    {
        #region Declarations

        private ICommand _commandclose;

        #endregion

        #region Properties

        public IDocumentPaginatorSource DocumentPreview { get; set; }
        public Window WindowParent { get; set; }
        #endregion

        #region Commands

        public ICommand CommandClose
        {
            get
            {
                if (_commandclose == null)
                    _commandclose = new DelegateCommand(ExecuteCommandClose, null);

                return _commandclose;
            }
        }

        #endregion

        public vmReportPreview(string fileXps)
        {
            try
            {
                using (XpsDocument document = new XpsDocument(fileXps, System.IO.FileAccess.Read))
                {
                    DocumentPreview = document.GetFixedDocumentSequence();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(string.Format("{0}", ex.Message), "Sim.App.Aviso!");
            }
        }

        #region Methods

        private void ExecuteCommandClose(object obj)
        {
            //WindowParent.Close();
            vmPageObserver.ReportClear();
        }

        #endregion
    }
}
