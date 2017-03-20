using System;
using AsyncFriendlyStackTrace;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;

namespace BLL.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static void LogException(this ILogger logger, LogLevel logLevel, Exception ex, params object[] args)
        {
            var format = new Func<object, Exception, string>(MessageFormatter);
            logger.Log(logLevel, 0, new FormattedLogValues(ex.ToAsyncString(), args), null, format);
        }

        private static string MessageFormatter(object state, Exception error) => state.ToString();
    }
}
