using FluentMigrator;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Reporting;
using Nop.Data.Mapping;
using System.Data;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2024/05/13 09:31:00:2551771", "QueuedEmai and Report", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class Report20240513 : AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        //Report

        var typeClass = typeof(Report);
        var tableName = NameCompatibilityManager.GetTableName(typeClass);
        var columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(Report.QueuedEmailId));

        var primaryTableName = NameCompatibilityManager.GetTableName(typeof(QueuedEmail));
        var primaryColumnName = NameCompatibilityManager.GetColumnName(typeof(QueuedEmail), nameof(QueuedEmail.Id));

        Alter.Column(columnName)
            .OnTable(tableName)
            .AsInt32()
            .ForeignKey(primaryTableName, primaryColumnName)
            .OnDelete(Rule.SetNull)
            .Nullable();
    }
}
