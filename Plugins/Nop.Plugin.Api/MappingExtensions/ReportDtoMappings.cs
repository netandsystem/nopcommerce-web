using Nop.Core.Domain.Reporting;
using Nop.Plugin.Api.AutoMapper;
using Nop.Plugin.Api.DTO.Reporting;

namespace Nop.Plugin.Api.MappingExtensions;

public static class ReportDtoMappings
{
    public static ReportDto ToDto(this Report item)
    {
        return item.MapTo<Report, ReportDto>();
    }

    //public static Report ToEntity(this ReportDto item)
    //{
    //    return item.MapTo<ReportDto, Report>();
    //}
}
