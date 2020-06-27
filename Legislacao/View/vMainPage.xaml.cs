using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Sim.Modulos.Legislacao.View
{
    /// <summary>
    /// Interaction logic for vMainPage.xaml
    /// </summary>
    public partial class vMainPage : Page
    {
        public delegate void Action();

        public vMainPage()
        {
            InitializeComponent();
            /*
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background, new MyOwnAction(InitializeComponent));*/
        }

    }
}
