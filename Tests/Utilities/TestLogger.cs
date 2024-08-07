using Microsoft.Extensions.Logging;
using Services.EmailService;
using System;
using System.Collections.Generic;

namespace Tests.Utilities
{
    public class TestLogger : ILogger<EmailService>
    {
        public List<string> LogMessages { get; } = new List<string>();

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                LogMessages.Add(formatter(state, exception));
            }
        }
    }
}