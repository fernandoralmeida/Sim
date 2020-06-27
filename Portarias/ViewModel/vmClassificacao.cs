using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Sim.Modulos.Portarias.ViewModel
{

    using Sim.Modulos.Portarias.Model;
    using Sim.Modulos.Portarias.Sql;

    public class vmClassificacao : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            return new mData().ListaGenerica(SqlCollections.Classi_Only_Non_Blocked).GetEnumerator();
        }

    }
}
