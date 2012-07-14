using System;
using Zac.DesignPattern.Models;

namespace Zac.StandardCore.Models
{
    public class ErrorLog : StandardPersistentObject
    {
        public string UserId { get; set; }
        public string ErrorCategory { get; set; }
        public DateTime ErrorDate { get; set; }
        public string HostIP { get; set; }
        public string ClientIP { get; set; }
        public string ExceptionType { get; set; }
        public string Message { get; set; }
        public string BaseMessage { get; set; }
        public string StackTrace { get; set; }
    }
}
