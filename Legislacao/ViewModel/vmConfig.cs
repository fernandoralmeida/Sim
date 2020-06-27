using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Sim.Modulos.Legislacao.ViewModel
{

    using Sim.Mvvm.Helpers.Commands;
    using Sim.Mvvm.Helpers.Notifiers;
    using Model;
    using SqlCommands;
    using Accounts;
    using Accounts.Model;

    class vmConfig : NotifyProperty
    {
        #region Declarations

        private List<mPages> _pagelist = new List<mPages>();
        private Uri _pageselected;

        #endregion

        #region Properties

        public List<mPages> PageList
        {
            get { return _pagelist; }
            set {
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

        #endregion

        public vmConfig()
        {
            Mvvm.Helpers.Observers.GlobalNavigation.Pagina = "OPÇÕES";
            PageList.Clear();
            PageList.Add(new mPages() { Name = "Pdfs", Link = "Pages/pLinkPdf.xaml" });

            if (AccountOn.Acesso != 0)
            {

                if (AccountOn.Acesso == (int)AccountAccess.Master)
                {
                    PageList.Add(new mPages() { Name = "Tipos", Link = "Pages/pTipos.xaml" });
                    PageList.Add(new mPages() { Name = "Classificações L", Link = "Pages/pClassLoLc.xaml" });
                    PageList.Add(new mPages() { Name = "Classificações D", Link = "Pages/pClassDec.xaml" });
                    PageList.Add(new mPages() { Name = "Situações", Link = "Pages/pSituacao.xaml" });
                    PageList.Add(new mPages() { Name = "Origens", Link = "Pages/pOrigem.xaml" });
                    PageList.Add(new mPages() { Name = "Autores", Link = "Pages/pAutor.xaml" });
                    PageList.Add(new mPages() { Name = "Ações", Link = "Pages/pAcao.xaml" });
                }

                else
                {

                    foreach (mSubModulos m in AccountOn.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.Legislacao)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Operador)
                            {
                                PageList.Add(new mPages() { Name = "Tipos", Link = "Pages/pTipos.xaml" });
                                PageList.Add(new mPages() { Name = "Classificações L", Link = "Pages/pClassLoLc.xaml" });
                                PageList.Add(new mPages() { Name = "Classificações D", Link = "Pages/pClassDec.xaml" });
                                PageList.Add(new mPages() { Name = "Situações", Link = "Pages/pSituacao.xaml" });
                                PageList.Add(new mPages() { Name = "Origens", Link = "Pages/pOrigem.xaml" });
                                PageList.Add(new mPages() { Name = "Autores", Link = "Pages/pAutor.xaml" });
                                PageList.Add(new mPages() { Name = "Ações", Link = "Pages/pAcao.xaml" });
                            }
                        }
                    }
                }
            }

            PageSelected = new Uri(PageList[0].Link, UriKind.Relative);
        }

    }
}
