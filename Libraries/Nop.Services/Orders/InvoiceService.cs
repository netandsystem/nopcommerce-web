using DocumentFormat.OpenXml.Presentation;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Orders;

//NaS Code

#nullable enable

public class InvoiceService : IInvoiceService
{
    #region Fields

    private readonly IRepository<Invoice> _repository;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<GenericAttribute> _genericAttributeRepository;

    #endregion

    #region Ctor

    public InvoiceService(
        IRepository<Invoice> repository,
        IRepository<Customer> customerRepository,
        IRepository<GenericAttribute> genericAttributeRepository
    )
    {
        _repository = repository;
        _customerRepository = customerRepository;
        _genericAttributeRepository = genericAttributeRepository;
    }

    #endregion

    #region Methods

    public virtual async Task DeleteAsync(IList<Invoice> entities)
    {
        await _repository.DeleteAsync(entities);
    }

    public virtual async Task<Invoice> GetInvoiceByIdAsync(int invoiceId)
    {
        return await _repository.GetByIdAsync(invoiceId);
    }

    public virtual async Task InsertAsync(IList<Invoice> entities)
    {
        await _repository.InsertAsync(entities);
    }

    public virtual async Task UpdateAsync(IList<Invoice> entities)
    {
        await _repository.UpdateAsync(entities);
    }

    /// <summary>
    /// Retorna las facturas vencidas
    /// </summary>
    /// <returns></returns>

    public virtual IQueryable<Invoice> GetOverdueInvoicesQuery()
    {
        /*
         * Una factura vencida es aquella que tiene un balance > 0 y la fecha de cobro es menor a la fecha actual
         *         */

        return from i in _repository.Table
               where i.Balance > 0 && i.DueDateUtc.HasValue && i.DueDateUtc < DateTime.Now
               orderby i.DueDateUtc ascending
               select i;
    }


    public virtual async Task<List<CustomerWithInvoiceList>> GetOverdueInvoicesWithCustomersAsync()
    {
        var invoices = await GetOverdueInvoicesQuery().ToListAsync();
        var customers = await GetCustomerWithAttributesQuery(invoices.Select(i => i.CustomerId).ToList()).ToListAsync();

        var query = from c in customers
                    join i in invoices
                           on c.Customer.Id equals i.CustomerId
                           into invoicesList
                    where invoicesList.Any()
                    select new CustomerWithInvoiceList(c.Customer, c.Seller, c.Attributes, invoicesList.ToList());

        return query.ToList();
    }

    //public virtual IQueryable<CustomerWithInvoice> GetOverdueInvoicesWithCustomersQuery()
    //{
    //    return from i in GetOverdueInvoicesQuery()
    //           join c in GetCustomerWithAttributesQuery()
    //               on i.CustomerId equals c.Customer.Id
    //           select new CustomerWithInvoice(c.Customer, c.Attributes, i);
    //}


    //public virtual async Task<List<Invoice>> GetOverdueInvoicesWithCustomersQuery()
    //{
    //    var q = from i in GetOverdueInvoicesQuery()
    //            join c in _customerRepository.Table
    //                on i.CustomerId equals c.Id
    //            join attribute in _genericAttributeRepository.Table
    //               on c.Id equals attribute.EntityId
    //               into attributesList
    //            select new CustomerWithAttributes(c, attributesList.ToList(), i);

    //}



    #endregion

    #region Private methods

    private IQueryable<CustomerWithAttributes> GetCustomerWithAttributesQuery(IList<int>? ids = null)
    {
        return from customer in _customerRepository.Table
               join seller in _customerRepository.Table
                    on customer.SellerId equals seller.Id
               join attribute in _genericAttributeRepository.Table
                   on customer.Id equals attribute.EntityId
                   into attributesList
               where ids == null || ids.Contains(customer.Id)
               select new CustomerWithAttributes(customer, seller, attributesList.ToList());
    }

    #endregion

    #region Nested classes

    private class CustomerWithAttributes
    {
        public Customer Customer { get; set; }
        public Customer? Seller { get; set; }
        public List<GenericAttribute> Attributes { get; set; }

        public CustomerWithAttributes(Customer customer, Customer? seller, List<GenericAttribute> attributes)
        {
            Customer = customer;
            Seller = seller;
            Attributes = attributes;
        }
    }

    #endregion
}

//public class CustomerWithInvoice
//{
//    public Customer Customer { get; set; }
//    public IList<GenericAttribute> Attributes { get; set; }
//    public Invoice Invoice { get; set; }

//    public CustomerWithInvoice(Customer customer, IList<GenericAttribute> attributes, Invoice invoice)
//    {
//        Customer = customer;
//        Attributes = attributes;
//        Invoice = invoice;
//    }
//}

public class CustomerWithInvoiceList
{
    public Customer Customer { get; set; }
    public Customer? Seller { get; set; }
    public List<GenericAttribute> Attributes { get; set; }
    public List<Invoice> InvoiceList { get; set; }

    public CustomerWithInvoiceList(Customer customer, Customer? seller, IList<GenericAttribute> attributes, IList<Invoice> invoiceList)
    {
        Customer = customer;
        Seller = seller;
        Attributes = attributes.ToList();
        InvoiceList = invoiceList.ToList();
    }
}
