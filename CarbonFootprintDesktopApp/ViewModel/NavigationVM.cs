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
        public ICommand MethodsCommand { get; set; }
       
        //to będzie zmieniało widok tworzac nowe CurrentView, które jest obserwowane przez OnPropChanged
        private void Home(object obj) => CurrentView = new HomeVM();
        private void Scopes(object obj) => CurrentView = new ScopesVM();
        private void Categories(object obj) => CurrentView = new CategoriesVM();
        private void Methods(object obj) => CurrentView = new MethodsVM();
 

        public NavigationVM()
        {
            /* RelayCommand -> klasa implementująca ICommand
             * Dzięki temu po utworzeniu obiektu NavVM i związaniu go z UI przyciski będą wywoływały akcje
             * czyli przełączanie się między ViewModelami */
            HomeCommand = new RelayCommand(Home);
            ScopesCommnad = new RelayCommand(Scopes);
            CategoriesCommand = new RelayCommand(Categories);
            MethodsCommand = new RelayCommand(Methods);

            //Strona startowa
            CurrentView = new HomeVM();
        }


    }
}
