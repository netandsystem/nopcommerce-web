using System;

namespace Nop.Core.Domain.Statistics;

public class SellerStatistics : BaseSyncEntity
{
    /// <summary>
    /// Gets or sets the seller identifier
    /// </summary>
    public int SellerId { get; set; }

    /// <summary>
    /// Gets or sets the current month
    /// </summary>

    public int Month { get; set; }

    /// <summary>
    /// Gets or sets the total invoiced
    /// </summary>
    public decimal TotalInvoiced { get; set; }

    /// <summary>
    /// Gets or sets the total collected
    /// </summary>
    public decimal TotalCollected { get; set; }

    /// <summary>
    /// Gets or sets the Activations
    /// </summary>
    public int Activations { get; set; }
}
