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
            set 
            {
                firstScope = value;
                OnPropertyChanged("FirstScope"); 
             }
        }

        private double secondScope;

        public double SecondScope
        {
            get { return secondScope; }
            set 
            {
                secondScope = value;
                OnPropertyChanged("SecondScope");
            }
        }

        private double thirdScope;

        public double ThirdScope
        {
            get { return thirdScope; }
            set 
            { 
                thirdScope = value;
                OnPropertyChanged("ThirdScope");
            }
        }

        public ScopesVM()
        {
            SetValues();
        }

        private void SetValues()
        {
            FirstScope = Convert.ToDouble(HelperDB.GetPieChartData("Scope 1"));
            SecondScope = HelperDB.GetPieChartData("Scope 2");
            ThirdScope = HelperDB.GetPieChartData("Scope 3");
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
                    Values = new ChartValues<ObservableValue> { new ObservableValue(HelperDB.GetPieChartData("Scope 1")) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Scope 2",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(HelperDB.GetPieChartData("Scope 2")) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Scope 3",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(HelperDB.GetPieChartData("Scope 3")) },
                    DataLabels = true
                }  
            };
        }
    }
}
