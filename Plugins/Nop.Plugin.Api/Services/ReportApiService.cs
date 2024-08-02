using Nop.Core.Domain.Reporting;
using Nop.Data;
using Nop.Plugin.Api.DTO.Reporting;
using Nop.Plugin.Api.DTOs.Base;
using Nop.Plugin.Api.MappingExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Api.Services;

#nullable enable
public class ReportApiService : BaseSyncService<ReportDto>, IReportApiService
{
    #region Fields

    private readonly IRepository<Report> _reportRepository;

    #endregion


    #region Ctr

    public ReportApiService(IRepository<Report> reportRepository)
    {
        _reportRepository = reportRepository;
    }

    #endregion

    #region Methods


    public override async Task<BaseSyncResponse> GetLastestUpdatedItems3Async(
       IList<int>? idsInDb, long? lastUpdateTs, int sellerId, int storeId
    )
    {
        async Task<List<ReportDto>> GetSellerItemsAsync()
        {
            var selectedItemsQuery = from s in _reportRepository.Table
                                     where s.CustomerId == sellerId
                                     select s.ToDto();

            return await selectedItemsQuery.ToListAsync();
        }


        return await GetLastestUpdatedItems3Async(
            idsInDb,
            lastUpdateTs,
            () => GetSellerItemsAsync()
         );
    }

    public override List<List<object?>> GetItemsCompressed3(IList<ReportDto> items)
    {
        /**
          [
            id, number
            deleted,  boolean
            updated_on_ts,  number
     
            type, string
            data_dic, json
            customer_id, number
          ]
          */

        return items.Select(p =>
            new List<object?>() {
                p.Id,
                p.Deleted,
                p.UpdatedOnTs,
                p.Type,
                p.DataDic,
            }
        ).ToList();
    }

    public async Task InsertReport(IList<Report> reports)
    {
        await _reportRepository.InsertAsync(reports);
    }

    public async Task UpdateReport(IList<Report> reports)
    {
        await _reportRepository.UpdateAsync(reports);
    }

    public async Task DeleteReport(Report report)
    {
        await _reportRepository.DeleteAsync(report);
    }

    public override async Task<BaseSyncResponse> GetLastestUpdatedItems4Async(
     bool useIdsInDb, IList<int>? idsInDb, long? lastUpdateTs, int sellerId, int storeId, int compressionVersion = 0
    )
    {
        async Task<List<ReportDto>> GetSellerItemsAsync()
        {
            var selectedItemsQuery = from s in _reportRepository.Table
                                     where s.CustomerId == sellerId
                                     select s.ToDto();

            return await selectedItemsQuery.ToListAsync();
        }

        return await InnerGetLastestUpdatedItems4Async(
            useIdsInDb,
            idsInDb,
            lastUpdateTs,
            GetSellerItemsAsync,
            compressionVersion,
            new() { GetItemsCompressed4 }
         );
    }

    public List<List<object?>> GetItemsCompressed4(IList<ReportDto> items)
    {
        /**
          [
            id, number
            deleted,  boolean
            updated_on_ts,  number
     
            type, string
            data_dic, json
            queued_email_id, number
            ext_id, string
          ]
          */

        return items.Select(p =>
            new List<object?>() {
                p.Id,
                p.Deleted,
                p.UpdatedOnTs,

                p.Type,
                p.DataDic,
                p.QueuedEmailId,
                p.ExtId,
            }
        ).ToList();
    }

    #endregion

    #region Private Methods
    #endregion
}
