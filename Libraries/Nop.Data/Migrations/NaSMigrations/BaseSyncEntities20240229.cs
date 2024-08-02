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

[NopMigration("2024/02/29 09:31:00:2551771", "BaseSyncEntity", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class BaseSyncEntities20240229 : AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        AddColumnsToTable(typeof(Address), true);
        AddColumnsToTable(typeof(Category), true);
        AddColumnsToTable(typeof(Customer), true);
        AddColumnsToTable(typeof(CustomerCustomerRoleMapping)); //No en API
        AddColumnsToTable(typeof(OrderItem));
        AddColumnsToTable(typeof(Order), true);
        AddColumnsToTable(typeof(Product), true);
        AddColumnsToTable(typeof(ProductCategory)); //No en API
        AddColumnsToTable(typeof(SellerStatistics));
        AddColumnsToTable(typeof(GenericAttribute)); //No en API
    }

    private void AddColumnsToTable(Type typeClass, bool isBaseSyncEntity2 = false)
    {
        var tableName = NameCompatibilityManager.GetTableName(typeClass);

        var columnName = NameCompatibilityManager.GetColumnName(typeClass, "UpdatedOnUtc");
        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDateTime2()
            .NotNullable()
            .WithDefault(SystemMethods.CurrentUTCDateTime);
        }

        columnName = NameCompatibilityManager.GetColumnName(typeClass, "ExtId");
        if (isBaseSyncEntity2 && !Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsString(50)
            .Nullable();
        }
    }
}
