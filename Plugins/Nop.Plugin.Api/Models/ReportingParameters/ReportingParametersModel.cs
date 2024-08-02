using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Core.Domain.Reporting;
using Nop.Plugin.Api.Infrastructure;
using Nop.Plugin.Api.ModelBinders;

namespace Nop.Plugin.Api.Models.ReportingParameters;

#nullable enable

public class ReportingParametersModel
{
    public ReportingParametersModel(ReportType reportType, List<ReportData> data, string? cc, string? bcc, bool useDefaultTemplate, string? extId)
    {
        ReportType = reportType;
        Data = data;
        Cc = cc;
        Bcc = bcc;
        UseDefaultTemplate = useDefaultTemplate;
        ExtId = extId;
    }

    [JsonProperty(Required = Required.Always)]
    public ReportType ReportType { get; set; }

    [JsonProperty(Required = Required.Always)]
    public List<ReportData> Data { get; set; }

    [JsonProperty(Required = Required.Default)]
    public string? Cc { get; set; }

    [JsonProperty(Required = Required.Default)]
    public string? Bcc { get; set; }

    [JsonProperty(Required = Required.Default)]
    public bool UseDefaultTemplate;

    [JsonProperty(Required = Required.Default)]
    public string? ExtId;
}