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
        public InvoiceDetail AddInvoiceDetail(InvoiceDetail invoiceDetail)
        {
            if (invoiceDetail == null)
            {
                throw new ArgumentException("No hay detalles de factura para agregar.");
            }

            _dbContext.InvoiceDetails.Add(invoiceDetail);
            _dbContext.SaveChanges();

            return invoiceDetail;
        }
    }
}
