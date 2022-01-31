using System;
using System.Collections.Generic;
using System.Text;

namespace CoastalAPIModels.Models
{
    public class ErrorLogCtrl
    {
        public DateTime CreatedDate { get; set; }
        public string ErrorMessage { get; set; }
        public string Exception { get; set; }
        public string CrashMethod { get; set; }
        public string StackTrace { get; set; }

    }
}
