using Pospos.Core.Common;
using Pospos.Core.Modules;
using System;
using System.Threading.Tasks;

namespace Pospos.Service
{
    public class BaseService
    {
        private readonly LogManager _logger;
        public BaseService(LogManager logger)
        {
            _logger = logger;
        }

        protected async Task<ResponseBase<T>> ExecuteAsync<T>(Func<Task<T>> action,string desc,string firstLevelCategory,string secondLevelCategory,string thirdLevelCategory,bool responseToLog,object request = null)
        {
            ResponseBase<T> model = new ResponseBase<T>();
            DateTime beginning = DateTime.Now;
            try
            {
                model.data = await action();
                _logger.Infomation(new LogObject(desc
                    , null
                    , firstLevelCategory
                    , secondLevelCategory
                    , thirdLevelCategory
                    , beginning
                    , DateTime.Now
                    , request
                    , (responseToLog ? model.data : (object)null)));
            }
            catch (Exception ex)
            {
                _logger.Error(new LogObject(desc
                    , ex
                    , firstLevelCategory
                    , secondLevelCategory
                    , thirdLevelCategory
                    , beginning
                    , DateTime.Now
                    , request));
                model.ErrorList.Add(ex.Message);
            }
            return model;
        }
    }
}
