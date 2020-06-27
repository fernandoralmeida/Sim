using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Charts.Model
{
    using Sim.Mvvm.Helpers.Notifiers;

    public class mBarChart : NotifyProperty
    {
        private string _key;
        private double _value;
        private double _percent;
        private double _with;
        private double _total;

        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                RaisePropertyChanged("Key");
            }
        }

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
            }
        }

        public double Width
        {
            get { return _with; }
            set
            {
                _with = value;
                RaisePropertyChanged("Width");
            }
        }

        public double Percent
        {
            get { return _percent; }
            set
            {
                _percent = value;
                RaisePropertyChanged("Percent");
            }
        }

        public double Total
        {
            get { return _total; }
            set
            {
                _total = value;
                RaisePropertyChanged("Total");
            }
        }
    }
}
