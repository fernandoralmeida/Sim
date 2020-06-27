using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.App.Model
{
    class mPages
    {
        public static Uri SetingsPage = new Uri("/View/pToolsTheme.xaml", UriKind.RelativeOrAbsolute);
        public static Uri UserPage = new Uri("/Sim.Modulo.Autenticacao;component/View/ucProfile.xaml", UriKind.RelativeOrAbsolute);

        public string Name { get; set; }
        public string Link { get; set; }
    }
}
