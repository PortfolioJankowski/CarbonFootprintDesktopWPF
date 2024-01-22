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
     public class ScopesVM :Utilities.ViewModelBase
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
            DoughnutChartViewModel chart = new();
        }

        private void SetValues()
        {
            List<Calculation> calculations = new List<Calculation>(HelperDB.Read<Calculation>().Where(c => c.Method != "Location")); 
            FirstScope = calculations.Where(c => c.Scope == "Scope 1").Sum(c => c.Result);
            SecondScope = calculations.Where(c => c.Scope == "Scope 2").Sum(c => c.Result);
            ThirdScope = calculations.Where(c => c.Scope == "Scope 3").Sum(c => c.Result);
        }

    }

    public class DoughnutChartViewModel
    {
        public SeriesCollection SeriesCollection { get; set; }

        public DoughnutChartViewModel()
        {
            List<Calculation> calculations = new List<Calculation>(HelperDB.Read<Calculation>().Where(c => c.Method != "Location"));
            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Scope 1",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(calculations.Where(c => c.Scope == "Scope 1").Sum(c => c.Result)) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Scope 2",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(calculations.Where(c => c.Scope == "Scope 2").Sum(c => c.Result)) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Scope 3",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(calculations.Where(c => c.Scope == "Scope 3").Sum(c => c.Result)) },
                    DataLabels = true
                }  
            };
            
        }  
    }
}
