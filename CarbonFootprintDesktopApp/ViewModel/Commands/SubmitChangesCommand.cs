using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonFootprintDesktopApp.ViewModel.Commands
{
    public class SubmitChangesCommand : ICommand
    {
        HomeVM VM;
        public SubmitChangesCommand(HomeVM vm)
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
            return true;
        }

        public void Execute(object? parameter)
        {
            HelperDB.Update(VM.SelectedCalculation, new Model.Calculation
            {
                Id = VM.SelectedCalculation.Id,
                Category = VM.SelectedCalculation.Category,
                Location = VM.SelectedCalculation.Location,
                Method = VM.SelectedCalculation.Method,
                Result = VM.SelectedCalculation.Result,
                Scope = VM.SelectedCalculation.Scope,
                Sector = VM.SelectedCalculation.Sector,
                Source = VM.SelectedCalculation.Source,
                Unit = VM.SelectedCalculation.Unit,
                Usage = VM.SelectedCalculation.Usage,
                Year = VM.SelectedCalculation.Year, 
            });
        }
    }
}
