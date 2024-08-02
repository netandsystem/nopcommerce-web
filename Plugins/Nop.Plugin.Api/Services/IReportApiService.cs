using Nop.Core.Domain.Reporting;
using Nop.Plugin.Api.DTO.Reporting;
using Nop.Plugin.Api.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Api.Services;

#nullable enable

public interface IReportApiService : IBaseSyncService<ReportDto>
{
    Task DeleteReport(Report report);
    Task InsertReport(IList<Report> reports);
    Task UpdateReport(IList<Report> reports);
}
