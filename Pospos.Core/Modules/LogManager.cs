using Pospos.Core.Common;
using Pospos.Core.Helpers;
using Pospos.Core.Settings;
using System;
using System.IO;

namespace Pospos.Core.Modules
{
    public class LogManager
    {
        private readonly ILogHelper _logger;
        private readonly AppSettings _settings;
        private readonly SecurityHelper _securityHelper;
        public LogManager(ILogHelper logger, AppSettings settings, SecurityHelper securityHelper)
        {
            _logger = logger;
            _settings = settings;
            _securityHelper = securityHelper;
        }

        public void Infomation(LogObject log)
        {
            _logger.Information(new LogItem()
            {
                ClientEndPoint = _securityHelper.GetWebHostAddress(),
                RequestHostAddress = System.Net.Dns.GetHostName(),
                Description = log.Description,
                FirstLevelCategory = log.FirstLevelCategory,
                SecondLevelCategory = log.SecondLevelCategory,
                ThirdLevelCategory = log.ThirdLevelCategory,
                RequestStartDate = log.RequestStartDate,
                RequestEndDate = log.RequestEndDate,
                RequestObject = log.Request,
                ResponseObject = log.Response,
                FilterKey =""
            });
        }

        public void TraceLog(LogObject log)
        {
            _logger.TraceLog(new LogItem()
            {
                ClientEndPoint = _securityHelper.GetWebHostAddress(),
                RequestHostAddress = System.Net.Dns.GetHostName(),
                Description = log.Description,
                FirstLevelCategory = log.FirstLevelCategory,
                SecondLevelCategory = log.SecondLevelCategory,
                ThirdLevelCategory = log.ThirdLevelCategory,
                RequestStartDate = log.RequestStartDate,
                RequestEndDate = log.RequestEndDate,
                RequestObject = log.Request,
                ResponseObject = log.Response,
                FilterKey = ""
            });
        }


        public void Infomation(string description)
        {
            _logger.Information(new LogItem()
            {
                ClientEndPoint = _securityHelper.GetWebHostAddress(),
                RequestHostAddress = System.Net.Dns.GetHostName(),
                Description = description,
                FilterKey = ""
            });
        }

        public void Error(string description,Exception exception = null,object request = null)
        {
            _logger.Error(new LogItem()
            {
                ClientEndPoint = _securityHelper.GetWebHostAddress(),
                RequestHostAddress = System.Net.Dns.GetHostName(),
                ErrorObject = exception,
                Description = description,
                RequestObject = request,
                FilterKey = ""
            });
        }

        public void Error(LogObject log)
        {
            _logger.Error(new LogItem()
            {
                ClientEndPoint = _securityHelper.GetWebHostAddress(),
                RequestHostAddress = System.Net.Dns.GetHostName(),
                ErrorObject = log.ErrorObject,
                Description = log.Description,
                FirstLevelCategory = log.FirstLevelCategory,
                SecondLevelCategory = log.SecondLevelCategory,
                ThirdLevelCategory = log.ThirdLevelCategory,
                RequestStartDate = log.RequestStartDate,
                RequestEndDate = log.RequestEndDate,
                RequestObject = log.Request,
                ResponseObject = log.Response,
                FilterKey = ""
            });
        }

        public void InternalError(LogObject log)
        {
            _logger.InternalError(new LogItem()
            {
                ClientEndPoint = _securityHelper.GetWebHostAddress(),
                RequestHostAddress = System.Net.Dns.GetHostName(),
                ErrorObject = log.ErrorObject,
                Description = log.Description,
                FirstLevelCategory = log.FirstLevelCategory,
                SecondLevelCategory = log.SecondLevelCategory,
                ThirdLevelCategory = log.ThirdLevelCategory,
                RequestStartDate = log.RequestStartDate,
                RequestEndDate = log.RequestEndDate,
                RequestObject = log.Request,
                ResponseObject = log.Response,
                FilterKey = ""
            });
        }

        public void Warning(LogObject log)
        {
            _logger.Warning(new LogItem()
            {
                ClientEndPoint = _securityHelper.GetWebHostAddress(),
                RequestHostAddress = System.Net.Dns.GetHostName(),
                ErrorObject = log.ErrorObject,
                Description = log.Description,
                FirstLevelCategory = log.FirstLevelCategory,
                SecondLevelCategory = log.SecondLevelCategory,
                ThirdLevelCategory = log.ThirdLevelCategory,
                RequestStartDate = log.RequestStartDate,
                RequestEndDate = log.RequestEndDate,
                RequestObject = log.Request,
                ResponseObject = log.Response,
                FilterKey = ""
            });
        }

        public void Warning(string description,Exception exception)
        {
            _logger.Warning(new LogItem()
            {
                ClientEndPoint = _securityHelper.GetWebHostAddress(),
                RequestHostAddress = System.Net.Dns.GetHostName(),
                ErrorObject = exception,
                Description = description,
                FilterKey = ""
            });
        }

        public void Warning(string shortCode,string description, Exception exception=null)
        {
            _logger.Warning(new LogItem()
            {
                ShortCode = shortCode,
                ClientEndPoint = _securityHelper.GetWebHostAddress(),
                RequestHostAddress = System.Net.Dns.GetHostName(),
                ErrorObject = exception,
                Description = description,
                FilterKey = ""
            });
        }

    }
}
