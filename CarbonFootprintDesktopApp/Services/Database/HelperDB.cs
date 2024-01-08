using CarbonFootprintDesktopApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CarbonFootprintDesktopApp.Database
{
    class HelperDB
    {
        //TODO generyczny insert/update/delete/read/
        //TODO calculate
        public static double GetResult()
        {
           return 0;
        }

        public static bool Insert<T>(T item)
        {
            bool result = false;
            using(SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<T>();
                int rows = conn.Insert(item);
                if (rows > 0) result = true;
            }
            return result;
        }

        public static bool Update<T>(T item)
        {
            bool result = false;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<T>();
                int rows = conn.Update(item);
                if (rows > 0) result = true;
            }
            return result;
        }

        public static bool Delete<T>(T item)
        {
            bool result = false;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<T>();
                int rows = conn.Delete(item);
                if (rows > 0) result = true;
            }
            return result;
        }

        public static List<T> Read<T>(T item) where T : new()
        {
            List<T> items;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<T>();
                items = conn.Table<T>().ToList();
            }
            return items;
        }

        public static IEnumerable<Calculation> GetCalculations()
        {
            IEnumerable<Calculation> calculations;
            using(var connection = new SQLiteConnection(App.databasePath))
            {
                var query = @"
            SELECT
            E.Year,
            E.Sector,
            E.Scope,
            E.Category,
            E.Additional,
            E.[Emission Source] as Source, 
            E.Location,
            E.Unit,
            E.Usage,
            E.Usage * F.Value as Result
            FROM    
                Emissions E
            JOIN Factors F 
                ON E.Year = F.Year
                    AND E.[Emission Source] = F.Source  
                    AND E.Additional = F.Additional
                    AND F.Unit = E.Unit;
                                                ";

                calculations = connection.Query<Calculation>(query);
                return calculations;
            }
        }
    }
}
