using Hotel.src.Core.Entities;


namespace Hotel.src.Core.Interfaces.IRepository
{
    public interface IInvoiceDetailsRepository
    {
    InvoiceDetail AddInvoiceDetails(ICollection<InvoiceDetail> invoices);
    }
}
