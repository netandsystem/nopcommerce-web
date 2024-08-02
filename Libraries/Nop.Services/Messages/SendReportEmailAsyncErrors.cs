//NaS Code
#nullable enable

namespace Nop.Services.Messages;

public enum ErrorType
{
    Low,
    Hight
}


public class SendReportEmailAsyncErrors
{
    public string Error { get; set; } = string.Empty;
    public ErrorType ErrorType { get; set; }
}

