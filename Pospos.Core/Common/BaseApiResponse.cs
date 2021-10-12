using System;
using System.Collections.Generic;
using System.Text;

namespace Pospos.Core.Common
{
    public class BaseApiResponse
    {
        public BaseApiResponse()
        {
            ErrorList = new List<string>();
        }
        public List<String> ErrorList { get; set; }
        public bool Status => ErrorList == null || ErrorList.Count == 0;
    }
    public class ResponseBase<T>: BaseApiResponse
    {
        public T data { get; set; }
    }
}
