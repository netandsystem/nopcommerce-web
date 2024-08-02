using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Reporting;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders.Statistics;
using System.Data;

namespace Nop.Data.Mapping.Builders.Reporting;

public partial class ReportBuilder : NopEntityBuilder<Report>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        BaseSyncEntityBuilder.MapEntity(table)
           .WithColumn(nameof(Report.Type)).AsInt32().NotNullable()
           //MAX string
           .WithColumn(nameof(Report.Data)).AsString(int.MaxValue).NotNullable()
           .WithColumn(nameof(Report.CustomerId)).AsInt32().ForeignKey<Customer>(onDelete: Rule.Cascade)
           .WithColumn(nameof(Report.QueuedEmailId)).AsInt32().ForeignKey<QueuedEmail>(onDelete: Rule.SetNull).Nullable();
    }

    #endregion
}
