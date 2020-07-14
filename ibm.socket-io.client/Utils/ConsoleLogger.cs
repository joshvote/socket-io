using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBM.SocketIO.Utils
{
    /// <summary>
    /// Represents an absolute barebones Console implementation of ILogger
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConsoleLogger<T> : ILogger<T>
    {
        private class LogScope : IDisposable
        {
            public void Dispose()
            { }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new LogScope();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            switch(logLevel)
            {
                case LogLevel.Critical:
                case LogLevel.Error:
                    Console.Error.WriteLine(formatter(state, exception));
                    break;
                default:
                    Console.Out.WriteLine(formatter(state, exception));
                    break;
            }
        }
    }
}
