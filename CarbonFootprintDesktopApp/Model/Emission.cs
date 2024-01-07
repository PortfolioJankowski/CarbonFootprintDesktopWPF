using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColumnAttribute = SQLite.ColumnAttribute;

namespace CarbonFootprintDesktopApp.Model
{
    [SQLite.Table("Emissions")]
    public class Emission
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Year { get; set; }
        public string Sector {  get; set; }
        public string Scope { get; set; }
        public string Category { get; set; }

        [Column("Emission source")]
        public string Source { get; set; }
        public string Location { get; set; }
        public string Unit { get; set; }
        public double Usage { get; set; }

    }
}
