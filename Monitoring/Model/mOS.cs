using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Monitoring.Model
{

    using Mvvm.Helpers.Notifiers;

    public class mOS : NotifyProperty
    {
        private string _version;
        private string _ram;
        private string _ramused;
        private List<string> _hdd;
        private string _hddused;
        private string _microprocessor;
        private string _microprocessorspeed;
        private string _microprocessorused;

        public string Version { get { return _version; }
            set { _version = value;
            RaisePropertyChanged("Version");
            }
        }

        public string Ram { get { return _ram; }
            set { _ram = value;
            RaisePropertyChanged("Ram");
            }
        }

        public string RamUsed { get { return _ramused; }
            set { _ramused = value;
            RaisePropertyChanged("RamUsed");
            }
        }

        public List<string> Hdd { get { return _hdd; }
            set
            { _hdd = new List<string>();
            _hdd = value;
            RaisePropertyChanged("Hdd");
            }
        }

        public string HddUsed { get { return _hddused; }
            set { _hddused = value;
            RaisePropertyChanged("HddUsed");
            }
        }

        public string MicroProcessor { get { return _microprocessor; }
            set { _microprocessor = value;
            RaisePropertyChanged("MicroProcessor");
            }
        }

        public string MicroProcessorSpeed { get { return _microprocessorspeed; }
            set { _microprocessorspeed = value;
            RaisePropertyChanged("MicroProcessorSpeed");
            }
        }

        public string MicroProcessorUsed { get { return _microprocessorused; }
            set { _microprocessorused = value;
            RaisePropertyChanged("MicroProcessorUsed");
            }
        }
    }
}
