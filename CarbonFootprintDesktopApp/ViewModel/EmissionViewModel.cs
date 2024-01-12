using CarbonFootprintDesktopApp.Model;
using CarbonFootprintDesktopApp.View;
using CarbonFootprintDesktopApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonFootprintDesktopApp.ViewModel
{
    public class EmissionViewModel : Utilities.ViewModelBase
    {
        private string Source;

        public string source
        {
            get { return source; }
            set 
            { 
                source = value; 
            }
        }
        private string Unit;

        public string unit
        {
            get { return unit; }
            set 
            {
                unit = value; 
            }
        }

        private double Usage;

        public double usage
        {
            get { return usage; }
            set 
            { 
                usage = value; 
            }
        }

        private int Years;

        public int years
        {
            get { return years; }
            set { years = value; }
        }


        public ICommand AddEmissionCommand { get; set; }

        public EmissionViewModel()
        {
            AddEmissionCommand = new AddEmissionCommand(this);
        }
        
        public void CreateEmission()
        {

        }
    }
}
