using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Runner.Generators.Base;
using MySqlX.XDevAPI;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Statistics;
using Nop.Data.Extensions;
using System.Data;

namespace Nop.Data.Mapping.Builders.Statistics;

/// <summary>
/// Represents a product entity builder
/// </summary>
public partial class BaseSyncEntityBuilder
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public static CreateTableExpressionBuilder MapEntity(CreateTableExpressionBuilder table, bool isBaseSyncEntity2 = false)
    {
        table
           .WithColumn(nameof(BaseSyncEntity.UpdatedOnUtc)).AsDateTime2().NotNullable().WithDefault(FluentMigrator.SystemMethods.CurrentUTCDateTime);

        if (isBaseSyncEntity2)
        {
            table
                .WithColumn(nameof(BaseSyncEntity2.ExtId))
                .AsString(4000)
                .Nullable();
        }

        return table;
    }

    #endregion
}