using Hotel.src.Core.Entities;

namespace Hotel.src.Core.Interfaces.IRepository
{
    public interface IInvoiceRepository
    {
        Invoice GetById(int id);
        List<Invoice> GetAll();
        void Add(Invoice invoice);
        void Update(Invoice invoice);
        void Delete(int id);
    }
}
