﻿using Nop.Plugin.Api.DTO.Base;
using Nop.Plugin.Api.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Api.Services;

#nullable enable

public interface IBaseSyncService<TDtoEntity> where TDtoEntity : BaseSyncDto
{
    List<List<object?>> GetItemsCompressed3(IList<TDtoEntity> items);

    Task<BaseSyncResponse> GetLastestUpdatedItems3Async(IList<int>? idsInDb, long? lastUpdateTs, int sellerId, int storeId);

    Task<BaseSyncResponse> GetLastestUpdatedItems4Async(bool useIdsInDb, IList<int>? idsInDb, long? lastUpdateTs, int sellerId, int storeId, int compressionVersion);
}