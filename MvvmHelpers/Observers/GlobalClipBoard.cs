using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Mvvm.Helpers.Observers
{

    using Notifiers;

    public class GlobalClipBoard : GlobalNotifyProperty
    {
        private static string _area_tranferencia;

        public static string AreaTranferencia
        {
            get { return _area_tranferencia; }
            set
            {
                _area_tranferencia = value;
                OnGlobalPropertyChanged("AreaTranferencia");
            }
        }
    }
}
