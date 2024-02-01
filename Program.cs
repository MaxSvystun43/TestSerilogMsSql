using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System.Data.SqlClient;
using System.Globalization;
using TestSerilogMsSql.Serilog.Sink;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using TestSerilogMsSql.Migrations;

namespace CustomLogEventFormatterDemo
{
    public static class Program
    {
        private const string _connectionString = "Server=localhost;Database=rscore;User ID=rscore;Password=RsCore2022";
        private const string _schemaName = "logs";
        private const string _tableName = "LogEvents";

        public static void Main()
        {
            using (var serviceProvider = CreateServices())
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
            var options = new ColumnOptions();
            var levelSwitch = new LoggingLevelSwitch();
            var services = new ServiceCollection();

            services.AddDbContext<LogDbContext>();

            var _appDbContext = new LogDbContext();
            // New MSSqlServerSinkOptions based interface
            Log.Logger = new LoggerConfiguration()
                .WriteTo.EntityFrameworkSink(_appDbContext, CultureInfo.CurrentCulture)
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Debug("Getting started");

                Log.Information("Hello {Name} from thread {ThreadId}", "Max", Environment.CurrentManagedThreadId);

                Log.Warning("No coins remain at position {@Position}", new { Lat = 25, Long = 134 });

                UseLevelSwitchToModifyLogLevelDuringRuntime(levelSwitch);

                Fail();
            }
            catch (DivideByZeroException e)
            {
                Log.Error(e, "Division by zero");
            }

            var data = _appDbContext.LogRecords.ToList();

            foreach(var column in data)
            {
                Console.WriteLine(column.Message);
            }
            
            
            Log.CloseAndFlush();
        }

        private static void UseLevelSwitchToModifyLogLevelDuringRuntime(LoggingLevelSwitch levelSwitch)
        {
            levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Error;

            Log.Information("This should not be logged");

            Log.Error("This should be logged");

            levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Information;

            Log.Information("This should be logged again");
        }

        private static void Fail()
        {
            throw new DivideByZeroException();
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </summary>
        private static ServiceProvider CreateServices()
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(_connectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(Migration_20240201).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        /// <summary>
        /// Update the database
        /// </summary>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
    }
}