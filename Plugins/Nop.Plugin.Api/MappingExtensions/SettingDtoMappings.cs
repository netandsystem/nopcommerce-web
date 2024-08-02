using Nop.Core.Domain.Configuration;
using Nop.Plugin.Api.AutoMapper;
using Nop.Plugin.Api.DTOs.Configuration;

namespace Nop.Plugin.Api.MappingExtensions;

public static class SettingDtoMappings
{
    public static SettingDto ToDto(this Setting item)
    {
        return item.MapTo<Setting, SettingDto>();
    }

    public static Setting ToEntity(this SettingDto item)
    {
        var newItem = item.MapTo<SettingDto, Setting>();

        newItem.Id = 0;
        newItem.StoreId = 0;
        //newItem.UpdatedOnUtc = DateTime.UtcNow;

        return newItem;
    }
}
