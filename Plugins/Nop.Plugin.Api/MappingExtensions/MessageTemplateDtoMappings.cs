using Nop.Core.Domain.Messages;
using Nop.Plugin.Api.AutoMapper;
using Nop.Plugin.Api.DTO.Messages;
using Nop.Plugin.Api.Helpers;

namespace Nop.Plugin.Api.MappingExtensions;

public static class QueuedEmailDtoMappings
{
    public static QueuedEmailDto ToDto(this QueuedEmail item)
    {
        var itemDto = item.MapTo<QueuedEmail, QueuedEmailDto>();
        itemDto.Body = DTOHelper.DeleteSpaces(itemDto.Body);
        return itemDto;
    }
}
