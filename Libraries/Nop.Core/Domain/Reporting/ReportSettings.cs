using Nop.Core.Configuration;

//NaS Code

namespace Nop.Core.Domain.Reporting;

/// <summary>
/// Report settings
/// </summary>
public class ReportSettings : ISettings
{
    /// <summary>
    /// Gets or sets a value indicating whether account statement test mode is enabled
    /// </summary>
    public bool AccountStatementTestMode { get; set; }

    /// <summary>
    /// Gets or sets an ID of a customer to test account statement
    /// </summary>
    public int AccountStatementTestCustomerId { get; set; }
}
