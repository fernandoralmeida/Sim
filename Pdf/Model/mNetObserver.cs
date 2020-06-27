using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Pdf.Model
{

    using Mvvm.Helpers.Notifiers;

    public class mNetObserver : GlobalNotifyProperty
    {
        private static string _speed;
        private static int _ipercent;
        private static string _percent;
        private static string _downloaded;
        private static bool _downloadcomplete;

        public static string Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                OnGlobalPropertyChanged("Speed");
            }
        }

        public static string Percent
        {
            get { return _percent; }
            set
            {
                _percent = value;
                OnGlobalPropertyChanged("Percent");
            }
        }

        public static string Downloaded
        {
            get { return _downloaded; }
            set
            {
                _downloaded = value;
                OnGlobalPropertyChanged("Downloaded");
            }
        }

        public static int IntPercent
        {
            get { return _ipercent; }
            set
            {
                _ipercent = value;
                OnGlobalPropertyChanged("IntPercent");
            }
        }

        public static bool DonwloadComplete
        {
            get { return _downloadcomplete; }
            set { _downloadcomplete = value;
            OnGlobalPropertyChanged("DonwloadComplete");
            }
        }
    }
}
