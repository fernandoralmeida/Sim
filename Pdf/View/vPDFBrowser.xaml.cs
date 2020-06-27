using System;
using System.Collections.Generic;
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

namespace Sim.Pdf.View
{
    /// <summary>
    /// Interaction logic for vPDFBrowser.xaml
    /// </summary>
    public partial class vPDFBrowser : UserControl
    {
        public vPDFBrowser(string httpurl)
        {
            InitializeComponent();
            this.DataContext = new ViewModel.vmPDFBrowser(httpurl);
        }

        public double AlternateWidth()
        {
            double width = SystemParameters.PrimaryScreenWidth;

            width = (width / 2);

            return width;
        }

        public double AlternateHeight()
        {

            double height = SystemParameters.PrimaryScreenHeight;

            height = height - (height * 0.2);

            return height;
        }
    }
}
