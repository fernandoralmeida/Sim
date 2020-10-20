using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Modulos.Administracao.ViewModel
{

    using Sim.Mvvm.Helpers.Observers;

    class Opcoes
    {
        public Opcoes()
        {
            GlobalNavigation.Pagina = "NOVO";
        }
    }
}
