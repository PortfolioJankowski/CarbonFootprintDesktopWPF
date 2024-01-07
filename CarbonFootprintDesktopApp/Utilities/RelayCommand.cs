using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace CarbonFootprintDesktopApp.Utilities
{
     class RelayCommand : ICommand
    {
        //przechowuje delegat (akcje) która zostanie wykonana gdy uruchomie polecenie
        private readonly Action<object> _execute;
        //pole przechowuje predykat czy polecenie może zostawć wykonane
        private readonly Func<object, bool> _canExecute;

        //wywołuje gdy rezultat metody CanExecute może ulec zmianie
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        //jeśli nie dostarcze canExecute to metoda CanExecute zawsze zwróci true a polecenie zawsze będzie możliwe do wykonania
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        //Metoda Czy można wykonać? Tak jeśli pole jest nullem, albo gdy predykat zwraca true
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        
        //odpalamy 
        public void Execute(object parameter) => _execute(parameter); 
    }
}
