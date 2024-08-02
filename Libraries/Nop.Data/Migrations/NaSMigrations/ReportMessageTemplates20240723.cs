using FluentMigrator;
using Nop.Core.Domain.Configuration;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Reporting;
using System.Collections.Generic;
using System.Linq;

// #region NaS Code

namespace Nop.Data.Migrations.NaSMigrations;

[NopMigration("2024/07/23 23:14:00:2551771", "Add Message Templates", UpdateMigrationType.Data, MigrationProcessType.Update)]
public class ReportMessageTemplates20240723 : Migration
{
    private readonly INopDataProvider _dataProvider;

    public ReportMessageTemplates20240723(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {

        CreateMessageTemplateAsync("Report.Default");

        foreach (var role in System.Enum.GetValues(typeof(ReportType)))
        {
            CreateMessageTemplateAsync("Report." + role.ToString());
        }

        var setting = new List<Setting>() {
            new()
            {
                //Name = "reportsettings.accountstatementtestmode",
                Name = nameof(ReportSettings).ToLower()
                    + "."
                    + nameof(ReportSettings.AccountStatementTestMode).ToLower(),
                StoreId = 0,
                Value = true.ToString()
            },
            new()
            {
                //Name = "reportsettings.accountstatementtestcustomerid",
                Name = nameof(ReportSettings).ToLower()
                    + "."
                    + nameof(ReportSettings.AccountStatementTestCustomerId).ToLower(),
                StoreId = 0,
                Value = "0"
            }
        };

        foreach (var item in setting)
        {
            if (!_dataProvider.GetTable<Setting>().Any(pr => string.Compare(pr.Name, item.Name, true) == 0))
            {
                _dataProvider.InsertEntity(item);
            }
        }
    }

    private void CreateMessageTemplateAsync(string name)
    {
        var item = new MessageTemplate()
        {
            Name = name,
            Subject = string.Empty,
            EmailAccountId = 1,
            Body = "<p>Test</p>",
            BccEmailAddresses = null,
            IsActive = false,
            DelayBeforeSend = null,
            DelayPeriodId = 0,
            AttachedDownloadId = 0,
            LimitedToStores = false
        };

        if (!_dataProvider.GetTable<MessageTemplate>().Any(pr => string.Compare(pr.Name, item.Name, true) == 0))
        {
            _dataProvider.InsertEntity(item);
        }
    }

    public override void Down()
    {
        //add the downgrade logic if necessary 
    }
}
