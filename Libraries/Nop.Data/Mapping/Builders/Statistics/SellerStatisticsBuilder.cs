using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Statistics;
using Nop.Data.Extensions;
using System.Data;

namespace Nop.Data.Mapping.Builders.Statistics;

/// <summary>
/// Represents a product entity builder
/// </summary>
public partial class SellerStatisticsBuilder : NopEntityBuilder<SellerStatistics>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
           .WithColumn(nameof(SellerStatistics.SellerId)).AsInt32().ForeignKey<Customer>(onDelete: Rule.Cascade)
           .WithColumn(nameof(SellerStatistics.Month)).AsInt32().NotNullable()
           .WithColumn(nameof(SellerStatistics.TotalInvoiced)).AsDecimal().NotNullable().WithDefaultValue(0)
           .WithColumn(nameof(SellerStatistics.TotalCollected)).AsDecimal().NotNullable().WithDefaultValue(0)
           .WithColumn(nameof(SellerStatistics.Activations)).AsInt32().NotNullable().WithDefaultValue(0)
           .WithColumn(nameof(SellerStatistics.UpdatedOnUtc)).AsDateTime2().NotNullable().WithDefault(FluentMigrator.SystemMethods.CurrentUTCDateTime);
    }

    #endregion
}