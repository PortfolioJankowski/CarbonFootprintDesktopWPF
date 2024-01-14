using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.Model;
using CarbonFootprintDesktopApp.View;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CarbonFootprintDesktopApp.ViewModel.Commands
{
    public class NewEmissionCommand : ICommand
    {
        public HomeVM VM { get; set; }

        public NewEmissionCommand(HomeVM vm)
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
            EmissionView view = new EmissionView();
            view.ShowDialog();
            //po zamknięciu formy ładujemy kalkulacje ponownie
            VM.Calculations.Clear();
            VM.Calculations = new ObservableCollection<Calculation>(VM.GetCalculations());
            VM.TotalResult = HelperDB.GetResult().ToString("#,##0");
        }
    }
}
