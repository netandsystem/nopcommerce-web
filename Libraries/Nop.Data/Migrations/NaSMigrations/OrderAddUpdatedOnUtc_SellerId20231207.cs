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

[NopMigration("2023/12/07 20:42:00:2551771", "Orders. Add UpdatedOnUtcv2 Add SellerId", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class OrderAddUpdatedOnUtc_SellerId20231207 : AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        var tableName = NameCompatibilityManager.GetTableName(typeof(Order));

        var columnName = NameCompatibilityManager.GetColumnName(typeof(Order), nameof(Order.UpdatedOnUtc));

        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDateTime2()
            .WithDefault(SystemMethods.CurrentDateTime);
        }

        columnName = NameCompatibilityManager.GetColumnName(typeof(Order), nameof(Order.SellerId));

        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
                .OnTable(tableName)
                .AsInt32()
                .Nullable();
        }

        columnName = NameCompatibilityManager.GetColumnName(typeof(Order), nameof(Order.OrderManagerGuid));

        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
                .OnTable(tableName)
                .AsGuid()
                .Nullable();
        }
    }
}
