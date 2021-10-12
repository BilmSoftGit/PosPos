using Arebis.Logging.GrayLog;
using Nelibur.ObjectMapper;
using Pospos.Core.Common;
using Pospos.Core.Settings;
using System;
using System.Threading.Tasks;

namespace Pospos.Core.Helpers
{
    public interface ILogHelper
    {
        void PushLog(LogDetailed _log);
        void Information(LogItem item);
        void TraceLog(LogItem item);
        void InternalError(LogItem item);
        void Error(LogItem item);
        void Warning(LogItem item);
    }
    public class LogHelper : ILogHelper
    {
        private readonly AppSettings _setting;
        private readonly ConnectionConfig _connections;
        public LogHelper(AppSettings setting, ConnectionConfig connections)
        {
            _setting = setting;
            _connections = connections;
        }

        public void PushLog(LogDetailed _log)
        {
            Task.Run(() =>
            {
                using (var client = new GrayLogUdpClient(_setting.ApplicationName, _connections.LogConnection, 12201))
                {
                    client.MaxPacketSize = 32768;
                    TinyMapper.Bind<LogDetailed, LogItem>(config => { config.Ignore(x => x.ErrorObject); });
                    var item = TinyMapper.Map<LogItem>(_log);
                    item.RequestObject = _log.RequestObject;
                    item.ResponseObject = _log.ResponseObject;

                    client.Send(item.ShortCode, null, item, _log.ErrorObject);
                }
            });
            
        }

        public void Information(LogItem item)
        {
            Log(new LogDetailed()
            {
                Severity = "Information",
                ShortCode = item.ShortCode,
                Description = item.Description,
                ErrorObject = null,
                ClientEndPoint = item.ClientEndPoint,
                RequestHostAddress = item.RequestHostAddress,
                FirstLevelCategory = item.FirstLevelCategory,
                SecondLevelCategory = item.SecondLevelCategory,
                ThirdLevelCategory = item.ThirdLevelCategory,
                RequestStartDate = item.RequestStartDate,
                RequestEndDate = item.RequestEndDate,
                RequestObject = item.RequestObject,
                ResponseObject = item.ResponseObject,
                FilterKey = item.FilterKey,
                RequestAppName = item.RequestAppName
            });
        }

        public void TraceLog(LogItem item)
        {
            Log(new LogDetailed()
            {
                Severity = "TraceLog",
                ShortCode = item.ShortCode,
                Description = item.Description,
                ErrorObject = null,
                ClientEndPoint = item.ClientEndPoint,
                RequestHostAddress = item.RequestHostAddress,
                FirstLevelCategory = item.FirstLevelCategory,
                SecondLevelCategory = item.SecondLevelCategory,
                ThirdLevelCategory = item.ThirdLevelCategory,
                RequestStartDate = item.RequestStartDate,
                RequestEndDate = item.RequestEndDate,
                RequestObject = item.RequestObject,
                ResponseObject = item.ResponseObject,
                FilterKey = item.FilterKey,
                RequestAppName = item.RequestAppName
            });
        }

        public void InternalError(LogItem item)
        {
            Log(new LogDetailed()
            {
                Severity = "InternalError",
                ShortCode = item.ShortCode,
                Description = item.Description,
                ErrorObject = item.ErrorObject,
                ClientEndPoint = item.ClientEndPoint,
                RequestHostAddress = item.RequestHostAddress,
                FirstLevelCategory = item.FirstLevelCategory,
                SecondLevelCategory = item.SecondLevelCategory,
                ThirdLevelCategory = item.ThirdLevelCategory,
                RequestStartDate = item.RequestStartDate,
                RequestEndDate = item.RequestEndDate,
                RequestObject = item.RequestObject,
                ResponseObject = item.ResponseObject,
                FilterKey = item.FilterKey,
                RequestAppName = item.RequestAppName
            });
        }

        public void Error(LogItem item)
        {
            Log(new LogDetailed()
            {
                Severity = "Error",
                ShortCode = item.ShortCode,
                Description = item.Description,
                ErrorObject = item.ErrorObject,
                ClientEndPoint = item.ClientEndPoint,
                RequestHostAddress = item.RequestHostAddress,
                FirstLevelCategory = item.FirstLevelCategory,
                SecondLevelCategory = item.SecondLevelCategory,
                ThirdLevelCategory = item.ThirdLevelCategory,
                RequestStartDate = item.RequestStartDate,
                RequestEndDate = item.RequestEndDate,
                RequestObject = item.RequestObject,
                ResponseObject = item.ResponseObject,
                FilterKey = item.FilterKey,
                RequestAppName = item.RequestAppName
            });
        }

        public void Warning(LogItem item)
        {
            Log(new LogDetailed()
            {
                Severity = "Warning",
                ShortCode = item.ShortCode,
                Description = item.Description,
                ErrorObject = item.ErrorObject,
                ClientEndPoint = item.ClientEndPoint,
                RequestHostAddress = item.RequestHostAddress,
                FirstLevelCategory = item.FirstLevelCategory,
                SecondLevelCategory = item.SecondLevelCategory,
                ThirdLevelCategory = item.ThirdLevelCategory,
                RequestStartDate = item.RequestStartDate,
                RequestEndDate = item.RequestEndDate,
                RequestObject = item.RequestObject,
                ResponseObject = item.ResponseObject,
                FilterKey = item.FilterKey,
                RequestAppName = item.RequestAppName
            });
        }

        private void Log(LogDetailed item)
        {
            item.Facility = _setting.ApplicationName;
            item.Platform = _setting.Environment;
            item.Machine = Environment.MachineName;
            if (item.RequestStartDate != null && item.RequestEndDate != null)
            {
                item.RequestExecutionTime = (int)item.RequestEndDate.Value.Subtract(item.RequestStartDate.Value).TotalMilliseconds;
            }
            PushLog(item);
        }
    }
}
