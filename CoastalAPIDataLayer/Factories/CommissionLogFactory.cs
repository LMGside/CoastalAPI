using CoastalAPIDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Factories
{
    public class CommissionLogFactory
    {
        private readonly string dbConnectionString;

        public CommissionLogFactory(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public CommissionLog Create(Action<CommissionLog> initalizer)
        {
            var commissionLog = new CommissionLog(this.dbConnectionString);
            initalizer(commissionLog);
            return commissionLog;
        }
    }
}
