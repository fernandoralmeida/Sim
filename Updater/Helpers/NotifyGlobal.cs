using System;
using System.ComponentModel;

namespace Sim.Updater.Helpers
{
    class NotifyGlobal : Notify
    {
        public static event PropertyChangedEventHandler GlobalPropertyChanged = delegate { };

        public static void OnGlobalPropertyChanged(string propertyName)
        {
            GlobalPropertyChanged(
                typeof(NotifyGlobal),
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
