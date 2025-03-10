﻿using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Infrastructure.Data;

namespace Hotel.src.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public InvoiceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Invoice AddInvoice(Invoice invoice)
        {
            if (invoice == null)
            {
                throw new ArgumentNullException(nameof(invoice), "La factura no puede ser nula.");
            }
            _dbContext.Invoices.Add(invoice);
            _dbContext.SaveChanges();
            return invoice;
        }

    }
}
