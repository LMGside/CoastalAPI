using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Models
{
    public class Car : Asset
    {
        private readonly string dbConnectionString;

        public Car(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public int Car_ID { get; set; }
        public string Licence { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
    }
}
