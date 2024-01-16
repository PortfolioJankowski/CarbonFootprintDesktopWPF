using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CarbonFootprintDesktopApp.Model
{
    [SQLite.Table("Factors")]
    public class Factor
    {
      
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Source { get; set; }
        public string Unit { get; set; }    
        public double Value { get; set; }
        public string Additional { get; set; } 
        public int Year { get; set; }
        public string Database { get; set; }
        public string Scope { get; set; }
        public string Category { get; set; }
        public string Method { get; set; }
    }
}
