using CarbonFootprintDesktopApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonFootprintDesktopApp.ViewModel
{
    public  class EmissionChangeViewModel : Utilities.ViewModelBase
    {
        public  ICommand SubmitChangesCommand { get; set; }
        public EmissionChangeViewModel()
        {
            
        }
    }
}
