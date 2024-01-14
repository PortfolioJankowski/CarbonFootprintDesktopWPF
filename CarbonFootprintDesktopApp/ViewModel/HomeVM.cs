using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.Model;
using CarbonFootprintDesktopApp.Utilities;
using CarbonFootprintDesktopApp.View;
using CarbonFootprintDesktopApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CarbonFootprintDesktopApp.ViewModel
{
    public class HomeVM : Utilities.ViewModelBase
    {
        private string totalResult { get; set; }
        public string TotalResult {
            get { return totalResult; }
            set
            {
                totalResult = value;
                OnPropertyChanged("TotalResult");
            }
        }
        private ObservableCollection<Calculation> calculations { get; set; }
        public ObservableCollection<Calculation> Calculations
        {
            get { return calculations; }
            set {
                if (calculations != value)
                {
                    calculations = value;
                    CalculationView = new ObservableCollection<Calculation>(Calculations);
                    OnPropertyChanged(nameof(Calculations));
                    OnPropertyChanged(nameof(CalculationView));

                }
            }
        }

        private string filteredText;

        public string FilteredText
        {
            get { return filteredText; }
            set 
            { 
                filteredText = value;
                OnPropertyChanged(nameof(FilteredText));
            }
        }

        private ObservableCollection<Calculation> calculationView;

        public ObservableCollection<Calculation> CalculationView
        {
            get { return calculationView; }
            set 
            {
                calculationView = value;
                OnPropertyChanged(nameof(CalculationView));
            }
        }

        public ICommand NewEmissionCommand { get; set; }
        public ICommand FilterCommand {  get; set; }
        public ICommand ChangeEmissionCommand;
        public ICommand DeleteEmissionCommand;
        public ICommand ImportEmissionCommand;


        public HomeVM() 
        {
            Calculations = new ObservableCollection<Calculation>(GetCalculations());
            TotalResult = HelperDB.GetResult().ToString("#,##0") ;
            NewEmissionCommand = new NewEmissionCommand(this);
            CalculationView = new ObservableCollection<Calculation>(Calculations);
            FilterCommand = new FilterCommand(this);
        }

        public IEnumerable<Calculation> GetCalculations()
        {
            return HelperDB.GetCalculations();    
        }

        public void Filter()
        {
            CalculationView.Clear();
            if (string.IsNullOrEmpty(FilteredText))
            {
                CalculationView = new ObservableCollection<Calculation>(Calculations);
            }
            else
            {
                var filteredCalculations = Calculations
                .Where(calc => calc.Source.Contains(FilteredText, StringComparison.OrdinalIgnoreCase))
                .ToList();

                CalculationView = new ObservableCollection<Calculation>(filteredCalculations);
            }

            if (CalculationView.Count == 0)
                CalculationView = new ObservableCollection<Calculation>(Calculations);

        }
    }
}
