using Newtonsoft.Json;
using Nop.Core.Domain.Reporting;
using Nop.Plugin.Api.DTO.Base;
using System.Collections.Generic;

namespace Nop.Plugin.Api.DTO.Reporting;

#nullable enable

public class ReportDto : BaseSync2Dto
{
    [JsonProperty("type", Required = Required.Always)]
    public ReportType Type { get; set; }

    [JsonProperty("data", Required = Required.Always)]
    public List<ReportData> DataDic { get; set; } = new();

    [JsonProperty("customer_id", Required = Required.Always)]
    public int CustomerId { get; set; }

    [JsonProperty("queued_email_id", Required = Required.AllowNull)]
    public int? QueuedEmailId { get; set; }
}
