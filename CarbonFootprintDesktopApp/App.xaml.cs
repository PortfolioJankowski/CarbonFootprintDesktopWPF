using CarbonFootprintDesktopApp.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CarbonFootprintDesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string databaseName = "CarbonFootprint.db";
        public static string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, databaseName);

        public App()
        {
           
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(databasePath))
            {
                conn.CreateTable<Emission>();
                conn.CreateTable<Factor>();
            }
            base.OnStartup(e);
        }
    }
}
