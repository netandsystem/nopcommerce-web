using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nop.Plugin.Api.Models.Base;

#nullable enable

public class Sync4ParametersModel
{
    public Sync4ParametersModel(List<int> idsInDb, long? lastUpdateTs, bool useIdsInDb = true, int compressionVersion = 0)
    {
        IdsInDb = idsInDb;
        LastUpdateTs = lastUpdateTs;
        UseIdsInDb = useIdsInDb;
        CompressionVersion = compressionVersion;
    }

    [JsonProperty("ids_in_db", Required = Required.AllowNull)]
    public List<int>? IdsInDb { get; set; }

    [JsonProperty("last_update_ts", Required = Required.AllowNull)]
    public long? LastUpdateTs { get; set; }

    [JsonProperty("use_ids_in_db", Required = Required.Default)]
    public bool UseIdsInDb { get; set; }

    [JsonProperty("compression_version", Required = Required.Default)]
    public int CompressionVersion { get; set; }
}
