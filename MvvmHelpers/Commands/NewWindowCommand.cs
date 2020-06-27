using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Sim.Mvvm.Helpers.Commands
{
    public class NewWindowCommand
    {
        private readonly INewWindow m_windowFactory;
        private ICommand m_openNewWindow;

        public NewWindowCommand(INewWindow windowFactory)
        {
            m_windowFactory = windowFactory;

            /**
             * * Would need to assign value to m_openNewWindow here, and associate the DoOpenWindow method
             * * to the execution of the command.
             * * */
            m_openNewWindow = null;
        }

        public void DoOpenNewWindow()
        {
            m_windowFactory.CreateNewWindow();
        }

        public ICommand OpenNewWindow { get { return m_openNewWindow; } }
    }
}
