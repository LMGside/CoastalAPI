using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CoastalAPIDataLayer
{
    public interface ICoastalAPISettings
    {
        string GetCoastalAPIDBConnectionString();
    }

    public class CoastalAPISettings : ICoastalAPISettings
    {
        public string GetCoastalAPIDBConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["CoastalFinanceDB"].ConnectionString;
        }
    }

    public class TestCoastalAPISettings : ICoastalAPISettings
    {
        private readonly string coastalAPIDBConnectionString;

        public TestCoastalAPISettings(string coastalAPIDBConnectionString)
        {
            this.coastalAPIDBConnectionString = coastalAPIDBConnectionString;
        }

        public string GetCoastalAPIDBConnectionString()
        {
            return this.coastalAPIDBConnectionString;
        }
    }
}
