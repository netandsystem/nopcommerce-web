using Nop.Core.Domain.Reporting;

//NaS Code
#nullable enable

namespace Nop.Services.Messages;

public class SendReportEmailAsyncParams
{
    public SendReportEmailAsyncParams(
        Report report,
        string? cc,
        string? bcc,
        bool sendPdf,
        bool useTemplate
    )
    {
        Report = report;
        Cc = cc;
        Bcc = bcc;
        SendPdf = sendPdf;
        UseTemplate = useTemplate;
    }

    public Report Report { get; set; }
    public string? Cc { get; set; }

    public string? Bcc { get; set; }
    public bool SendPdf { get; set; }
    public bool UseTemplate { get; set; }

}
