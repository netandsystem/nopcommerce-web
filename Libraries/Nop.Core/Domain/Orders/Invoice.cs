using System;

namespace Nop.Core.Domain.Orders;

//NaS Code

#nullable enable

public enum InvoiceType
{
    Invoice = 1,
    DeliveryNote,
}

public class Invoice : BaseSyncEntity2
{

    public InvoiceType DocumentType { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public decimal Total { get; set; }
    public int CustomerId { get; set; }
    public int? SellerId { get; set; }
    public decimal Balance { get; set; }
    public string? TaxPrinterNumber { get; set; }

    // ========================================

    public decimal TaxAmount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal TaxExemptAmount { get; set; }
    public decimal DiscountNumber { get; set; }
    public string? DiscountCode { get; set; }

    /// <summary>
    /// Fecha de cobro
    /// </summary>
    public DateTime? DueDateUtc { get; set; }

    /// <summary>
    /// Base imponible
    /// </summary>
    public decimal TaxBase { get; set; }

    /// <summary>
    /// Tasa de cambio
    /// </summary>
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// Días de negociación
    /// </summary>
    public int? GetDaysNegotiated() { return (DueDateUtc - CreatedOnUtc)?.Days; }

    /// <summary>
    /// Días de vencido
    /// </summary>
    public int? GetDaysPastDue() { return (DateTime.UtcNow - DueDateUtc)?.Days; }
}
