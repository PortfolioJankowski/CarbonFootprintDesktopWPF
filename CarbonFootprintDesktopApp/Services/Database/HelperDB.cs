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

        public static bool Update(Calculation item, Emission emission)
        {
            //usuwam selected calculation
            DeleteEmission(item);
            //dodaje nową emisję -> utworzoną z kalkulacji
            InsertEmission(emission);
            return true;
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
        //na gridzie zaznaczam kalkulacje a musze usunąć emisje
        public static void DeleteEmission(Calculation calc)
        {       
            try
            { 
                using (var cnn = new SQLite.SQLiteConnection(App.databasePath))
                {
                    //usuwam kalkulację, a jeżeli jest to energia to usuwam też location
                    if (calc.Source != "Purchased grid electricity")
                    {
                        cnn.Delete(calc);
                    }
                    else
                    {
                        cnn.Delete(calc);
                        string locationSql = $@"DELETE FROM Calculation
                                               WHERE Year = '{calc.Year}' and Source = '{calc.Source}' and Unit = '{calc.Unit}' and Location = '{calc.Location}' and Usage = '{calc.Usage}' and Sector = '{calc.Sector}' and Method = 'Location' ";
                        cnn.Execute(locationSql);
                    }

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
