using FluentMigrator;
using Nop.Core.Domain.ScheduleTasks;
using Nop.Data.Mapping;
using System.Linq;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2024/07/23 15:23:00:2551771", "ScheduleTask", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class ScheduleTask20240722 : AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        var typeClass = typeof(ScheduleTask);
        var tableName = NameCompatibilityManager.GetTableName(typeClass);
        var columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(ScheduleTask.InitialDateUtc));

        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDateTime2()
            .Nullable();
        }
    }
}
