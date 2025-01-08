using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace BaseApp.Server.Settings
{
    public class SerilogSettings
    {
        public MSSqlServerSinkOptions MSSqlSinkOptions = new MSSqlServerSinkOptions()
        {
            SchemaName = "App",
            TableName = "Log",
            AutoCreateSqlTable = false,
            BatchPostingLimit = 50,
            BatchPeriod = TimeSpan.FromSeconds(5),
            UseSqlBulkCopy = true,
        };

        public ColumnOptions GetColumnOptions()
        {
            var columnOptions = new ColumnOptions();

            // Removing all the default columns that are not needed
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);

            // Adding all the custom columns
            columnOptions.AdditionalColumns = new List<SqlColumn>
            {
                new SqlColumn { DataType = SqlDbType.NVarChar, ColumnName = "MachineName", DataLength = 200, AllowNull = true },
                new SqlColumn { DataType = SqlDbType.NVarChar, ColumnName = "Application", DataLength = 200, AllowNull = true },
                new SqlColumn { DataType = SqlDbType.Int, ColumnName = "ThreadId", AllowNull = true },
                new SqlColumn { DataType = SqlDbType.UniqueIdentifier, ColumnName = "CorrelationId", AllowNull = true },
                new SqlColumn { DataType = SqlDbType.NVarChar, ColumnName = "UserName", DataLength = 200, AllowNull = true },
                new SqlColumn { DataType = SqlDbType.Int, ColumnName = "ProcessId", AllowNull = true },
                new SqlColumn { DataType = SqlDbType.NVarChar, ColumnName = "ProcessName", DataLength = 200, AllowNull = true },
                new SqlColumn { DataType = SqlDbType.NVarChar, ColumnName = "ClientIp", DataLength = 50, AllowNull = true },
                new SqlColumn { DataType = SqlDbType.NVarChar, ColumnName = "RequestHeader", DataLength = 2000, AllowNull = true }
            };

            return columnOptions;
        }
    }
}
