using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Updater.Helpers
{
    class Observer : NotifyGlobal
    {
        private static Uri _uripage;

        /// <summary>
        /// 
        /// </summary>
        public static Uri UriPage
        {
            get { return _uripage; }
            set
            {
                _uripage = value;
                OnGlobalPropertyChanged("UriPage");
            }
        }
        /// <summary>
        /// Anula o controle
        /// </summary>
        public static void Clear()
        {
            UriPage = null;
            GC.Collect();
        }
    }
}
