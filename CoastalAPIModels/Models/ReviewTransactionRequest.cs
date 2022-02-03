using System;
using System.Collections.Generic;
using System.Text;

namespace CoastalAPIModels.Models
{
    public class ReviewTransactionRequest
    {
        public int TransactionID { get; set; }
        public int AssetID { get; set; }
        public int BuyerID { get; set; }
        public TransactionStatus Decision { get; set; }

        public enum TransactionStatus
        {
            Success = 0,
            Fail = 1,
            Approved = 2,
            Rejected = 3
        }
    }
}
