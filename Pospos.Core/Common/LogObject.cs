using System;
using System.Collections.Generic;
using System.Text;

namespace Pospos.Core.Common
{
    public class LogObject
    {
        public LogObject()
        {

        }
        public LogObject(string description,Exception error,string firstlevel,string secondlevel,string thirdlevel,DateTime? startDate = null,DateTime? endDate = null,object request = null,object response = null)
        {
            Description = description;
            ErrorObject = error;
            FirstLevelCategory = firstlevel;
            SecondLevelCategory = secondlevel;
            ThirdLevelCategory = thirdlevel;
            RequestStartDate = startDate;
            RequestEndDate = endDate;
            Request = request;
            Response = response;
        }
        public string Description { get; set; }
        public Exception ErrorObject { get; set; }
        public string FirstLevelCategory { get; set; }
        public string SecondLevelCategory { get; set; }
        public string ThirdLevelCategory { get; set; }
        public DateTime? RequestStartDate { get; set; }
        public DateTime? RequestEndDate { get; set; }
        public object Request { get; set; }
        public object Response { get; set; }
    }
}
