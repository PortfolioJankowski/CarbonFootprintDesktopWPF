using CarbonFootprintDesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonFootprintDesktopApp.ViewModel.Commands
{
    public class ExportCommand : ICommand
    {
        HomeVM VM;
        public ExportCommand(HomeVM vm)
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
            if (VM.Calculations != null)
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
            Exporter exporter = new();
            string temp = exporter.ChooseFile();
            if (temp != null)
            {
                exporter.Export(temp);
            }
        }
    }
}
