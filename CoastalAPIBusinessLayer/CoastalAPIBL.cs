using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoastalAPIDataLayer;

namespace CoastalAPIBusinessLayer
{
    public class CoastalAPIBL
    {
        private readonly string dbConnectionString;
        private readonly ICoastalAPISettings coastalAPISettings;
        public CoastalAPIBL(ICoastalAPISettings coastalAPISettings)
        {
            this.coastalAPISettings = coastalAPISettings;
            this.dbConnectionString = coastalAPISettings.GetCoastalAPIDBConnectionString();
        }
    }
}
