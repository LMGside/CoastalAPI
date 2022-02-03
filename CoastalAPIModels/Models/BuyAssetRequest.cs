using System;
using System.Collections.Generic;
using System.Text;

namespace CoastalAPIModels.Models
{
    public class BuyAssetRequest
    {
        public int Asset_ID { get; set; }
        public string Buyer_ID { get; set; }
        public decimal Purchase_Price { get; set; }
    }
}
