using Newtonsoft.Json;

namespace Nop.Plugin.Api.DTO.Base;

public abstract class BaseSync2Dto : BaseSyncDto
{
#nullable enable
    [JsonProperty("ext_id", Required = Required.AllowNull)]
    public string? ExtId { get; set; }
}
