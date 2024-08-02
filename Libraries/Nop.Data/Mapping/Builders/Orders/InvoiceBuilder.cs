using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Statistics;
using Nop.Data.Extensions;
using System;
using System.Data;

namespace Nop.Data.Mapping.Builders.Statistics;

/// <summary>
/// Represents a product entity builder
/// </summary>
public partial class InvoiceBuilder : NopEntityBuilder<Invoice>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        BaseSyncEntityBuilder.MapEntity(table, true)
               .WithColumn(nameof(Invoice.DocumentType)).AsInt32().NotNullable()
               .WithColumn(nameof(Invoice.CreatedOnUtc)).AsDateTime().NotNullable().WithDefault(FluentMigrator.SystemMethods.CurrentUTCDateTime)
               .WithColumn(nameof(Invoice.Total)).AsDecimal().NotNullable()
               .WithColumn(nameof(Invoice.CustomerId)).AsInt32().NotNullable().ForeignKey<Customer>(onDelete: Rule.None)
               .WithColumn(nameof(Invoice.SellerId)).AsInt32().Nullable().ForeignKey<Customer>(onDelete: Rule.None)
               .WithColumn(nameof(Invoice.Balance)).AsDecimal().NotNullable().WithDefaultValue(0)
               .WithColumn(nameof(Invoice.TaxPrinterNumber)).AsString(400).Nullable();
    }

    #endregion
}