using Serilog;
using Serilog.Configuration;

namespace TestSerilogMsSql.Serilog.Sink
{
    public static class EntityFrameworkCoreSinkExtensions
    {
        public static LoggerConfiguration EntityFrameworkSink(
                 this LoggerSinkConfiguration loggerConfiguration,
                 LogDbContext dbContextProvider,
                 IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new EntityFrameworkCoreSink(dbContextProvider, formatProvider));
        }
    }
}
