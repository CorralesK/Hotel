using Hotel.src.Core.Entities;

namespace Hotel.src.Core.Interfaces.IRepository
{
    public interface IInvoiceRepository
    {
        Invoice AddInvoice(Invoice invoice);
    }
}
