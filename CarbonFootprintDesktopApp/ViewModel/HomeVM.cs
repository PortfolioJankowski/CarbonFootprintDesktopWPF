using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonFootprintDesktopApp.ViewModel
{
    public class HomeVM : Utilities.ViewModelBase
    {
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
    
        public ICommand NewEmissionCommand;
        public ICommand ChangeEmissionCommand;
        public ICommand DeleteEmissionCommand;
        public ICommand ImportEmissionCommand;


        public HomeVM() 
        {
            Calculations = new ObservableCollection<Calculation>(GetCalculations());
            
        }

        public IEnumerable<Calculation> GetCalculations()
        {
            return HelperDB.GetCalculations();
        }
    }
}
