using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Api.DTO.Messages;
using Nop.Plugin.Api.JSON.Serializers;
using Nop.Plugin.Api.Services;
using Nop.Services.Authentication;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Stores;


namespace Nop.Plugin.Api.Controllers;

#nullable enable

[Route("api/queued-emails")]

public class QueuedEmailController : BaseSyncController<QueuedEmailDto>
{
    #region Fields

    private readonly IQueuedEmailApiService _queuedEmailApiService;

    #endregion

    #region Ctr

    public QueuedEmailController(
        IQueuedEmailApiService queuedEmailApiService,
        IJsonFieldsSerializer jsonFieldsSerializer,
        IAclService aclService,
        ICustomerService customerService,
        IStoreMappingService storeMappingService,
        IStoreService storeService,
        IDiscountService discountService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        IPictureService pictureService,
        IAuthenticationService authenticationService,
        IStoreContext storeContext
    ) :
        base(queuedEmailApiService, jsonFieldsSerializer, aclService, customerService, storeMappingService, storeService, discountService, customerActivityService,
             localizationService, pictureService, authenticationService, storeContext)
    {
        _queuedEmailApiService = queuedEmailApiService;
    }

    #endregion

    #region Methods

    #endregion

    #region Private methods


    #endregion
}
