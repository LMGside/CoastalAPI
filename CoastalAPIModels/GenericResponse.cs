using System;

namespace CoastalAPIModels
{
    public class GenericResponse
    {
        public GenericResponse()
        {
            Error = new Error();
        }
        public ResponseStatus Status { get; set; }
        public Error Error { get; set; }
    }

    public enum ResponseStatus

    {
        Pending,
        Success,
        Fail,
        Timeout,
        Error,
        Blocked
    }

    public class Error
    {
        public string ErrorMessage { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
        public string CrashedMethod { get; set; }

    }
}
