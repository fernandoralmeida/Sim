using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Sim.Monitoring.ViewModel
{
    class GroupByCommand : ICommand
    {
        private ObservableCollection<string> groupingColumns;
        private string groupName;

        public GroupByCommand(ObservableCollection<string> groupingColumns, string groupName)
        {
            this.groupingColumns = groupingColumns;
            this.groupName = groupName;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        //public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.groupingColumns.Clear();
            this.groupingColumns.Add(this.groupName);
        }

        bool ICommand.CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        void ICommand.Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
