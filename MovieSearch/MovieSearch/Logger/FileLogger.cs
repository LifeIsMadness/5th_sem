using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Logger
{
    public class FileLogger : ILogger
    {
        private readonly string _allLogsPath;
        public FileLogger()
        {
            _allLogsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logging", "all");
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Debug;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var msg = DateTime.Now + ":" + formatter(state, exception);

            File.AppendAllText(_allLogsPath, msg + Environment.NewLine);
           // Console.WriteLine($"{msg}");
        }
    }
}
