using Newtonsoft.Json;
using Nop.Plugin.Api.DTO.Base;

namespace Nop.Plugin.Api.DTOs.Configuration;

#nullable enable

public class SettingDto : BaseSyncDto
{
    public SettingDto(string name, string value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// Gets or sets the name
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value
    /// </summary>
    [JsonProperty("value", Required = Required.Always)]
    public string Value { get; set; }
}
