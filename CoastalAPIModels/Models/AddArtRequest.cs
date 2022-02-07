using System;
using System.Collections.Generic;
using System.Text;

namespace CoastalAPIModels.Models
{
    public class AddArtRequest
    {
        public string Artist { get; set; }
        public string Art_Title { get; set; }
        public int Art_Year { get; set; }
        public CoastalAPIDataLayer.Models.Asset.AssetType Type { get; set; }
        public bool Auto_Sale { get; set; }
        public int? Auto_Valuation { get; set; }
        public int Normal_Valuation { get; set; }
        public int Owner { get; set; }
    }
}
