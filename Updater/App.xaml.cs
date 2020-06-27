using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Sim.Updater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        internal static string[] Args = Environment.GetCommandLineArgs();
        
        protected override void OnStartup(StartupEventArgs e)
        {
            foreach (string arg in e.Args)
            {
                Args[0] = e.Args[0];
            }
            base.OnStartup(e);
                        
        }
    }
}
