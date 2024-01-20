using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.Model;
using CarbonFootprintDesktopApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonFootprintDesktopApp.ViewModel.Commands
{
    public class ChangeEmissionCommand : ICommand
    {
        HomeVM VM;
        public ChangeEmissionCommand(HomeVM vm)
        {
            VM = vm;
        }
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            if (VM.SelectedCalculation != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Execute(object? parameter)
        {
            //zrób właściwość CalculationTemp
            //VM.Calculation temp = VM.SelectedCalculation
            ChangeEmissionView changeEmissionView = new ChangeEmissionView(VM);
            changeEmissionView.ShowDialog();
            VM.Calculations.Clear();
            VM.Calculations = new ObservableCollection<Calculation>(HelperDB.Read<Calculation>().Where(c => c.Method != "Location"));
            VM.TotalResult = VM.Calculations.Sum(e => e.Result).ToString("#,##0");
        }
    }
}
