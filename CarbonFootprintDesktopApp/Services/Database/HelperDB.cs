using CarbonFootprintDesktopApp.Model;
using CarbonFootprintDesktopApp.View;
using HandyControl.Themes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static SQLite.SQLite3;
using System.Windows.Controls.Primitives;
using static SQLite.TableMapping;
using Microsoft.VisualBasic;
using System.Windows;
using CarbonFootprintDesktopApp.ViewModel;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Cmp;

namespace CarbonFootprintDesktopApp.Database
{
    public class HelperDB
    {
        private static char additional1;

        //TODO generyczny insert/update/delete/read/

        public static double GetResult()
        {
            try
            {
                using (var cnn = new SQLite.SQLiteConnection(App.databasePath))
                {
                    string sqlQuery = $@"SELECT SUM(E.Usage* F.Value) as Result
                                            FROM Emissions E
                                            JOIN Factors F
                                            ON E.Year = F.Year  AND E.[Emission Source] = F.Source  AND E.Additional = F.Additional AND F.Unit = E.Unit
                                            WHERE F.Method IN ('Market', 'General')"
                                            ;
                    var result = cnn.ExecuteScalar<double?>(sqlQuery);
                    return (double)result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }

        public static bool InsertEmission<T>(T item) where T : Emission
        {
            bool result = false;      
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.databasePath))
            {
                //dodaje Emisje do tabeli
                conn.CreateTable<T>();
                int rows = conn.Insert(item);
                if (rows > 0) result = true;
                
                //jeżeli dodano Emisje to pobieram z bazy odpowiedni wskaźnik
                List<Factor> factorList = HelperDB.Read<Factor>();
                Factor factor = factorList.Where(f => f.Unit == item.Unit && f.Source == item.Source && f.Additional == "0" && f.Year == item.Year).FirstOrDefault();
                if (factor != null)
                {
                    //tworzę kalkulację, którą dodam do tabeli na podstawie user inputu i pobranego wskaźnika
                    conn.Insert(new Calculation
                    {
                        Year = item.Year,
                        Sector = item.Sector,
                        Scope = factor.Scope,
                        Category = factor.Category,
                        Source = item.Source,
                        Location = item.Location,
                        Unit = item.Unit,
                        Usage = item.Usage,
                        Result = item.Usage * factor.Value,
                        Method = factor.Method
                    });
                }

                //tworzę drugą kalkulację dla location
                if (item.Source == "Purchased grid electricity")
                {
                    factor = factorList.Where(f => f.Unit == item.Unit && f.Source == item.Source && f.Additional == item.Location && f.Year == item.Year).FirstOrDefault();
                    if (factor != null)
                    {
                        factor = factorList.Where(f => f.Unit == item.Unit && f.Source == item.Source && f.Additional == item.Location && f.Year == item.Year).FirstOrDefault();
                        conn.Insert(new Calculation
                        {
                            Year = item.Year,
                            Sector = item.Sector,
                            Scope = factor.Scope,
                            Category = factor.Category,
                            Source = item.Source,
                            Location = item.Location,
                            Unit = item.Unit,
                            Usage = item.Usage,
                            Result = item.Usage * factor.Value,
                            Method = factor.Method
                        });
                    }
                }   
            }
            return result;
        }

        public static bool Update<T>(T item, Calculation calc) where T : Calculation
        {
            Delete(calc);
            InsertEmission(new Emission() { 
                Year = item.Year,
                Sector = item.Sector,
                Additional = "0",
                Location = item.Location,
                Source = item.Source,
                Unit = item.Unit,
                Usage = item.Usage
            });
            return true;
        }

        public static bool Delete<T>(T item) where T : Calculation
        {
            bool result = false;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.databasePath))
            {
                //usuwam kalkulacje
                int rows = conn.Delete(item);
                if (rows > 0) result = true;
                string additional = "0";
                //usuwam emisje związaną z kalkulacją
                string deleteSql = $"DELETE FROM Emissions WHERE Id = (Select Id FROM Emissions WHERE Year = '{item.Year}' AND [Emission Source] = '{item.Source}' AND Unit = '{item.Unit}' AND Sector = '{item.Sector}' AND Location = '{item.Location}' AND Usage = '{item.Usage}' AND Additional = {additional} Limit 1)";
                conn.Execute(deleteSql);

                if (item.Source == "Purchased grid electricity")
                {
                    string deleteLocation = $"DELETE FROM Emissions WHERE Id = (Select Id FROM Emissions WHERE Year = '{item.Year}' AND [Emission Source] = '{item.Source}' AND Unit = '{item.Unit}' AND Sector = '{item.Sector}' AND Location = '{item.Location}' AND Usage = '{item.Usage}' AND Additional = '{item.Location}' Limit 1)";
                }
            }
            return result;
        }

        public static List<T> Read<T>() where T : new()
        {
            List<T> items;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.databasePath))
            {
                items = conn.Table<T>().ToList();
            }
            return items;
        }

        public static double GetPieChartData(string column)
        {
            try {
                using (var cnn = new SQLite.SQLiteConnection(App.databasePath))
                {
                    string sqlQuery = $@"SELECT SUM(E.Usage * F.Value) as Result
                                            FROM Emissions E
                                            JOIN Factors F ON E.Year = F.Year
                                                           AND E.[Emission Source] = F.Source
                                                           AND E.Additional = F.Additional
                                                           AND F.Unit = E.Unit
                                            JOIN Scopes S ON S.FactorsId = F.Id
                                            WHERE S.Scope = '{column}' AND F.Method in ('Market', 'General');";
                    var result = cnn.ExecuteScalar<double?>(sqlQuery);
                    return (double)result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }


        public static List<Factor> GetEmissions()
        {
            using (var cnn = new SQLite.SQLiteConnection(App.databasePath))
            {
                string sql = $@"SELECT * FROM Factors";
                List<Factor> result = cnn.Query<Factor>(sql).ToList();
                return result;
            }

        }
        //na gridzie zaznaczam kalkulacje a musze usunąć emisje
        public static void DeleteEmission(Calculation calc)
        {       
            try
            { 
                using (var cnn = new SQLite.SQLiteConnection(App.databasePath))
                {
                    string sql = $@"DELETE FROM Emissions
                                WHERE Year = '{calc.Year}' and Additional = '0' and Sector = '{calc.Sector}' and [Emission Source] = '{calc.Source}' and Unit = '{calc.Unit}' and Location = '{calc.Location}' and Usage = '{calc.Usage}';";
                    cnn.Execute(sql);

                    if (calc.Source == "Purchased grid electricity")
                    {
                        string additional2 = calc.Location;
                        string sql2 = $@"DELETE FROM Emissions
                                WHERE Year = '{calc.Year}' and Additional = '{additional2}' and Sector = '{calc.Sector}' and [Emission Source] = '{calc.Source}' and Unit = '{calc.Unit}' and Location = '{calc.Location}' and Usage = '{calc.Usage}';";
                        cnn.Execute(sql2);
                    }
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
    }
}
