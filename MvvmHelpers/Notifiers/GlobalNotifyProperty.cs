using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Sim.Mvvm.Helpers.Notifiers
{
    public abstract class GlobalNotifyProperty : NotifyProperty
    {
        public static event PropertyChangedEventHandler GlobalPropertyChanged = delegate { };

        public static void OnGlobalPropertyChanged(string propertyName)
        {
            GlobalPropertyChanged(
                typeof(GlobalNotifyProperty),
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
