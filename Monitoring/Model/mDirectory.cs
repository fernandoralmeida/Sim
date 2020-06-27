using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Monitoring.Model
{
    using Mvvm.Helpers.Notifiers;

    public class mDirectory : NotifyProperty
    {

        private string _installed;
        private List<string> _files;

        public string Installed { get { return _installed; }
            set { _installed = value;
            RaisePropertyChanged("Installed");
            }
        }

        public List<string> Files { get { return _files; }
            set { _files = value;
            RaisePropertyChanged("Files");
            }
        }

    }
}
