using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Infrastructure.Repositories
{
    public class InvoiceDetailsRepository : IInvoiceDetailsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public InvoiceDetailsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public InvoiceDetail AddInvoiceDetails(ICollection<InvoiceDetail> invoiceDetails)
        {
            _dbContext.InvoiceDetails.AddRange(invoiceDetails);  // ✅ Usa AddRange() para insertar la lista
            _dbContext.SaveChanges();

            return invoiceDetails.FirstOrDefault();  // ✅ Devuelve el primer elemento (o null si está vacío)
        }
    }
}
