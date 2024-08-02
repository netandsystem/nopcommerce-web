using Nop.Core.Domain.Configuration;
using Nop.Data;
using Nop.Plugin.Api.DTOs.Base;
using Nop.Plugin.Api.DTOs.Configuration;
using Nop.Plugin.Api.DTOs.Statistics;
using Nop.Plugin.Api.MappingExtensions;
using Nop.Plugin.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Api.Services;

#nullable enable
public class SettingApiService : BaseSyncService<SettingDto>, ISettingApiService
{
    #region Fields

    private readonly IRepository<Setting> _settingRepository;

    #endregion

    #region Ctr

    public SettingApiService(IRepository<Setting> settingRepository)
    {
        _settingRepository = settingRepository;
    }

    #endregion

    #region Methods

    public override async Task<BaseSyncResponse> GetLastestUpdatedItems3Async(
       IList<int>? idsInDb, long? lastUpdateTs, int sellerId, int storeId
    )
    {
        async Task<List<SettingDto>> GetSellerItemsAsync()
        {
            var selectedItemsQuery = from s in _settingRepository.Table
                                     where s.Name.Contains("Plugins.Api.Mobile.")
                                     select s.ToDto();

            return await selectedItemsQuery.ToListAsync();
        }


        return await GetLastestUpdatedItems3Async(
            idsInDb,
            null,
            () => GetSellerItemsAsync()
         );
    }

    public override List<List<object?>> GetItemsCompressed3(IList<SettingDto> items)
    {
        /**
          [
            id, number
            deleted,  boolean
            updated_on_ts,  number
     
            seller_id, number
            month, number
            total_invoiced, number
            total_collected, number
            activations, number
          ]
          */

        return items.Select(p =>
            new List<object?>() {
                p.Id,
                p.Deleted,
                p.UpdatedOnTs,
                p.Name,
                p.Value,
            }
        ).ToList();
    }

    public async Task<List<Setting>> AddSetting(List<Setting> settings)
    {
        foreach (var setting in settings)
        {
            await _settingRepository.InsertAsync(setting);
        }

        return settings;
    }

    public async Task<List<Setting>> UpdateSetting(List<Setting> settings)
    {
        foreach (var setting in settings)
        {
            await _settingRepository.UpdateAsync(setting);
        }

        return settings;
    }

    public async Task<List<DbResult<Setting>>> InsertOrUpdateSettingsAsync(IList<Setting> settings)
    {
        // delete duplicated settings
        var settingDic = new Dictionary<string, Setting>();

        foreach (var setting in settings)
        {
            settingDic[setting.Name] = setting;
        }

        var _settings = settingDic.Values.ToList();

        var query = from s in _settingRepository.Table
                    where _settings.Select(x => x.Name).Contains(s.Name)
                    select s;

        var existingSettings = await query.ToListAsync();

        var result = new DbResult<Setting>();

        foreach (var setting in _settings)
        {
            var existingSetting = existingSettings.FirstOrDefault(x => x.Name == setting.Name);

            if (existingSetting != null)
            {
                existingSetting.Value = setting.Value;
                await _settingRepository.UpdateAsync(existingSetting);
                result.Updated.Add(existingSetting);
            }
            else
            {
                await _settingRepository.InsertAsync(setting);
                result.Inserted.Add(setting);
            }
        }

        return new List<DbResult<Setting>> { result };
    }

    public override async Task<BaseSyncResponse> GetLastestUpdatedItems4Async(
      bool useIdsInDb, IList<int>? idsInDb, long? lastUpdateTs, int sellerId, int storeId, int compressionVersion = 0
     )
    {
        async Task<List<SettingDto>> GetSellerItemsAsync()
        {
            var selectedItemsQuery = from s in _settingRepository.Table
                                     where s.Name.Contains("Plugins.Api.Mobile.")
                                     select s.ToDto();

            return await selectedItemsQuery.ToListAsync();
        }

        return await InnerGetLastestUpdatedItems4Async(
            useIdsInDb,
            idsInDb,
            null,
            GetSellerItemsAsync,
            compressionVersion,
            new() { GetItemsCompressed3 }
         );
    }

    #endregion

    #region Private Methods
    #endregion
}
