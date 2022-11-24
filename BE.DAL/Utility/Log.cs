using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace BE.DAL.Utility
{
    public class Log
    {
        private static Logger _logger;
        public Logger GetLogger()
        {
            if (_logger == null)
            {
                IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: false).AddEnvironmentVariables()
                    .Build();
                LogManager.Configuration = new NLogLoggingConfiguration(configurationRoot.GetSection("NLog"));
                _logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();
            }

            return _logger;
        } 
    }
}
