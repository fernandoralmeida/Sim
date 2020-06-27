using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Sim.Monitoring.ViewModel
{
    using Model;
    using Mvvm.Helpers.Notifiers;
    

    class vmApps : NotifyProperty
    {
        private mApps _apps = new mApps();

        public mApps Apps { get { return _apps; }
            set { _apps = value;
            RaisePropertyChanged("Apps");
            }
        }

        public vmApps(string assemblyname)
        {            
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assemblyname);
            Apps.Name = "Nome: " + fvi.OriginalFilename;
            Apps.Version = "Versão: " + AssemblyName.GetAssemblyName(assemblyname).Version.ToString();
            Apps.Build = "Build: " + AssemblyName.GetAssemblyName(assemblyname).Version.Build;
            Apps.Revision = "Revisão: " + AssemblyName.GetAssemblyName(assemblyname).Version.Revision;
            Apps.Update = "Update: " + mXml.Listar("sim_update", "System", "Version", "Update")[0];
            Apps.Install = "Diretório: " + AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
