using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Statistics;
using Nop.Data.Mapping;
using System;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2024/03/19 09:31:00:2551771", "IBaseSyncInsertEntities", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class IBaseSyncInsertEntities20240319 : AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        AddColumnsToTable(typeof(Customer));
        AddColumnsToTable(typeof(OrderItem));
        AddColumnsToTable(typeof(Order));
    }

    private void AddColumnsToTable(Type typeClass)
    {
        var tableName = NameCompatibilityManager.GetTableName(typeClass);

        var columnName = NameCompatibilityManager.GetColumnName(typeClass, "Synchronized");
        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsBoolean()
            .NotNullable()
            .WithDefaultValue(false);
        }
    }
}
