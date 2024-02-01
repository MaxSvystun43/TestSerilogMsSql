using Microsoft.EntityFrameworkCore;
using TestSerilogMsSql.Serilog.Sink.Configuration;

namespace TestSerilogMsSql.Serilog.Sink
{
    public class LogDbContext : DbContext
    {
        public DbSet<LogRecord> LogRecords { get; set; }

        public LogDbContext() { } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conn = "Server=localhost;Database=rscore;User ID=rscore;Password=RsCore2022";

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(conn);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("log");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogRecordConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
