using FluentMigrator;
using Nop.Core.Domain.Orders;
using Nop.Data.Mapping;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2024/08/06 10:34:00:2551771", "Invoice - Drop DaysNegotiated", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class Invoice20240806 : Migration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        var typeClass = typeof(Invoice);
        var tableName = NameCompatibilityManager.GetTableName(typeClass);

        var columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(Invoice.ExchangeRate));
        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDecimal()
            .WithDefaultValue(1);
        }

        columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(Invoice.DaysNegotiated));
        if (Schema.Table(tableName).Column(columnName).Exists())
        {
            //Drop the column
            Delete.Column(columnName).FromTable(tableName);
        }
    }

    public override void Down()
    {
        //add the downgrade logic if necessary 
    }
}
