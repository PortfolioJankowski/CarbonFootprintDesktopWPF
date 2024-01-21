using CarbonFootprintDesktopApp.Database;
using CarbonFootprintDesktopApp.Model;
using CarbonFootprintDesktopApp.View;
using Devart.Data.SQLite;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VBA = Microsoft.Vbe.Interop;
using Excel = Microsoft.Office.Interop.Excel;

namespace CarbonFootprintDesktopApp.Services
{
    public class Exporter
    {
        public string ChooseFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files (*.xlsm)|*.xlsm";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Title = "Choose folder";
            saveFileDialog.FileName = "Report.xlsm";
            if (saveFileDialog.ShowDialog() == true)
            {
                string destination = saveFileDialog.FileName;
                return destination;
            }
            else
            {
                return null;
            }
        }

        public void Export(string filePath)
        {
            //pobieram sobie ścieżke templatki
            string templatePath = Path.GetFullPath(Path.Combine(System.IO.Path.Combine(new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().FullName).DirectoryName, ""), @"..\..\..\Services\ExcelTemplate\Template.xlsx"));
            //kopiuje templatke na pulpit
            File.Copy(templatePath, filePath, true);

            try
            {
                using (System.Data.SQLite.SQLiteConnection cnn = new System.Data.SQLite.SQLiteConnection($"Data Source={App.databasePath};Version=3;Read Only=True;"))
                {
                    //ładuje tabele z bazy do tabeli w C#
                    cnn.Open();
                    System.Data.SQLite.SQLiteCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM Calculation";
                    System.Data.SQLite.SQLiteDataReader execute = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);
                    System.Data.DataSet dt = new();
                    dt.Load(execute, System.Data.LoadOption.OverwriteChanges, "Calculation");
                    
                    //jeżeli załadowała się tabela to
                    if (dt.Tables.Count > 0 && dt.Tables[0].Columns.Count > 0)
                    {
                        //kopiuje templatke na pulpit
                        File.Copy(templatePath, filePath, true);
                        //wrzucam do skopiowanego pliku kalkulacje
                        Spire.Xls.Workbook book = new Spire.Xls.Workbook();
                        book.LoadFromFile(filePath);
                        Spire.Xls.Worksheet sheet = book.Worksheets[0];
                        sheet.InsertDataTable(dt.Tables[0], true, 1, 1);
                        book.Save();

                        SuccesMsgBox success = new();
                        success.ShowDialog();
                    }
                    else
                    {
                        FailMsgBox fail = new();
                        fail.ShowDialog();
                    }
                    
                }
            }
            catch
            {
                FailMsgBox fail = new();
                fail.ShowDialog();
            }
            
        }
    }
} 
