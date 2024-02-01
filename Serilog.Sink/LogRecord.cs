using System.ComponentModel.DataAnnotations;

namespace TestSerilogMsSql.Serilog.Sink
{
    public class LogRecord
    {
        public int Id { get; set; }

        public string? Message { get; set; }

        public string? MessageTemplate { get; set; }

        [MaxLength(128)]
        public string? Level { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public string? Exception { get; set; }

        public string? LogEvent { get; set; }
        
    }
}
