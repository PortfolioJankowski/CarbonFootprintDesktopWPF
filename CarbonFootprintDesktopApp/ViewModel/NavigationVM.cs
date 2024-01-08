using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CarbonFootprintDesktopApp.Utilities;
using CarbonFootprintDesktopApp.ViewModel;

namespace CarbonFootprintDesktopApp.ViewModel
{
    public class NavigationVM : Utilities.ViewModelBase
    {
        //przechowuje aktualny widok, który ma być wyświetlany
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        //przyciski zostaną zainicjowane w konstruktorze
        public ICommand HomeCommand { get; set; }
        public ICommand ScopesCommnad { get; set; }
        public ICommand CategoriesCommand { get; set; }
        public ICommand SourcesCommand { get; set; }
        public ICommand LocationsCommand { get; set; }
        public ICommand FactorsCommand { get; set; }

        //to będzie zmieniało widok tworzac nowe CurrentView, które jest obserwowane przez OnPropChanged
        private void Home(object obj) => CurrentView = new HomeVM();
        private void Scopes(object obj) => CurrentView = new ScopesVM();
        private void Categories(object obj) => CurrentView = new CategoriesVM();
        private void Sources(object obj) => CurrentView = new SourcesVM();
        private void Locations(object obj) => CurrentView = new LocationsVM();
        private void Factors(object obj) => CurrentView = new FactorsVM();

        public NavigationVM()
        {
            /* RelayCommand -> klasa implementująca ICommand
             * Dzięki temu po utworzeniu obiektu NavVM i związaniu go z UI przyciski będą wywoływały akcje
             * czyli przełączanie się między ViewModelami */
            HomeCommand = new RelayCommand(Home);
            ScopesCommnad = new RelayCommand(Scopes);
            CategoriesCommand = new RelayCommand(Categories);
            SourcesCommand = new RelayCommand(Sources);
            LocationsCommand = new RelayCommand(Locations);
            FactorsCommand = new RelayCommand(Factors);

            //Strona startowa
            CurrentView = new HomeVM();
        }


    }
}
