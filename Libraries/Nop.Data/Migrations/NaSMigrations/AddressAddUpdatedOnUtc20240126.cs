using FluentMigrator;
using Nop.Core.Domain.Common;
using Nop.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2024/01/26 15:53:00:2551771", "Address. Add UpdatedOnUtc", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class AddressAddUpdatedOnUtc20240126 : AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        var tableName = NameCompatibilityManager.GetTableName(typeof(Address));
        var columnName = NameCompatibilityManager.GetColumnName(typeof(Address), nameof(Address.UpdatedOnUtc));

        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDateTime2()
            .WithDefault(SystemMethods.CurrentDateTime);
        }
    }
}
