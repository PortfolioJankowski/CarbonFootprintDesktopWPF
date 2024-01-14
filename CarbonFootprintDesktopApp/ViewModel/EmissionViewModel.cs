using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.Model;
using CarbonFootprintDesktopApp.View;
using CarbonFootprintDesktopApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonFootprintDesktopApp.ViewModel
{
    public class EmissionViewModel : Utilities.ViewModelBase
    {
        //deklaruje zmienne, które będą zbindowane z Comboboxami
        private int year;
        public int Year
        {
            get { return year; }
            set 
            { 
                year = value;
                OnPropertyChanged("Year");
                OnPropertyChanged("IsFormValid");
            }
        }

        private string unit;
        public string Unit
        {
            get { return unit; }
            set
            { 
                unit = value;
                OnPropertyChanged("Unit");
                OnPropertyChanged("IsFormValid");
            }
        }

        private double usage;

        public double Usage
        {
            get { return usage; }
            set 
            {
                usage = value;
                OnPropertyChanged("Usage");
                OnPropertyChanged("IsFormValid");
            }
        }

        private string location;

        public string Location
        {
            get { return location; }
            set
            { 
                location = value;
                OnPropertyChanged("Location");
                OnPropertyChanged("IsFormValid");
            }
        }

        private string sector;

        public string Sector
        {
            get { return sector; }
            set 
            { 
                sector = value;
                OnPropertyChanged("Sector");
                OnPropertyChanged("IsFormValid");
            }
        }

        //deklaruje listy dla Comboboxów
        public List<string> Sources { get; set; }
        public List<int> Years { get; set; }
        //zmieniająca się lista w zależności od wyboru źródła -> muszę ją obserwować
        private List<string> units;
        public List<string> Units
        {
            get { return units; }
            set
            {
                units = value;
                OnPropertyChanged("Units");
            }
        }
        //w zależności od wybranego źródła będę ładował jednostki
        private string selectedSource;
        public string SelectedSource
        {
            get { return selectedSource; }
            set 
            { 
                selectedSource = value;
                OnPropertyChanged("SelectedSource");
                Units = HelperDB.GetEmissions()
                         .Where(e => e.Source == selectedSource)
                         .Select(e => e.Unit)
                         .Distinct()
                         .ToList();
                isUnitEnabled = true;
                OnPropertyChanged("IsUnitEnabled");
                OnPropertyChanged("IsFormValid");
            }
        }

        //guziczek
        public ICommand AddEmissionCommand { get; set; }
        //do tego EventHandlera odwołam się w CodeBehind formy
        public EventHandler CloseWindow;
        //gdy wybiorę źródło będę mógł wybrać jednostkę
        private bool isUnitEnabled;
        public bool IsUnitEnabled
        {
            get { return isUnitEnabled; }
            set 
            {   
                isUnitEnabled = value;
                OnPropertyChanged("IsUnitEnabled");
            }
        }
        //czy mogę odpalić przycisk dodawania emisji
        public bool IsFormValid => Validate();

        //inicjalizuje listboxy
        public EmissionViewModel()
        {
            AddEmissionCommand = new AddEmissionCommand(this);
            Sources = HelperDB.GetEmissions().Select(e => e.Source).Distinct().ToList();
            Years = HelperDB.GetEmissions().Select(e => e.Year).Distinct().ToList();
            Units = new List<string>();
            isUnitEnabled = false;
        }
        
        public void CreateEmission()
        {
            //jeżeli jest energia elektryczna to dodaje 2x (w jednym przypadku dodaje z Additional, żeby zaznaczyć że będzie location)
            if (selectedSource != "Purchased grid electricity")
            {
                HelperDB.Insert(new Emission
                {
                    Year = Year,
                    Additional = "0",
                    Location = Location,
                    Sector = Sector,
                    Unit = Unit,
                    Usage = Usage,
                    Source = SelectedSource,
                });
            }
            else
            {
                HelperDB.Insert(new Emission
                {
                    Year = Year,
                    Additional = "0",
                    Location = Location,
                    Sector = Sector,
                    Unit = Unit,
                    Usage = Usage,
                    Source = SelectedSource,
                });
                HelperDB.Insert(new Emission
                {
                    Year = Year,
                    Additional = Location,
                    Location = Location,
                    Sector = Sector,
                    Unit = Unit,
                    Usage = Usage,
                    Source = SelectedSource,
                });
            }
            CloseWindow?.Invoke(this, new EventArgs());
        }

        public bool Validate()
        {
            //sprawdzam czy pola są wypełnione
            if (Unit != null && SelectedSource != null && Usage != 0 && Year != null && Sector != null && Location != null) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}
