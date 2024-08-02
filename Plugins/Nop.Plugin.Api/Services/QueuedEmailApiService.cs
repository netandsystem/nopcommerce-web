using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Reporting;
using Nop.Data;
using Nop.Plugin.Api.DTO.Messages;
using Nop.Plugin.Api.DTOs.Base;
using Nop.Plugin.Api.MappingExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Api.Services;

#nullable enable
public class QueuedEmailApiService : BaseSyncService<QueuedEmailDto>, IQueuedEmailApiService
{
    #region Fields

    private readonly IRepository<QueuedEmail> _queuedEmailRepository;
    private readonly IRepository<Report> _reportRepository;

    #endregion

    #region Ctr

    public QueuedEmailApiService(IRepository<QueuedEmail> queuedEmailRepository, IRepository<Report> reportRepository)
    {
        _queuedEmailRepository = queuedEmailRepository;
        _reportRepository = reportRepository;
    }

    #endregion

    #region Methods

    public override async Task<BaseSyncResponse> GetLastestUpdatedItems3Async(
       IList<int>? idsInDb, long? lastUpdateTs, int sellerId, int storeId
    )
    {
        async Task<List<QueuedEmailDto>> GetSellerItemsAsync()
        {
            var selectedItemsQuery = from q in _queuedEmailRepository.Table
                                     join r in _reportRepository.Table
                                     on q.Id equals r.QueuedEmailId
                                     where r.CustomerId == sellerId
                                     && r.QueuedEmailId != null
                                     select q.ToDto();

            return await selectedItemsQuery.ToListAsync();
        }


        return await GetLastestUpdatedItems3Async(
            idsInDb,
            null,
            GetSellerItemsAsync
         );
    }

    public override async Task<BaseSyncResponse> GetLastestUpdatedItems4Async(
      bool useIdsInDb, IList<int>? idsInDb, long? lastUpdateTs, int sellerId, int storeId, int compressionVersion = 0
    )
    {
        async Task<List<QueuedEmailDto>> GetSellerItemsAsync()
        {
            var selectedItemsQuery = from q in _queuedEmailRepository.Table
                                     join r in _reportRepository.Table
                                     on q.Id equals r.QueuedEmailId
                                     where r.CustomerId == sellerId
                                     && r.QueuedEmailId != null
                                     select q.ToDto();

            return await selectedItemsQuery.ToListAsync();
        }


        return await InnerGetLastestUpdatedItems4Async(
            useIdsInDb,
            idsInDb,
            lastUpdateTs,
            GetSellerItemsAsync,
            compressionVersion,
            new() { GetItemsCompressed3 }
         );
    }

    public override List<List<object?>> GetItemsCompressed3(IList<QueuedEmailDto> items)
    {
        /*
          [
            id, number
            deleted,  boolean
            updated_on_ts,  number

            created_on_ts,  number
            subject, string
            body, string
            attachment_file_path, string
            attachment_file_name, string
            sent_tries, number
            sent_on_ts, number
          ]
        */

        return items.Select(p =>
            new List<object?>() {
                p.Id,
                p.Deleted,
                p.UpdatedOnTs,
                p.CreatedOnTs,
                p.Subject,
                p.Body,
                p.AttachmentFilePath,
                p.AttachmentFileName,
                p.SentTries,
                p.SentOnTs
            }
        ).ToList();
    }

    #endregion

    #region Private Methods
    #endregion
}
