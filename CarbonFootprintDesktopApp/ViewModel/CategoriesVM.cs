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
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using NPOI.SS.Formula.Functions;

namespace CarbonFootprintDesktopApp.ViewModel
{
    public class CategoriesVM : Utilities.ViewModelBase
    {
        public CategoriesVM()
        {
            Calculations = new ObservableCollection<Calculation>(HelperDB.Read<Calculation>().Where(c => c.Method != "Location"));
            var categories = Calculations.Select(e => e.Category).Distinct().ToList();
            SeriesCollection = new SeriesCollection();
            
            foreach (var category in categories)
            {
                var ColumnSeries = new ColumnSeries
                {
                    Title = category,
                    Values = new ChartValues<double> { Math.Round(Calculations
                                                            .Where(c => c.Category == category)
                                                            .Sum(c => c.Result), 2) },
                    ColumnPadding = 20,
                    MaxColumnWidth = 50
                };
                SeriesCollection.Add(ColumnSeries);  
            }
            Labels = new[] { "Emission Categories" };
            Formatter = value => value.ToString("N");
        }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public string BestCategory { get; set; }
        public string WorstCategory { get; set; }
        public double BestCategoryResult { get; set; }
        public double WorstCategoryResult { get; set; }
        private ObservableCollection<Calculation> calculations;
        public ObservableCollection<Calculation> Calculations
        {
            get { return calculations; }
            set 
            { 
                calculations = value;
                BestCategory = Calculations
                                    .GroupBy(c => c.Category)
                                    .OrderBy(group => group.Sum(c => c.Result))
                                    .Select(group => group.Key)
                                    .FirstOrDefault();
                WorstCategory = Calculations
                                    .GroupBy(c => c.Category)
                                    .OrderByDescending(group => group.Sum(c => c.Result))
                                    .Select(group => group.Key)
                                    .FirstOrDefault();
                BestCategoryResult = Calculations
                                         .GroupBy(c => c.Category)
                                         .OrderBy(group => group.Sum(c => c.Result))
                                         .Select(group => group.First().Result)
                                         .FirstOrDefault();
                WorstCategoryResult = Calculations
                                        .GroupBy(c => c.Category)
                                        .OrderByDescending(group => group.Sum(c => c.Result))
                                        .Select(group => group.First().Result)
                                        .FirstOrDefault();
                OnPropertyChanged(nameof(Calculations));
            }
        }

    }
}
