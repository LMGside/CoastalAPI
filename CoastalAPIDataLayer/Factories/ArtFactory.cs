using CoastalAPIDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Factories
{
    public class ArtFactory
    {
        private readonly string dbConnectionString;
        public ArtFactory(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public Art Create(Action<Art> initalizer)
        {
            var art = new Art(this.dbConnectionString);
            initalizer(art);
            return art;
        }
    }
}
