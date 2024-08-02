using FluentMigrator;
using Nop.Core.Domain.ScheduleTasks;
using Nop.Data.Mapping;
using System.Linq;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2024/07/24 15:23:00:2551771", "ScheduleTask", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class ScheduleTask20240724 : Migration
{
    private readonly INopDataProvider _dataProvider;

    public ScheduleTask20240724(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        var typeClass = typeof(ScheduleTask);
        var tableName = NameCompatibilityManager.GetTableName(typeClass);
        var columnName = NameCompatibilityManager.GetColumnName(typeClass, nameof(ScheduleTask.InitialDateUtc));

        var task = new ScheduleTask
        {
            // enviar estados de cuenta
            Name = "Send account statements",
            Seconds = 7 * 24 * 3600, // 7 days
            Type = "Nop.Services.Messages.AccountStatementsSendTask, Nop.Services",
            Enabled = false,
            StopOnError = false,
            InitialDateUtc = null
        };

        if (!_dataProvider.GetTable<ScheduleTask>().Any(pr => string.Compare(pr.Name, task.Name, true) == 0))
        {
            _dataProvider.InsertEntity(task);
        }
    }

    public override void Down()
    {
        //add the downgrade logic if necessary 
    }
}
