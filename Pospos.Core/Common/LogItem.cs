using System;

namespace Pospos.Core.Common
{
    public class LogItem
    {
        public string ShortCode { get; set; }
        public string FilterKey { get; set; }
        public string Severity { get; set; }
        public string Platform { get; set; }
        public string ClientEndPoint { get; set; }
        public string Description { get; set; }
        public Exception ErrorObject { get; set; }
        public string RequestHostAddress { get; set; }
        public string FirstLevelCategory { get; set; }
        public string SecondLevelCategory { get; set; }
        public string ThirdLevelCategory { get; set; }
        public DateTime? RequestStartDate { get; set; }
        public DateTime? RequestEndDate { get; set; }
        public object RequestObject { get; set; }
        public object ResponseObject { get; set; }
        public int? RequestExecutionTime { get; set; }
        public string RequestAppName { get; set; }
        public LogItem()
        {
            ShortCode = Guid.NewGuid().ToString();
        }
    }

    public class LogDetailed
    {
        public string ShortCode { get; set; }
        public string FilterKey { get; set; }
        public string Facility { get; set; }
        public string Machine { get; set; }
        public string Platform { get; set; }
        public string ClientEndPoint { get; set; }
        public string Description { get; set; }
        public Exception? ErrorObject { get; set; }
        public string Severity { get; set; }
        public string RequestHostAddress { get; set; }
        public string FirstLevelCategory { get; set; }
        public string SecondLevelCategory { get; set; }
        public string ThirdLevelCategory { get; set; }
        public DateTime? RequestStartDate { get; set; }
        public DateTime? RequestEndDate { get; set; }
        public object RequestObject { get; set; }
        public object ResponseObject { get; set; }
        public int? RequestExecutionTime { get; set; }
        public string RequestAppName { get; set; }
        public LogDetailed()
        {
            this.ErrorObject = null;
        }
    }
}
