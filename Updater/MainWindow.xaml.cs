using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Interop;

namespace Sim.Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();

        public MainWindow()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
            
            ni.Icon = Properties.Resources.sim_downloads;
            ni.Visible = true;
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };

            if (App.Args[0] == ":h")
            {
                this.WindowState = WindowState.Minimized;
                this.Hide();
            }

            if (App.Args[0] == ":tm")
            {
                this.Topmost = true;
            }

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Minimized)
                this.Hide();

            if (WindowState == WindowState.Normal)
                this.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            ni.Visible = false;
        }

    }
}
