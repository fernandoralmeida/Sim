using System;
using System.ComponentModel;

namespace Sim.Updater.Helpers
{
    class Notify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message,
                    "Sim.App.Aviso!",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
            }
        }
    }
}
