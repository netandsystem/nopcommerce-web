using FluentMigrator;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.ScheduleTasks;
using Nop.Data.Mapping;
using System.Linq;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2024/07/24 10:34:00:2551771", "Invoice - Account Statements", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class Invoice20240724 : AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        var typeClass = typeof(Invoice);
        var tableName = NameCompatibilityManager.GetTableName(typeClass);

        var columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(Invoice.TaxAmount));
        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDecimal()
            .WithDefaultValue(0);
        }

        columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(Invoice.ShippingAmount));
        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDecimal()
            .WithDefaultValue(0);
        }

        columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(Invoice.TaxExemptAmount));
        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDecimal()
            .WithDefaultValue(0);
        }

        columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(Invoice.DaysNegotiated));
        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsInt32()
            .WithDefaultValue(0);
        }

        columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(Invoice.DiscountNumber));
        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDecimal()
            .WithDefaultValue(0);
        }

        columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(Invoice.DiscountCode));
        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsString()
            .Nullable();
        }

        columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(Invoice.DueDateUtc));
        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDateTime2()
            .Nullable();
        }

        columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(Invoice.TaxBase));
        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDecimal()
            .WithDefaultValue(0);
        }

        columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(Invoice.CreatedOnUtc));
        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Alter.Table(tableName)
                .AlterColumn(columnName)
                .AsDateTime2()
                .NotNullable()
                .WithDefault(FluentMigrator.SystemMethods.CurrentUTCDateTime);
        }
    }
}
