using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.Model;
using CarbonFootprintDesktopApp.Utilities;
using CarbonFootprintDesktopApp.View;
using CarbonFootprintDesktopApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
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
                    OnPropertyChanged(nameof(Calculations));
                }
            }
        }
        public ICommand NewEmissionCommand { get; set; }
        public ICommand ChangeEmissionCommand;
        public ICommand DeleteEmissionCommand;
        public ICommand ImportEmissionCommand;


        public HomeVM() 
        {
            Calculations = new ObservableCollection<Calculation>(GetCalculations());
            TotalResult = HelperDB.GetResult().ToString("#,##0") ;
            NewEmissionCommand = new NewEmissionCommand(this);

        }

        public IEnumerable<Calculation> GetCalculations()
        {
            return HelperDB.GetCalculations();
        }
    }
}
