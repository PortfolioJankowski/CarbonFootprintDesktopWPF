using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonFootprintDesktopApp.ViewModel.Commands
{
    public class DeleteEmissionCommand : ICommand
    {
        HomeVM VM;

        public DeleteEmissionCommand(HomeVM vm)
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
            try
            {
                HelperDB.DeleteEmission(VM.SelectedCalculation);
                VM.Calculations.Clear();
                VM.Calculations = new System.Collections.ObjectModel.ObservableCollection<Model.Calculation>(VM.GetCalculations());
                VM.TotalResult = HelperDB.GetResult().ToString();
                SuccesMsgBox succes = new();
                succes.ShowDialog();
            }
            catch
            {
                FailMsgBox fail = new();
                fail.ShowDialog();
            }
        }
    }
}
