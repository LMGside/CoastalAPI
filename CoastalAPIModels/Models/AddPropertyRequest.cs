using System;
using System.Collections.Generic;
using System.Text;

namespace CoastalAPIModels.Models
{
    public class AddPropertyRequest
    {
        public string Address { get; set; }
        public int SQ { get; set; }
        public CoastalAPIDataLayer.Models.Property.PropertyType Property_Type { get; set; }
        public CoastalAPIDataLayer.Models.Asset.AssetType Type { get; set; }
        public bool Auto_Sale { get; set; }
        public int? Auto_Valuation { get; set; }
        public int Normal_Valuation { get; set; }
        public int Owner { get; set; }
    }
}
