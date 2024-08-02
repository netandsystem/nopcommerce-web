using FluentMigrator;
using Nop.Core.Domain.Orders;
using Nop.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2024/01/29 20:42:00:2551771", "OrderItem. Add UpdatedOnUtc", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class OrderItemAddUpdatedOnUtc20240129 : AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        var tableName = NameCompatibilityManager.GetTableName(typeof(OrderItem));

        var columnName = NameCompatibilityManager.GetColumnName(typeof(OrderItem), nameof(OrderItem.UpdatedOnUtc));

        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDateTime2()
            .WithDefault(SystemMethods.CurrentDateTime);
        }
    }
}
