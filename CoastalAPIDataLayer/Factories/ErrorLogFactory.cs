using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoastalAPIDataLayer.Models;

namespace CoastalAPIDataLayer.Factories
{
    public class ErrorLogFactory
    {
        private readonly string dbConnectionString;

        public ErrorLogFactory(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public ErrorLog Create(Action<ErrorLog> initalizer)
        {
            var errorLog = new ErrorLog(this.dbConnectionString);
            initalizer(errorLog);
            return errorLog;
        }
    }
}
