using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepository;

namespace Hotel.src.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly List<Invoice> _invoices = new();

        public Invoice GetById(int id)
        {
            return _invoices.FirstOrDefault(i => i.ID == id);
        }

        public List<Invoice> GetAll()
        {
            return _invoices;
        }

        public void Add(Invoice invoice)
        {
            _invoices.Add(invoice);
        }

        public void Update(Invoice invoice)
        {
            var existingInvoice = GetById(invoice.ID);
            if (existingInvoice != null)
            {
                existingInvoice.DateIssued = invoice.DateIssued;
                existingInvoice.TotalAmount = invoice.TotalAmount;
                existingInvoice.InvoiceDetails = invoice.InvoiceDetails;
            }
        }

        public void Delete(int id)
        {
            var invoice = GetById(id);
            if (invoice != null)
            {
                _invoices.Remove(invoice);
            }
        }
    }
}
