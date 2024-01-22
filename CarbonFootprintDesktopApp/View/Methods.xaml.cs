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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarbonFootprintDesktopApp.View
{
    /// <summary>
    /// Logika interakcji dla klasy Methods.xaml
    /// </summary>
    public partial class Methods : UserControl
    {
        public Methods()
        {
            InitializeComponent();
            Chart.AxisX[0].Separator.StrokeThickness = 0;
            Chart.AxisY[0].Separator.StrokeThickness = 0;
      
        }
    }
}
