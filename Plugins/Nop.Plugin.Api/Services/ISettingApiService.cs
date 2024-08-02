using Nop.Core.Domain.Configuration;
using Nop.Plugin.Api.DTOs.Configuration;
using Nop.Plugin.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Api.Services;

#nullable enable

public interface ISettingApiService : IBaseSyncService<SettingDto>
{
    Task<List<DbResult<Setting>>> InsertOrUpdateSettingsAsync(IList<Setting> settings);
}