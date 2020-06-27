using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Sim.Mvvm.Helpers.Commands
{
    class NewWindow : INewWindow
    {
        #region Implementation of INewWindowFactory

        public void CreateNewWindow()
        {
            Window window = new Window
            {
                DataContext = null
            };
            window.Show();            
        }

        #endregion
    }
}
