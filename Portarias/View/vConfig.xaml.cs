using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Animation;

namespace Sim.Modulos.Portarias.View
{
    /// <summary>
    /// Interaction logic for ucConfig.xaml
    /// </summary>
    public partial class vConfig : Page
    {
        public vConfig()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.vmConfig();
        }

        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
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
    }
}
