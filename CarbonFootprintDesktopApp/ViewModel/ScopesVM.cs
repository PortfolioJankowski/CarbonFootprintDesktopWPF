using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarbonFootprintDesktopApp.Model;

namespace CarbonFootprintDesktopApp.ViewModel
{
    //dziedzicze z klasy, która zaimplementowała INotifyPropertyChanged
     class ScopesVM :Utilities.ViewModelBase
    {
        //pole o klasie modelu
        private readonly Emission _emission;

        //mapowanie właściwości z modelu
        public string Scope
        {
            get { return _emission.Scope; }
            set { _emission.Scope = value; OnPropertyChanged(); }
        }

        public ScopesVM()
        {
            _emission = new Emission();
            Scope = "Zakres 1";
        }

    }
}
