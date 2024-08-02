using Nop.Core.Domain.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Services.Orders;

//NaS Code

#nullable enable

public interface IInvoiceService
{
    IQueryable<Invoice> GetOverdueInvoicesQuery();
    Task<List<CustomerWithInvoiceList>> GetOverdueInvoicesWithCustomersAsync();
}
