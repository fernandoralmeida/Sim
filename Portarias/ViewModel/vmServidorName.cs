using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace Sim.Modulos.Portarias.ViewModel
{
    
    using Sim.Modulos.Portarias.Model;

    class vmServidorName : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            return new mData().ListName().GetEnumerator();
        }
    }
}
