using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Mapping;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2024/02/29 20:42:00:2551771", "Customer. Add Balance", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class CustomerBalance20240229 : AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        var tableName = NameCompatibilityManager.GetTableName(typeof(Customer));
        var columnName = NameCompatibilityManager.GetColumnName(typeof(Customer), nameof(Customer.Balance));

        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDecimal()
            .WithDefaultValue(0);
        }
    }
}
