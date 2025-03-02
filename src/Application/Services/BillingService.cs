using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Core.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Hotel.src.Application.Services
{
    class BillingService : IBillingService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceDetailsRepository _invoiceDetailsRepository;

        public BillingService(IReservationRepository reservationRepository, IInvoiceRepository invoiceRepository, IInvoiceDetailsRepository invoiceDetailsRepository)
        {
            _reservationRepository = reservationRepository;
            _invoiceRepository = invoiceRepository;
            _invoiceDetailsRepository = invoiceDetailsRepository;
        }

        public Invoice GenerateInvoice(int reservationId)
        {
            var reservation = _reservationRepository.GetById(reservationId);
            if (reservation == null)
            {
                throw new Exception("La reserva no existe.");
            }

            // Crear la factura usando TOTALPRICE de la reserva
            Invoice invoice = new Invoice
            {
                DateIssued = DateTime.UtcNow,
                TotalAmount = (float)reservation.TOTALPRICE,
                InvoiceDetails = new List<InvoiceDetail>()
            };

            // Guardar la factura primero para obtener un ID real
            _invoiceRepository.AddInvoice(invoice);
          

            // Crear los detalles de la factura
            foreach (var reservationRoom in reservation.ReservationRooms)
            {
                invoice.InvoiceDetails.Add(new InvoiceDetail
                {
                    InvoiceID = invoice.ID,  // Ahora sí tiene un ID válido
                    RoomID = reservationRoom.Room.ID,
                    ReservationID = reservationId,
                    Price = (float)(reservationRoom.Room.PRICEPERNIGHT * (reservation.ENDDATE - reservation.STARTDATE).TotalDays)
                });
            }

            // Agregar los detalles a la base de datos
            _invoiceDetailsRepository.AddInvoiceDetails(invoice.InvoiceDetails);
           
/*
            // Obtener los detalles guardados desde la base de datos
            invoice.InvoiceDetails = _invoiceDetailsRepository.GetByInvoiceId(invoice.ID);
*/
            return invoice;

        }
    }
}
