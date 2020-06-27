using System.Windows;
using System.Windows.Navigation;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Sim.App
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Declarations
        private Storyboard backgroundAnimation;
        #endregion

        #region Construtor

        public MainWindow()
        {
            InitializeComponent();

            Mvvm.Helpers.Observers.GlobalNavigation.NavService = _globalframe.NavigationService;

            this.DataContext = new ViewModel.vmMainWindow();            

            this.CommandBindings.Add(new CommandBinding(Microsoft.Windows.Shell.SystemCommands.CloseWindowCommand, OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(Microsoft.Windows.Shell.SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(Microsoft.Windows.Shell.SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(Microsoft.Windows.Shell.SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));

            ModernUI.Presentation.ThemeObserver.GlobalPropertyChanged += ThemeObserver_GlobalPropertyChanged;
        }
        
        #endregion

        #region Methods

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            Microsoft.Windows.Shell.SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            Microsoft.Windows.Shell.SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            Microsoft.Windows.Shell.SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            Microsoft.Windows.Shell.SystemCommands.RestoreWindow(this);
        }


        private void ThemeObserver_GlobalPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //this.backgroundAnimation.Begin();
            // start background animation if theme has changed
            if (e.PropertyName == "ThemeSource" && this.backgroundAnimation != null)
            {
                this.backgroundAnimation.Begin();
            }
        }

        #endregion

        #region Overridies
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        /// 
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // retrieve BackgroundAnimation storyboard
            
            var border = GetTemplateChild("WindowBorder") as Border;
            if (border != null) {
                this.backgroundAnimation = border.Resources["BackgroundAnimation"] as Storyboard;

                if (this.backgroundAnimation != null) {
                    this.backgroundAnimation.Begin();
                }
            }
            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (Modulos.Accounts.AccountOn.Autenticado == true)
            {
                new Modulos.Accounts.AccountOn().LogOut();
            }           
        }

        #endregion

        private void NavigationWindow_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                ShowFadingContent("FadingContent", (sender as Frame));
                ShowFadingContent("ShowRightContent", (sender as Frame));
            }

            if (e.NavigationMode == NavigationMode.Back)
            {
                ShowFadingContent("FadingContent", (sender as Frame));
                ShowFadingContent("ShowRightContent", (sender as Frame));
            }
        }

        private void ShowFadingContent(string Storyboard, Frame pnl)
        {
            Storyboard sb = Resources[Storyboard] as Storyboard;
            sb.Begin(pnl);
        }

        private void mLateral_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as Grid).Visibility == Visibility.Visible)
            {
                Storyboard sb = Resources["ShowLeftMenu"] as Storyboard;
                sb.Begin((sender as Grid));
            }
        }
    }
}
