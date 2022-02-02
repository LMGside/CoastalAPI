using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Models
{
    public class Property : Asset
    {
        private readonly string dbConnectionString;

        public Property(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public int Property_ID { get; set; }
        public string Address { get; set; }
        public int SQ { get; set; }
        public PropertyType Property_Type { get; set; }

        public enum PropertyType
        {
            House = 0,
            Apartment = 1,
            Commercial_Building = 2
        }
    }
}
