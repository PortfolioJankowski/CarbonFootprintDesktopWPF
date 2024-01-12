using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonFootprintDesktopApp.ViewModel.Commands
{   
    public class AddEmissionCommand : ICommand
    {
        public EmissionViewModel VM { get; set; }
        public AddEmissionCommand(EmissionViewModel vm)
        {
            VM = vm;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            //TODO
            return true;
        }

        public void Execute(object? parameter)
        {
            //Walidacja czy wszystkie comboboxy są uzupełnione
            VM.CreateEmission();
        }
    }
}
