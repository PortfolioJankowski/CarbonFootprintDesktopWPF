using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.Model;
using CarbonFootprintDesktopApp.Utilities;
using CarbonFootprintDesktopApp.View;
using CarbonFootprintDesktopApp.ViewModel.Commands;
using Ganss.Excel;
using HandyControl.Controls;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace CarbonFootprintDesktopApp.ViewModel
{
    public class HomeVM : Utilities.ViewModelBase
    {
        //łączny ślad węglowy
        private string totalResult { get; set; }
        public string TotalResult
        {
            get { return totalResult; }
            set
            {
                totalResult = HelperDB.GetResult().ToString();
                OnPropertyChanged("TotalResult");
            }
        }
        //wybrany record z grida
        private Calculation selectedCalculation;
        public Calculation SelectedCalculation
        {
            get { return selectedCalculation; }
            set
            {
                selectedCalculation = value;
                OnPropertyChanged(nameof(SelectedCalculation));
            }
        }

        //kolekcja wszystkich kalkulacji, jakie pobieram z rozpoczęciem programu
        private ObservableCollection<Calculation> calculations { get; set; }
        public ObservableCollection<Calculation> Calculations
        {
            get { return calculations; }
            set
            {
                if (calculations != value)
                {
                    calculations = value;
                    CalculationView = new ObservableCollection<Calculation>(Calculations);
                    OnPropertyChanged(nameof(Calculations));
                    OnPropertyChanged(nameof(CalculationView));
                    OnPropertyChanged(nameof(TotalResult));

                }
            }
        }
        //tekst, który wpisuje w pole filtru
        private string filteredText;
        public string FilteredText
        {
            get { return filteredText; }
            set
            {
                filteredText = value;
                OnPropertyChanged(nameof(FilteredText));
            }
        }
        //kolekcje, które pokazuje w Gridzie
        private ObservableCollection<Calculation> calculationView;
        public ObservableCollection<Calculation> CalculationView
        {
            get { return calculationView; }
            set
            {
                calculationView = value;
                OnPropertyChanged(nameof(CalculationView));
            }
        }
        //guziki
        public ICommand NewEmissionCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand ChangeEmissionCommand { get; set; }
        public ICommand DeleteEmissionCommand { get; set; }
        public ICommand ImportEmissionCommand { get; set; }

        //Konstruktor
        public HomeVM()
        {
            Calculations = new ObservableCollection<Calculation>(GetCalculations());
            TotalResult = HelperDB.GetResult().ToString("#,##0");
            NewEmissionCommand = new NewEmissionCommand(this);
            CalculationView = new ObservableCollection<Calculation>(Calculations);
            FilterCommand = new FilterCommand(this);
            ChangeEmissionCommand = new ChangeEmissionCommand(this);
            DeleteEmissionCommand = new DeleteEmissionCommand(this);
            ImportEmissionCommand = new ImportEmissionCommand(this);
        }
        //Metoda pobierająca wszystkie kalkulacje z bazy danych
        public IEnumerable<Calculation> GetCalculations()
        {
            return HelperDB.GetCalculations();
        }
        //metoda filtrująca
        public void Filter()
        {
            CalculationView.Clear();
            if (string.IsNullOrEmpty(FilteredText))
            {
                CalculationView = new ObservableCollection<Calculation>(Calculations);
            }
            else
            {
                var filteredCalculations = Calculations
                .Where(calc => calc.Source.Contains(FilteredText, StringComparison.OrdinalIgnoreCase))
                .ToList();

                CalculationView = new ObservableCollection<Calculation>(filteredCalculations);
            }


        }

        public void ImportEmissions()
        {
            string filePath;
            //tworzę okno dialogowe
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Title = "Browse XLSX files",
                DefaultExt = "xlsx",
                CheckFileExists = true,
                Multiselect = false,
                Filter = "(*.xlsx)|*.xlsx"
            };

            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
                //patrze czy ścieżka nie jest pusta
                if (!string.IsNullOrEmpty(filePath))
                {
                    try
                    {
                        dynamic xlApp = Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application"));
                        dynamic xlWorkbook = xlApp.Workbooks.Open(filePath);
                        dynamic xlWorksheet = xlWorkbook.Sheets[1];
                                if (xlWorksheet.Cells[1, 1]?.Text?.ToString() == "bv")
                                {
                                    //wywołuje import
                                    ImportEmissionData(xlWorksheet);
                                    Calculations.Clear();
                                    Calculations = new ObservableCollection<Calculation>(HelperDB.GetCalculations());
                                    TotalResult = HelperDB.GetResult().ToString();
                                    SuccesMsgBox succes = new();
                                    succes.ShowDialog();
                                }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (e.g., display a message or log the error)
                        System.Windows.MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        FailMsgBox fail = new();
                        fail.ShowDialog();
                    }
                }
            }
        }
        //importujemy emisje
        private void ImportEmissionData(dynamic xlWorksheet)
        {
            //inicjalizacja - czytam od 11 wiersza
            var row = 11;
            int i;
            bool flag = true;
            //zakładam, że mogę czytać
            while (flag)
            {
                //interesują mnie kolumny 6-11
                for (i = 6; i <= 11; i++)
                {
                    if (string.IsNullOrEmpty(xlWorksheet.Cells[row, i]?.Text?.ToString()))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    //parsuje do doubla zużycie i dodaje emisje do bazy danych

                    if (double.TryParse(xlWorksheet.Cells[row, 11]?.Text?.ToString(), out double usage) )
                    {
                        int year = Convert.ToInt32(xlWorksheet.Cells[row, 6]?.Text?.ToString());
                        HelperDB.Insert(new Emission
                        {
                            Year = year,
                            Sector = xlWorksheet.Cells[row, 7]?.Text?.ToString(),
                            Source = xlWorksheet.Cells[row, 8]?.Text?.ToString(),
                            Location = xlWorksheet.Cells[row, 9]?.Text?.ToString(),
                            Unit = xlWorksheet.Cells[row, 10]?.Text?.ToString(),
                            Usage = usage,
                            Additional = "0"
                        });
                    }

                    row++;
                }
            }
        }
    }
}