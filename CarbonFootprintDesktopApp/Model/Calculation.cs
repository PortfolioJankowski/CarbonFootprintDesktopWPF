
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarbonFootprintDesktopApp.Model
{
    public class Calculation
    {
       public int Id { get; set; }
        public int Year { get; set; }
        public string Sector { get; set; }
        public string Scope { get; set; }
        public string Category { get; set; }
        public string Source { get; set; }
        public string Location { get; set; }
        public string Unit { get; set; }
        public double Usage { get; set; }
        public double Result { get; set; }
        public string Additional { get; set; }
        public string Method { get; set; }
    }
}
