using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IWshRuntimeLibrary;

namespace Sim.Updater.Model
{
    class mShortCut
    {
        public static void Createlnk(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "Sistema Modular"; // The description of the shortcut
            shortcut.IconLocation = Common.Folders.AppData_Sim + "\\" + "Sim.Apps.Icon.ico"; // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation; // The path of the file that will launch when the shortcut is run
            shortcut.WorkingDirectory = Common.Folders.AppData_Sim;
            shortcut.Save(); // Save the shortcut
        }
    }
}
