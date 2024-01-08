using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.Model;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;
using LiveCharts.Defaults;

namespace CarbonFootprintDesktopApp.ViewModel
{
    //dziedzicze z klasy, która zaimplementowała INotifyPropertyChanged
     class ScopesVM :Utilities.ViewModelBase
    {
 
        private double firstScope;

        public double FirstScope
        {
            get { return firstScope; }
            set { firstScope = GetScopeResult(1); }
        }

        private double secondScope;

        public double SecondScope
        {
            get { return secondScope; }
            set { secondScope = GetScopeResult(2); }
        }

        private double thirdScope;

        public double ThirdScope
        {
            get { return thirdScope; }
            set { thirdScope = GetScopeResult(3); }
        }

        public ScopesVM()
        { 
            
        }

        public double GetScopeResult(int number)
        {
            return HelperDB.GetResult();
        }

    }
    public class DoughnutChartViewModel
    {
        public SeriesCollection SeriesCollection { get; set; }

        public DoughnutChartViewModel()
        {
            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Scope 1",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Scope 2",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(6) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Scope 3",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(10) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Values = new ChartValues<ObservableValue> { new ObservableValue(4) },
                    DataLabels = true
                }
            };
        }
    }
}
