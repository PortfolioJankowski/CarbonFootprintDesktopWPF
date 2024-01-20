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
            var calculation = HelperDB.Read<Calculation>().Where(c => c.Id == VM.Id).First();
            if (calculation != null)
            {
                HelperDB.Update(HelperDB.Read<Calculation>().Where(c => c.Id == VM.Id).First(), new Emission
                {
                    Sector = VM.Sector,
                    Location = VM.Location,
                    Source = VM.Source,
                    Unit = VM.Unit,
                    Usage = VM.Usage,
                    Year = VM.Year,
                    Additional = "0"
                });
            }
            
        }
    }
}
