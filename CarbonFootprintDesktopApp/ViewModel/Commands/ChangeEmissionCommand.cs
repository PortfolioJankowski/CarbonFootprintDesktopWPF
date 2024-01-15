using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonFootprintDesktopApp.ViewModel.Commands
{
    public class ChangeEmissionCommand : ICommand
    {
        HomeVM VM;
        public ChangeEmissionCommand(HomeVM vm) { VM = vm; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value;  }
            remove { CommandManager.RequerySuggested -= value;}
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
            throw new NotImplementedException();
        }
    }
}
