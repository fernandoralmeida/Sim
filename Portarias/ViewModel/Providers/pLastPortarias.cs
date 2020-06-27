using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;

namespace Sim.Modulos.Portarias.ViewModel.Providers
{
    using Model;
    using Mvvm.Helpers.Notifiers;

    class PLastPortarias : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {            
            return new mData().LastRows().GetEnumerator();
        }
    }
}
