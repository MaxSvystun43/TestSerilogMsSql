using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSerilogMsSql.Serilog.Sink.Configuration
{
    public class LogRecordConfiguration : IEntityTypeConfiguration<LogRecord>
    {
        public void Configure(EntityTypeBuilder<LogRecord> builder)
        {
            builder.ToTable("LogRecords");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);
            builder.Property(x => x.Level);
            builder.Property(x => x.Message);
            builder.Property(x => x.MessageTemplate);
            builder.Property(x => x.Exception);
            builder.Property(x => x.TimeStamp);
            builder.Property(x => x.LogEvent);
        }
    }
}
