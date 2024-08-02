using System.Collections.Generic;

namespace Nop.Core.Domain.Reporting;

#nullable enable

public class Report : BaseSyncEntity2
{
    public ReportType Type { get; set; }
    public string Data { get; set; } = string.Empty;
    public List<ReportData>? DataDic
    {
        set
        {
            Data = Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        get =>
           Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReportData>>(Data);
    }
    public int CustomerId { get; set; }
    public int? QueuedEmailId { get; set; }
}
