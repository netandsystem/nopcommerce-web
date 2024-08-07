using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Reporting;
using Nop.Services.Configuration;
using Nop.Services.Orders;
using Nop.Services.ScheduleTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Nop.Services.Logging;
using DocumentFormat.OpenXml.Drawing.Charts;
using Nop.Core;
using StackExchange.Profiling.Internal;


// NaS Code
#nullable enable

namespace Nop.Services.Messages;

/// <summary>
/// Represents a task for sending queued message 
/// </summary>
public partial class AccountStatementsSendTask : IScheduleTask
{
    #region Fields

    private readonly IInvoiceService _invoiceService;
    private readonly IWorkflowMessageService _workflowMessageService;
    private readonly ISettingService _settingService;
    private readonly ILogger _logger;

    #endregion

    #region Ctor

    public AccountStatementsSendTask(
        IInvoiceService invoiceService,
        IWorkflowMessageService workflowMessageService,
        ISettingService settingService,
        ILogger logger
    )
    {
        _invoiceService = invoiceService;
        _workflowMessageService = workflowMessageService;
        _settingService = settingService;
        _logger = logger;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Executes a task
    /// </summary>
    public virtual async Task ExecuteAsync()
    {
        /*
         
        1.- Get all the invoices with balance > 0 with Customer data
        2.- Group by Customer
        3.- For each Customer
            1.- Generate Report
            2.- Send Email
         
         */
        var setting = await _settingService.LoadSettingAsync<ReportSettings>();

        var customers = await _invoiceService.GetOverdueInvoicesWithCustomersAsync();

        // Enviar solo a un cliente en modo test
        if (setting.AccountStatementTestMode && setting.AccountStatementTestCustomerId > 0)
        {
            customers = customers.Where(x => x.Customer.Id == setting.AccountStatementTestCustomerId).ToList();
        }

        var reports = GenerateReports(customers);

        var data = reports.Select(x => new SendReportEmailAsyncParams(
            report: x.Report,
            cc: setting.AccountStatementTestMode ? null : x.Customer.Email,
            bcc: null,
            sendPdf: true,
            useTemplate: true
        )).ToList();

        var result = await _workflowMessageService.SendReportEmailAsync(data);


        if (result.Item2.Any())
        {
            throw new NopException("Error generating Reports: " + JsonSerializer.Serialize(result.Item2));
        }
        else
        {
            await _logger.InsertLogAsync(Core.Domain.Logging.LogLevel.Information, $"AccountStatementsSendTask: {result.Item1.Count} messages were queued successfully, ids: {string.Join(",", result.Item1)}");
        }
    }

    #endregion

    #region Private methods

    private string FormatDate(DateTime? date)
    {
        // gtm -4 "dd/MM/yyyy"
        return date?.AddHours(-4).ToString("dd/MM/yyyy") ?? "";
    }

    private string FormatDecimal(decimal value)
    {
        return value.ToString("N2");
    }

    private List<ReportWithCustomer> GenerateReports(IList<CustomerWithInvoiceList> customers)
    {
        var reportList = new List<ReportWithCustomer>();


        const string tableStyles = @"
                                border-bottom: 1px solid #000;
                                border-top: 1px solid #000;
                                border-collapse: collapse;
                                width: 100%;
                                margin-bottom: 10px;";
        const string headerRowStyles = @"
                                border-bottom: 1px solid #000;
                                border-collapse: collapse;
                                text-align: left;";
        const string headerStyles = @"
                                border-bottom: 1px solid #000;
                                padding: 0.4em; 
                                font-weight: bold;
                                text-align: left;";
        const string rowStyles = "text-align: left;";
        const string cellStyles = "padding: 0.4em;";


        foreach (var customer in customers)
        {
            var report = new Report
            {
                CustomerId = customer.Customer.Id,
                ExtId = null,
                Type = ReportType.AccountStatement,
                DataDic = new()
                {
                    new ReportData
                    {
                        Key = "SellerExtId",
                        Label = "SellerExtId",
                        FeatureValue = customer.Seller?.ExtId ?? "",
                    },
                    new ReportData
                    {
                        Key = "Customer.ExtId",
                        Label = "Customer.ExtId",
                        FeatureValue = customer.Customer.ExtId?.ToString().Substring(1) ?? "",
                    },
                    new ReportData
                    {
                        Key = "RazonSocial",
                        Label = "Razón Social",
                        StringValue = customer.Attributes.FirstOrDefault(x => x.Key == "Company")?.Value,
                    },
                    new ReportData
                    {
                        Key = "Email",
                        Label = "Email",
                        StringValue = customer.Customer.Email,
                    },
                    new ReportData
                    {
                        Key = "Telefonos",
                        Label = "Teléfonos",
                        StringValue = customer.Attributes.FirstOrDefault(x => x.Key == "Phone")?.Value,
                    },
                    new ReportData
                    {
                        Key = "ResumenDocumentos",
                        Label = "Resumen de Documentos",
                        StyledTableValue = new()
                        {
                            Headers = new ()
                            {
                                new("Tipo", headerStyles),
                                new("Nro Doc", headerStyles),
                                new("Emisión", headerStyles),
                                new("Días N.", headerStyles),
                                new("Descto. Neg", headerStyles),
                                new("Cobro", headerStyles),
                                new("Días V.", headerStyles),
                                new("Base Imp.", headerStyles),
                                new("Exento", headerStyles),
                                new("Flete", headerStyles),
                                new("IVA", headerStyles),
                                new("Saldo", headerStyles + "background-color: #c4c4c4;"),
                            },

                            Rows = customer.InvoiceList.Select(x => new StyledRow
                            {
                                Cells = {
                                    new (
                                        x.DocumentType == InvoiceType.DeliveryNote ? "NE" : "FAC",
                                        cellStyles
                                    ),  //Tipp
                                    new (x.ExtId ?? "", cellStyles),  //Nro Doc
                                    new (FormatDate(x.CreatedOnUtc), cellStyles), //Emisión
                                    new (x.GetDaysNegotiated()?.ToString() ?? "", cellStyles),    //Días N.
                                    new (x.DiscountCode ?? "", cellStyles), //Descto. Neg
                                    new (FormatDate(x.DueDateUtc), cellStyles),   //Cobro
                                    new (
                                            x.GetDaysPastDue()?.ToString() ?? "",
                                            cellStyles +
                                            (
                                                x.GetDaysPastDue() > 20
                                                ? "background-color: #ffa1a1; font-weight: bold;"
                                                : ""
                                            )
                                    ),    //Días V.
                                    new (FormatDecimal(x.TaxBase), cellStyles),   //Base Imp.
                                    new (FormatDecimal(x.TaxExemptAmount), cellStyles),   //Exento
                                    new (FormatDecimal(x.ShippingAmount), cellStyles),    //Flete
                                    new (FormatDecimal(x.TaxAmount), cellStyles), //IVA
                                    new (
                                        FormatDecimal(x.Balance),
                                        cellStyles + "background-color: #c4c4c4;"
                                    ),   //Saldo
                                },

                                Style = rowStyles
                            }).ToList(),

                            TableStyle = tableStyles,
                            HeaderRowStyle = headerRowStyles
                        }
                    },
                    new ReportData
                    {
                        Key = "TotalGeneral",
                        Label = "Total General",
                        FeatureValue = FormatDecimal(customer.InvoiceList.Sum(x => x.Balance)),
                    }
                }
            };

            reportList.Add(new(report, customer.Customer));
        }

        return reportList;
    }

    #endregion

    #region Nested classes

    private class ReportWithCustomer
    {
        public Report Report { get; set; }
        public Customer Customer { get; set; }

        public ReportWithCustomer(Report report, Customer customer)
        {
            Report = report;
            Customer = customer;
        }
    }

    #endregion
}
