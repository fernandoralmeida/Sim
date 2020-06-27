using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Sim.Modulos.Portarias.ViewModel
{

    using Mvvm.Helpers.Commands;
    using Mvvm.Helpers.Notifiers;
    using Mvvm.Helpers.Observers;
    using Model;
    using Sql;
    using Accounts;

    class vmConfig : NotifyProperty
    {
        #region Declarations

        private List<mPages> _pagelist = new List<mPages>();
        private Uri _pageselected;
        private Uri _comebackpage;

        #endregion

        #region Properties

        public List<mPages> PageList
        {
            get { return _pagelist; }
            set
            {
                _pagelist = value;
                RaisePropertyChanged("PageList");
            }
        }

        public Uri PageSelected
        {
            get { return _pageselected; }
            set
            {
                _pageselected = value;
                RaisePropertyChanged("PageSelected");
            }
        }

        public Uri ComeBackPage
        {
            get { return _comebackpage; }
            set
            {
                _comebackpage = value;
                RaisePropertyChanged("ComeBackPage");
            }
        }

        #endregion

        public vmConfig()
        {
            PageList.Clear();
            PageList.Add(new mPages() { Name = "Pdfs", Link = "Pages/pLinkPdf.xaml" });

            if (AccountOn.Acesso != 0)
            {

                if (AccountOn.Acesso == (int)AccountAccess.Master)
                {
                    PageList.Add(new mPages() { Name = "Classificações", Link = "Pages/pClassf.xaml" });
                }

                else
                {

                    foreach (Accounts.Model.mSubModulos m in AccountOn.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.Legislacao)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Operador)
                            {
                                PageList.Add(new mPages() { Name = "Classificações", Link = "Pages/pClassf.xaml" });
                            }
                        }
                    }
                }
            }

            PageSelected = new Uri(PageList[0].Link, UriKind.Relative);
        }

    }
}
