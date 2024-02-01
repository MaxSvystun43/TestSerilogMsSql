using Microsoft.EntityFrameworkCore;
using TestSerilogMsSql.Serilog.Sink;

namespace TestSerilogMsSql
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseDb(this DbContextOptionsBuilder builder, DatabaseSettings configuration)
        {

            ArgumentNullException.ThrowIfNull(configuration);

            return configuration.DatabaseType switch
            {
                DatabaseType.PostgreSql => builder.UseNpgsql(configuration.ConnectionString).UseSnakeCaseNamingConvention(),
                DatabaseType.SqlServer => builder.UseSqlServer(configuration.ConnectionString).UseSnakeCaseNamingConvention(),
                DatabaseType.Oracle => builder.UseOracle(configuration.ConnectionString).UseUpperSnakeCaseNamingConvention(),
                DatabaseType.SQLite => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException($"Invalid DB type: '{configuration.DatabaseType}")
            };
        }
    }
}
