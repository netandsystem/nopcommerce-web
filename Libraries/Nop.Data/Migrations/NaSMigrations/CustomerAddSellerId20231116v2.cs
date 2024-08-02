using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2023/11/16 12:00:00:2551771", "Customer. Add SellerIdv2", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class CustomerAddSellerId20231116v2: AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        var tableName = NameCompatibilityManager.GetTableName(typeof(Customer));
        var columnName = NameCompatibilityManager.GetColumnName(typeof(Customer), nameof(Customer.SellerId));

        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
                .OnTable(tableName)
                .AsInt32()
                .Nullable();
        }

    }
}
