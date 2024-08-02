using FluentMigrator;
using Nop.Core.Domain.Reporting;
using Nop.Data.Extensions;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2024/04/17 09:31:00:2551771", "Report", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class Report20241704 : AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        Create.TableFor<Report>();
    }
}
