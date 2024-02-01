using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSerilogMsSql.Serilog.Sink
{
    /// <summary>
    /// Represents the database settings model
    /// </summary>
    public sealed class DatabaseSettings
    {
        /// <summary>
        /// The <see cref="DatabaseType"/>
        /// </summary>
        public DatabaseType DatabaseType { get; set; }

        /// <summary>
        /// Connection string
        /// </summary>
        public string ConnectionString { get; set; } = default!;

        /// <summary>
        /// Default schema name
        /// </summary>
        public string SchemaName { get; set; } = default!;
    }
}
