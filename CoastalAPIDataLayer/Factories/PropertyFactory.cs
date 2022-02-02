using CoastalAPIDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Factories
{
    public class PropertyFactory
    {
        private readonly string dbConnectionString;
        public PropertyFactory(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public Property Create(Action<Property> initalizer)
        {
            var property = new Property(this.dbConnectionString);
            initalizer(property);
            return property;
        }
    }
}
