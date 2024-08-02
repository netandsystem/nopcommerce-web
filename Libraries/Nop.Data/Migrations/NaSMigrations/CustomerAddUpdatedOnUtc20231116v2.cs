using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2023/11/16 20:42:00:2551771", "Customer. Add UpdatedOnUtcv2", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class CustomerAddUpdatedOnUtc20231116v2: AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        var tableName = NameCompatibilityManager.GetTableName(typeof(Customer));
        var columnName = NameCompatibilityManager.GetColumnName(typeof(Customer), nameof(Customer.UpdatedOnUtc));

        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDateTime2()
            .WithDefault(SystemMethods.CurrentDateTime);
        }
    }
}
