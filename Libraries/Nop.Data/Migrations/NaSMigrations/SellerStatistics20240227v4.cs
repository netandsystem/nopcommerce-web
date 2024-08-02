using FluentMigrator;
using FluentMigrator.Runner.Generators.Base;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
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

[NopMigration("2024/02/27 09:31:00:2551771", "Seller Statistics v4", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class SellerStatistics20240227v4 : AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        Create.TableFor<SellerStatistics>();
    }
}
