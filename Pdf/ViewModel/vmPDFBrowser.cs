using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Pdf.ViewModel
{

    using Mvvm.Helpers.Notifiers;
    

    class vmPDFBrowser : NotifyProperty
    {
        #region Declarations

        private Uri _uri;
        //private string _url;

        #endregion

        #region Properties

        public Uri ShowPDF
        {
            get { return _uri; }
            set
            {
                _uri = value;
                RaisePropertyChanged("ShowPDF");
            }
        }

        #endregion

        public vmPDFBrowser(string url)
        {
            ShowPDF = new Uri(url);
            System.Windows.MessageBox.Show(url + "\n" + "http://www.jau.sp.gov.br/painel/arquivos/jornal/Jornal_Oficial_740.pdf");

        }
    }
}
