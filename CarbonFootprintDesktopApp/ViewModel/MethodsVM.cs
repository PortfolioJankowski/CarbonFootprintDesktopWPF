using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.Model;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CarbonFootprintDesktopApp.ViewModel
{
    public class MethodsVM : Utilities.ViewModelBase
    {
        public MethodsVM()
        {
            Calculations = new ObservableCollection<Calculation>(HelperDB.Read<Calculation>());
            SeriesCollection = new SeriesCollection();

            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Location",
                Values = new ChartValues<double> { Math.Round(LocationResult, 2) },
                ColumnPadding = 40,
                MaxColumnWidth = 80
            });

            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Market",
                Values = new ChartValues<double> { Math.Round(MarketResult,2) },
                ColumnPadding = 40,
                MaxColumnWidth = 80
            });
            Labels = new[] { "Methods" };
        }
        private ObservableCollection<Calculation> calculations;
        public ObservableCollection<Calculation> Calculations
        {
            get { return calculations; }
            set 
            {
                calculations = value;
                MarketResult = Calculations.Where(c => c.Method == "Location" || c.Method =="General").Sum(c => c.Result);
                LocationResult = Calculations.Where(c => c.Method == "Market" || c.Method == "General").Sum(c => c.Result);
                OnPropertyChanged("Calculations");
                OnPropertyChanged("MarketResult");
                OnPropertyChanged("Calculations");
            }
        }
        private double locationResult;
        public double LocationResult
        {
            get { return locationResult; }
            set { locationResult = value; }
        }
        private double marketResult;
        public double MarketResult
        {
            get { return marketResult; }
            set { marketResult = value; }
        }

        //WYKRES
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }


    }

}
