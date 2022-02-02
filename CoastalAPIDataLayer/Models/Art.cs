using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Models
{
    public class Art
    {
        private readonly string dbConnectionString;

        public Art(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public int Art_ID { get; set; }
        public string Artist { get; set; }
        public string ArtTitle { get; set; }
        public int Year_Completed { get; set; }
    }
}
