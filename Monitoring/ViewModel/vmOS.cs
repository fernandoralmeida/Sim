using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Windows.Threading;

namespace Sim.Monitoring.ViewModel
{
    using Model;
    using Mvvm.Helpers.Notifiers;

    class vmOS : NotifyProperty
    {

        #region Declarations

        private mOS _mos = new mOS();        

        #endregion

        #region Properties

        public mOS OS { get { return _mos; }
            set { _mos = value;
            RaisePropertyChanged("OS");
            }
        }

        #endregion
        
        public vmOS() {

            DispatcherTimer dispTimer = new DispatcherTimer();
            dispTimer.Tick += new EventHandler(dispTimer_Tick);
            dispTimer.Interval = new TimeSpan(0, 0, 1);
            //dispTimer.Start();

            OperatingSystem OsEnv = Environment.OSVersion;
            ManagementObjectSearcher mObs = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");

            if (Environment.Is64BitOperatingSystem == true)
                OS.Version = OsEnv.VersionString.ToString() + " 64Bits";
            else
                OS.Version = OsEnv.VersionString.ToString() + " 32Bits";

            foreach (ManagementObject obj in mObs.Get())
            {                

                if(obj["name"] != null)
                    OS.MicroProcessor = obj["name"].ToString();

            }

            List<string> _hddlist = new List<string>();
            
            foreach (var hdd in System.IO.DriveInfo.GetDrives())
            {
                if (hdd.IsReady)
                    _hddlist.Add(string.Format("HDD: {0} {1} {2} {3:#.#,##}GB {4:#.#,##}GB",
                        hdd.VolumeLabel,
                        hdd.Name,
                        hdd.DriveFormat,
                        ((hdd.TotalSize / 1024) / 1024) / 1024,
                        ((hdd.TotalFreeSpace / 1024) / 1024) / 1024));
            }

            OS.Hdd = _hddlist;
            DisplayTotalRam();
        }

        void dispTimer_Tick(object sender, EventArgs e)
        {
            ManagementObjectSearcher mObs = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (ManagementObject obj in mObs.Get())
                if (obj["CurrentClockSpeed"] != null)
                    OS.MicroProcessorSpeed = string.Format("Speed: {0}MHz", obj["CurrentClockSpeed"].ToString());
        }

        private void DisplayTotalRam()
        {
            string Query = "SELECT MaxCapacity FROM Win32_PhysicalMemoryArray";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(Query);
            foreach (ManagementObject WniPART in searcher.Get())
            {
                UInt32 SizeinKB = Convert.ToUInt32(WniPART.Properties["MaxCapacity"].Value);
                UInt32 SizeinMB = SizeinKB / 1024;
                UInt32 SizeinGB = SizeinMB / 1024;
                OS.Ram = string.Format("Memória: {0}KB {1}MB {2}GB", SizeinKB, SizeinMB, SizeinGB);
            }
        }
    }
}
