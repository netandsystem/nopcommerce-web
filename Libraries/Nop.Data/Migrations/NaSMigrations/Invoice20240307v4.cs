using FluentMigrator;
using FluentMigrator.Runner.Generators.Base;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Statistics;
using Nop.Data.Extensions;
using Nop.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2024/03/07 16:04:00:2551771", "Invoice Entity v4", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class Invoice20240307v4 : AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        var tableName = NameCompatibilityManager.GetTableName(typeof(Invoice));
        if (!Schema.Table(tableName).Exists())
        {
            Create.TableFor<Invoice>();
        }
    }
}
