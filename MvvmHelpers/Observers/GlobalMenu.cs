using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Sim.Mvvm.Helpers.Observers
{
    using Notifiers;

    public class GlobalMenu : GlobalNotifyProperty
    {
        private static Menu _menucontent;
        /// <summary>
        /// Get or Set Current ViewContent for MainWindow
        /// </summary>
        public static Menu MenuContent
        {
            get { return _menucontent; }
            set
            {
                _menucontent = value;
                OnGlobalPropertyChanged("MenuContent");
            }
        }
    }
}
