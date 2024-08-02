using FluentMigrator;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Reporting;
using Nop.Data.Mapping;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2024/05/13 09:31:00:2551771", "QueuedEmai and Report", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class QueuedEmail20240513 : AutoReversingMigration
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        //QueuedEmail

        var typeClass = typeof(QueuedEmail);
        var tableName = NameCompatibilityManager.GetTableName(typeClass);
        var columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(QueuedEmail.UpdatedOnUtc));
        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsDateTime2()
            .NotNullable()
            .WithDefault(SystemMethods.CurrentUTCDateTime);
        }

        //Report

        typeClass = typeof(Report);
        tableName = NameCompatibilityManager.GetTableName(typeClass);
        columnName = NameCompatibilityManager.GetColumnName(typeClass, "ExtId");

        if (!Schema.Table(tableName).Column(columnName).Exists())
        {
            Create.Column(columnName)
            .OnTable(tableName)
            .AsString(50)
            .Nullable();
        }
    }
}
