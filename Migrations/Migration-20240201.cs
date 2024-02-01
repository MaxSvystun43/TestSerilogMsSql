using FluentMigrator;

namespace TestSerilogMsSql.Migrations
{
    [Migration(20240201)]
    public class Migration_20240201 : Migration
    {
        private readonly string SchemaName = "log";
        public override void Up()
        {
            if(!Schema.Schema(SchemaName).Exists())
                Create.Schema(SchemaName);


            Create.Table("LogRecords").InSchema(SchemaName)
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Message").AsString().Nullable()
                .WithColumn("MessageTemplate").AsString().Nullable()
                .WithColumn("Level").AsString().Nullable()
                .WithColumn("TimeStamp").AsDateTime()
                .WithColumn("Exception").AsString().Nullable()
                .WithColumn("LogEvent").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Table("LogRecorts");
        }
    }
}
