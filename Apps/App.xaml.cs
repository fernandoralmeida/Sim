using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Diagnostics;

namespace Sim.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

               
            base.OnStartup(e);

            try
            {
                System.Windows.Media.Color paleta = new System.Windows.Media.Color();

                //paleta = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF3399FF");

                paleta = System.Windows.Media.Color.FromRgb(0x1b, 0xa1, 0xe2);

                new ModernUI.Presentation.ThemeManager().Apply(paleta, "Light");

            }
            catch
            {
                Current.Resources.MergedDictionaries.Clear();
                new ModernUI.Presentation.ThemeManager().Apply(System.Windows.Media.Color.FromRgb(0x1b, 0xa1, 0xe2), "Light");
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            //StopProcess("Sim.App.Updater");
        }

        private void ExecuteProcess()
        {
            Process[] pname = Process.GetProcessesByName("Sim.App.Desktop");

            if (pname.Length == 0)
            {
                ProcessStartInfo pInfo = new ProcessStartInfo("Sim.App.Desktop.exe");
                pInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Process p = Process.Start(pInfo);
            }
        }

        private void StopProcess(string processname)
        {
            Process[] pname = Process.GetProcessesByName(processname);

            if (pname.Length > 0)
                pname[0].CloseMainWindow();
        }

    }
}
