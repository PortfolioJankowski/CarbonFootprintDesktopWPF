﻿using CarbonFootprintDesktopApp.View;
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
            try
            {
                VM.CreateEmission();
                VM.CloseWindow?.Invoke(this, new EventArgs());
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
