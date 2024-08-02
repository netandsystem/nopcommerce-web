using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Reporting;
using Nop.Plugin.Api.Authorization.Policies;
using Nop.Plugin.Api.DTO.Errors;
using Nop.Plugin.Api.DTO.Messages;
using Nop.Plugin.Api.DTO.Reporting;
using Nop.Plugin.Api.JSON.Serializers;
using Nop.Plugin.Api.MappingExtensions;
using Nop.Plugin.Api.Models.ReportingParameters;
using Nop.Plugin.Api.Services;
using Nop.Services.Authentication;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Nop.Plugin.Api.Controllers;

#nullable enable

[Route("api/reports")]

public class ReportController : BaseSyncController<ReportDto>
{
    #region Attributes

    private readonly IReportApiService _reportApiService;
    private readonly IWorkflowMessageService _workFlowMessageService;
    private readonly IPdfService _pdfService;


    #endregion

    #region Ctr
    public ReportController(
        IReportApiService reportApiService,
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
        IPdfService pdfService,
        IWorkflowMessageService workFlowMessageService,
        //ISettingService settingService,
        IStoreContext storeContext
    ) :
    base(
        reportApiService,
        jsonFieldsSerializer,
        aclService,
        customerService,
        storeMappingService,
        storeService,
        discountService,
        customerActivityService,
        localizationService,
        pictureService,
        authenticationService,
        storeContext
    )
    {
        _reportApiService = reportApiService;
        _pdfService = pdfService;
        _workFlowMessageService = workFlowMessageService;
        //_settingService = settingService;
    }


    #endregion

    #region Methods

    /// <summary>
    ///     Retrieve current customer
    /// </summary>
    /// <param name="fields">Fields from the customer you want your json to contain</param>
    /// <response code="200">OK</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost("test", Name = "SendReportTest")]
    [Authorize(Policy = SellerRoleAuthorizationPolicy.Name)]
    [ProducesResponseType(typeof(EmailResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> SendReportTest([FromBody] HtmtParameter body)
    {
        var sellerEntity = await _authenticationService.GetAuthenticatedCustomerAsync();

        if (sellerEntity is null)
        {
            return Error(HttpStatusCode.Unauthorized);
        }

        var fileName = $"test_{CommonHelper.GenerateRandomDigitCode(4)}.pdf";

        var example_html = body.Html ?? @"
                <p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>
            "
        ;

        var file = _pdfService.Html2Pdf(example_html, fileName);

        var result = new EmailResponse()
        {
            EmailId = await _workFlowMessageService.SendReportEmailTestAsync(example_html, file, fileName)
        };

        return Ok(result);
    }


    /// <summary>
    ///     Retrieve current customer
    /// </summary>
    /// <param name="fields">Fields from the customer you want your json to contain</param>
    /// <response code="200">OK</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost(Name = "SendReport")]
    [Authorize(Policy = SellerRoleAuthorizationPolicy.Name)]
    [ProducesResponseType(typeof(EmailResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> SendReport(ReportingParametersModel body)
    {
        //Authorize
        var sellerEntity = await _authenticationService.GetAuthenticatedCustomerAsync();

        if (sellerEntity is null)
        {
            return Error(HttpStatusCode.Unauthorized);
        }

        /*
            1.- Load form from settings
            2.- validate body
            3.- load tokens, generate pdf and add email to queue
            4.- save report
            5.- return email id and report id

         */

        var report = new Report()
        {
            Type = body.ReportType,
            DataDic = body.Data,
            CustomerId = sellerEntity.Id,
            UpdatedOnUtc = DateTime.UtcNow,
            QueuedEmailId = null,
            ExtId = body.ExtId,
        };

        //Save report
        await _reportApiService.InsertReport(new List<Report>() { report });

        //Load tokens, Generate pdf and add email to queue
        int? emailId;

        try
        {
            emailId = await _workFlowMessageService.SendReportEmailAsync(report, body.Cc, null, true, !body.UseDefaultTemplate);
        }
        catch (Exception ex)
        {
            await _reportApiService.DeleteReport(report);
            return Error(HttpStatusCode.InternalServerError, "General", ex.Message);
        }

        if (emailId is null || emailId == 0)
        {
            return Ok(new EmailResponse()
            {
                EmailId = 0,
                ReportId = 0,
                Error = "Plantilla no encontrada"
            });
        }

        report.QueuedEmailId = emailId;
        await _reportApiService.UpdateReport(new List<Report>() { report });


        //Return email id and report id
        var result = new EmailResponse()
        {
            EmailId = (int)emailId,
            ReportId = report.Id,
            Error = null
        };

        return Ok(result);
    }

    public class GetQueuedEmailFromReportParams
    {
        public GetQueuedEmailFromReportParams(ReportDto report, bool useDefaultTemplate)
        {
            Report = report;
            UseDefaultTemplate = useDefaultTemplate;
        }

        public ReportDto Report { get; set; }
        public bool UseDefaultTemplate { get; set; }
    }

    /// <summary>
    ///     Retrieve current customer
    /// </summary>
    /// <param name="fields">Fields from the customer you want your json to contain</param>
    /// <response code="200">OK</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost("queued-email", Name = "GetQueuedEmailFromReport")]
    [Authorize(Policy = SellerRoleAuthorizationPolicy.Name)]
    [ProducesResponseType(typeof(QueuedEmailDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetQueuedEmailFromReport(GetQueuedEmailFromReportParams body)
    {
        //Authorize
        var sellerEntity = await _authenticationService.GetAuthenticatedCustomerAsync();

        if (sellerEntity is null)
        {
            return Error(HttpStatusCode.Unauthorized);
        }

        var report = new Report()
        {
            Id = body.Report.Id,
            Type = body.Report.Type,
            DataDic = body.Report.DataDic,
            CustomerId = sellerEntity.Id,
            UpdatedOnUtc = body.Report.UpdatedOnUtc,
            QueuedEmailId = body.Report.QueuedEmailId,
            ExtId = body.Report.ExtId
        };

        var queuedEmail = await _workFlowMessageService.GetQueuedEmailFromReportAsync(report, !body.UseDefaultTemplate);

        return Ok(queuedEmail.ToDto());
    }

    #endregion

    #region private Methods


    #endregion

    #region private classes

    public class EmailResponse
    {
        public string? Error { get; set; }
        public int EmailId { get; set; }
        public int ReportId { get; set; }
    }

    public class HtmtParameter
    {
        public HtmtParameter(string? html)
        {
            Html = html;
        }

        public string? Html { get; set; }
    }

    #endregion
}
