using CoastalAPIDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoastalAPIModels.Models
{
    public class DateRangeTransactionsResponse : GenericResponse
    {
        public List<Transaction> Transactions { get; set; }
        
    }
}
