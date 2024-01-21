using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.Model;
using CarbonFootprintDesktopApp.Utilities;
using CarbonFootprintDesktopApp.View;
using CarbonFootprintDesktopApp.ViewModel.Commands;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Ganss.Excel;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using Microsoft.Win32;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using NPOI.XSSF.Streaming.Values;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        private int year;
        public int Year
        {
            get { return year; }
            set 
            {
                year = value;
                OnPropertyChanged("Year");
                Validate();
                OnPropertyChanged("IsCloseVisible");
            }
        }

        private string source;
        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                OnPropertyChanged("Source");
                Units = HelperDB.Read<Factor>()
                        .Where(c => c.Source == Source)
                        .Select(c => c.Unit)
                        .Distinct()
                        .ToList();
                OnPropertyChanged("Source");
                Validate();
                OnPropertyChanged("IsCloseVisible");
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
                Validate();
                OnPropertyChanged("IsCloseVisible");
               
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
                Validate();
                OnPropertyChanged("IsCloseVisible");
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
                Validate();
                OnPropertyChanged("IsCloseVisible");
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
                Validate();
                OnPropertyChanged("IsCloseVisible");
            }
        }
        private int id;

        public int Id
        {
            get { return id; }
            set 
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }


        //łączny ślad węglowy
        private string totalResult { get; set; }
        public string TotalResult
        {
            get { return totalResult; }
            set
            {
                totalResult = value;
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
                if (SelectedCalculation != null)
                {
                    //jeżeli kalkulacja została wybrana to przypisuje jej właściwości
                    Year = SelectedCalculation.Year;
                    Location = SelectedCalculation.Location;
                    Sector = SelectedCalculation.Sector;
                    Usage = SelectedCalculation.Usage;
                    Unit = SelectedCalculation.Unit;
                    Source = SelectedCalculation.Source;
                    Id = SelectedCalculation.Id;
                    OnPropertyChanged("Year");
                    OnPropertyChanged("Location");
                    OnPropertyChanged("Sector");
                    OnPropertyChanged("Usage");
                    OnPropertyChanged("Unit");
                    OnPropertyChanged("Source");
                    OnPropertyChanged("Id");
                    Units = HelperDB.Read<Factor>()
                            .Where(c => c.Source == Source)
                            .Select(c => c.Unit)
                            .Distinct()
                            .ToList();

                    if (Year != null && Sector != null && Location != null && Unit != null && Usage != null && Source != null)
                    {
                        //jeżeli kalkulacja jest wybrana i jej właściwości są uzupełnione to przycisk zamykania jest aktywny
                        IsCloseVisible = Visibility.Visible;
                    }
                    else
                    {
                        IsCloseVisible = Visibility.Hidden;
                    }
                }
                else
                {
                    IsCloseVisible = Visibility.Visible;
                }

                OnPropertyChanged("IsCloseVisible");
                OnPropertyChanged("SelectedCalculation");
                OnPropertyChanged("Units");
            }
        }
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
                //jeżeli mam kalkulacje to pokazuje przycisk do eksportu
                if (calculations.Count > 0)
                {
                    IsCalculation = Visibility.Visible;
                    OnPropertyChanged("IsCalculation");
                }
                else
                {
                    IsCalculation = Visibility.Hidden;
                    OnPropertyChanged("IsCalculation");
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
        public ICommand SubmitChangesCommand { get; set; }
        public ICommand ExportCommand { get; set; }

        private Visibility isCloseVisible;

        public Visibility IsCloseVisible
        {
            get { return isCloseVisible; }
            set
            {
                isCloseVisible = value;
            }
        }
        private Visibility isCalculation;

        public Visibility IsCalculation
        {
            get { return isCalculation; }
            set 
            {
                isCalculation = value;
                OnPropertyChanged("IsCalculation");
            }
        }

        //Konstruktor
        public HomeVM()
        {
            Calculations = new ObservableCollection<Calculation>(HelperDB.Read<Calculation>().Where(c => c.Method != "Location")); 
            TotalResult = Calculations.Sum(e => e.Result).ToString("#,##0");
            NewEmissionCommand = new NewEmissionCommand(this);
            CalculationView = new ObservableCollection<Calculation>(Calculations);
            FilterCommand = new FilterCommand(this);
            DeleteEmissionCommand = new DeleteEmissionCommand(this);
            ImportEmissionCommand = new ImportEmissionCommand(this);
            ChangeEmissionCommand = new ChangeEmissionCommand(this);
            SubmitChangesCommand = new SubmitChangesCommand(this);
            ExportCommand = new ExportCommand(this);
            Units = new List<string>();
            IsCloseVisible = Visibility.Visible;
        }
        
        //metoda filtrująca
        public void Filter()
        {
            CalculationView.Clear();
            if (string.IsNullOrEmpty(FilteredText))
            {
                Calculations = new ObservableCollection<Calculation>(HelperDB.Read<Calculation>().Where(c => c.Method != "Location"));
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
                                    xlWorkbook.Close(false);
                                    Marshal.ReleaseComObject(xlWorkbook);
                                    xlApp.Quit();
                                    Marshal.ReleaseComObject(xlApp);
                                    Calculations.Clear();
                                    Calculations = new ObservableCollection<Calculation>(HelperDB.Read<Calculation>().Where(c => c.Method != "Location"));
                                    TotalResult = Calculations.Sum(e => e.Result).ToString("#,##0");
                                    SuccesMsgBox succes = new();
                                    succes.ShowDialog();
                                }
                                else
                                {
                                    FailMsgBox fail = new();
                                    fail.ShowDialog();
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
                        
                            HelperDB.InsertEmission(new Emission
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
        public void Validate()
        {
            if (Year == null || string.IsNullOrEmpty(Location) || string.IsNullOrEmpty(Sector) || string.IsNullOrEmpty(Usage.ToString()) || Unit == null || Source == null)
            {
                IsCloseVisible = Visibility.Hidden;
            }
            else
            {
                IsCloseVisible = Visibility.Visible;
            }
        }
    }
}