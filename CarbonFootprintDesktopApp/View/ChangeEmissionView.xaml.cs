using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.Model;
using CarbonFootprintDesktopApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Logika interakcji dla klasy ChangeEmissionView.xaml
    /// </summary>
    public partial class ChangeEmissionView : Window
    {
        HomeVM VM;
        public ChangeEmissionView(HomeVM vm)
        {
            InitializeComponent();
            VM = vm;
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HelperDB.Update<Calculation>(VM.SelectedCalculation);
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UnitsCmb.ItemsSource =  HelperDB.Read<Factor>()
                                 .Where(c => c.Source == VM.SelectedCalculation.Source)
                                 .Select(c => c.Unit)
                                 .Distinct().ToList();
        }
    }
}
