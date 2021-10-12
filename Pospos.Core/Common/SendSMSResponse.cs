using System.Collections.Generic;

namespace Pospos.Core.Common
{
    public class SendSMSResponse
    {
        public string Error { get; set; }
        public List<Result> Results { get; set; }
    }

    public class Result
    {
        public string Status { get; set; }
        public string MessageID { get; set; }
    }
}
