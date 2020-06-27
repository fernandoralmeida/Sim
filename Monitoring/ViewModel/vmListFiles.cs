using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Sim.Monitoring.ViewModel
{

    using Model;
    using Mvvm.Helpers.Notifiers;

    class vmListFiles : NotifyProperty
    {

        private ObservableCollection<mFiles> _files = new ObservableCollection<mFiles>();
        private ObservableCollection<string> groupingColumns = new ObservableCollection<string>();

        public ObservableCollection<mFiles> Files
        {
            get { return _files; }
        }

        public ObservableCollection<string> GroupingColumns
        {
            get { return this.groupingColumns; }
        }

        public ICommand GroupByExtension
        {
            get;
            private set;
        }

        public vmListFiles() {

            //var view = CollectionViewSource.GetDefaultView(Files);
            //GroupByExtension = new GroupByCommand(GroupingColumns, "Extension");

            //GroupByExtension.CanExecuteChanged += GroupByExtension_CanExecuteChanged;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Files);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Extension");
            view.GroupDescriptions.Add(groupDescription);

            /*
            GroupingColumns.CollectionChanged += (s, e) =>
            {
                view.GroupDescriptions.Clear();
                foreach (var groupName in GroupingColumns)
                {
                    view.GroupDescriptions.Add(new PropertyGroupDescription(groupName));
                }
            };*/
        }

    }
}
