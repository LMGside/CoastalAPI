using System;
using System.Collections.Generic;
using System.Text;
using CoastalAPIDataLayer.Models;

namespace CoastalAPIModels.Models
{
    public class AddCarRequest
    {
        public string Licence { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public CoastalAPIDataLayer.Models.Asset.AssetType Type { get; set; }
        public bool Auto_Sale { get; set; }
        public int? Auto_Valuation { get; set; }
        public int Normal_Valuation { get; set; }
        public int Owner { get; set; }
    }
}