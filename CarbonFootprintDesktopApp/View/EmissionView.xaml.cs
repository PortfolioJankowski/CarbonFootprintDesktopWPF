using CarbonFootprintDesktopApp.ViewModel;
using CarbonFootprintDesktopApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarbonFootprintDesktopApp.View
{
    /// <summary>
    /// Logika interakcji dla klasy EmissionView.xaml
    /// </summary>
    public partial class EmissionView : Window
    {
        EmissionViewModel VM;
        public EmissionView()
        {
            InitializeComponent();
            VM = Resources["vm"] as EmissionViewModel;
            VM.CloseWindow += VM_CloseWindow; ;
        }

        private void VM_CloseWindow(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }
    }
}
