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
    /// Interaction logic for vPDFImage.xaml
    /// </summary>
    public partial class vPDFImage : UserControl
    {
        public vPDFImage(string _urlbase, string urlfile)
        {
            InitializeComponent();
           
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\libmupdf.dll"))
                this.DataContext = new ViewModel.vmPDFImage(_urlbase, urlfile);
            
            else
                MessageBox.Show("Biblioteca libmupdf.dll não encontrada.\nReinstale o SIM para corrigir o problema!",
                    "Sim.App.Alerta", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            
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
