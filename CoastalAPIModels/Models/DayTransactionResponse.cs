﻿using System;
using System.Collections.Generic;
using System.Text;
using CoastalAPIDataLayer.Models;

namespace CoastalAPIModels.Models
{
    public class DayTransactionResponse : GenericResponse
    {
        public List<Transaction> Transactions { get; set; }
    }
}
