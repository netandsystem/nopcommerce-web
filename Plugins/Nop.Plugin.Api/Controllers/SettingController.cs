using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Configuration;
using Nop.Plugin.Api.Authorization.Policies;
using Nop.Plugin.Api.DTO.Errors;
using Nop.Plugin.Api.DTOs.Configuration;
using Nop.Plugin.Api.JSON.Serializers;
using Nop.Plugin.Api.Models;
using Nop.Plugin.Api.Services;
using Nop.Services.Authentication;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace Nop.Plugin.Api.Controllers;

#nullable enable

[Route("api/settings")]

public class SettingController : BaseSyncController<SettingDto>
{
    #region Fields

    private readonly ISettingApiService _settingApiService;

    #endregion

    #region Ctr

    public SettingController(
        ISettingApiService settingApiService,
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
        base(settingApiService, jsonFieldsSerializer, aclService, customerService, storeMappingService, storeService, discountService, customerActivityService,
             localizationService, pictureService, authenticationService, storeContext)
    {
        _settingApiService = settingApiService;
    }

    #endregion

    #region Methods

    [HttpPost(Name = "Add Many Settings")]
    [Authorize(Policy = AdministratorsRoleAuthorizationPolicy.Name)]
    [ProducesResponseType(typeof(DbResult<Setting>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddManySettings([FromBody] List<SettingPost> settings)
    {
        //Authorize
        var sellerEntity = await _authenticationService.GetAuthenticatedCustomerAsync();

        if (sellerEntity is null)
        {
            return Error(HttpStatusCode.Unauthorized);
        }

        if (settings == null)
            return Error(HttpStatusCode.BadRequest, "settings is null");

        if (settings.Count == 0)
            return Error(HttpStatusCode.BadRequest, "settings is empty");

        var settingsList = settings.Select(x => new Setting()
        {
            Id = 0,
            Name = x.Name,
            Value = x.Value,
            StoreId = 0
        }).ToList();

        var result = await _settingApiService.InsertOrUpdateSettingsAsync(settingsList);

        return Ok(result);
    }

    public class SettingPost
    {
        public SettingPost(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [JsonProperty("value", Required = Required.Always)]
        public string Value { get; set; }
    }

    #endregion

    #region Private methods


    #endregion
}
