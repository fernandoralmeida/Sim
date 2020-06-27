using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using System.IO;

namespace Sim.Monitoring.ViewModel
{
    class vmListViewGroups
    {

        private CollectionViewSource _items;
        public ICollectionView Items
        {
            get
            {
                if (_items == null)
                {
                    List<string> exefiles = new List<string>();

                    var exenames = from fullFilename
                                        in Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.*", SearchOption.AllDirectories)
                                    select Path.GetFileName(fullFilename);

                    foreach (string filename in exenames)
                    {
                        FileInfo fi = new FileInfo(filename);
                        exefiles.Add(fi.Name);
                        _items = new CollectionViewSource { Source = new ObservableCollection<string>(exefiles) };
                        _items.GroupDescriptions.Add(new PropertyGroupDescription(fi.Extension));
                    }                                   

                }

                return _items.View;
            }
        }

    }
}
