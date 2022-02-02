using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Models
{
    public class Asset
    {
        private readonly string dbConnectionString;

        public Asset(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public Asset()
        {
            this.dbConnectionString = ConfigurationManager.ConnectionStrings["CoastalFinanceDB"].ConnectionString;
        }

        public int ID { get; set; }
        public AssetType Type { get; set; }
        public bool Auto_Sale { get; set; }
        public int Auto_Valuation { get; set; }
        public int Normal_Valuation { get; set; }
        public int Owner { get; set; }

        public enum AssetType
        {
            Property = 0,
            Car = 1,
            Art = 2
        }
    }
}
