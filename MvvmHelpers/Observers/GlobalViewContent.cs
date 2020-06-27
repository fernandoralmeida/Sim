using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Sim.Mvvm.Helpers.Observers
{
    using Sim.Mvvm.Helpers.Notifiers;
    /// <summary>
    /// Observa se o conteudo da viewcontent mudou.
    /// </summary>
    public class GlobalViewContent : GlobalNotifyProperty
    {
        private static ContentControl _viewcontent;

        /// <summary>
        /// Get or Set Current ViewContent for MainWindow
        /// </summary>
        public static ContentControl Content
        {
            get { return _viewcontent; }
            set
            {
                _viewcontent = value;
                OnGlobalPropertyChanged("Content");
            }
        }

        /// <summary>
        /// Anula o controle
        /// </summary>
        public static void Clear()
        {
            Content = null;       
            GC.Collect();
        }
    }
}
