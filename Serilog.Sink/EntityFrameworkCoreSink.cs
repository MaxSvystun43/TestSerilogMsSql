using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using Serilog.Core;

namespace TestSerilogMsSql.Serilog.Sink
{
    public class EntityFrameworkCoreSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;
        private readonly DbContext _dbContextProvider;
        private readonly JsonFormatter _jsonFormatter;
        static readonly object _lock = new object();

        public EntityFrameworkCoreSink(DbContext dbContextProvider, IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
            _dbContextProvider = dbContextProvider ?? throw new ArgumentNullException(nameof(dbContextProvider));
            _jsonFormatter = new JsonFormatter(formatProvider: formatProvider);
        }

        public void Emit(LogEvent logEvent)
        {
            lock (_lock)
            {
                ArgumentNullException.ThrowIfNull(logEvent);

                try
                {
                    DbContext context = _dbContextProvider;

                    if (context != null)
                    {
                        context.Set<LogRecord>().Add(ConvertLogEventToLogRecord(logEvent));

                        context.SaveChanges();
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        private LogRecord? ConvertLogEventToLogRecord(LogEvent logEvent)
        {
            ArgumentNullException.ThrowIfNull(logEvent);

            string json = ConvertLogEventToJson(logEvent);

            var jObject = JObject.Parse(json);

            return new LogRecord
            {
                Exception = logEvent.Exception?.ToString(),
                Level = logEvent.Level.ToString(),
                LogEvent = json,
                Message = _formatProvider == null ? null : logEvent.RenderMessage(_formatProvider),
                MessageTemplate = logEvent.MessageTemplate?.ToString(),
                TimeStamp = logEvent.Timestamp.DateTime.ToUniversalTime(),
            };
        }

        private string ConvertLogEventToJson(LogEvent logEvent)
        {
            ArgumentNullException.ThrowIfNull(logEvent);
            
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                _jsonFormatter.Format(logEvent, writer);
            }

            return sb.ToString();
        }
    }
}
