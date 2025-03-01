using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Core.Interfaces.IServices;

namespace Hotel.src.Application.Services
{
    class BillingService : IBillingService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public BillingService(IReservationRepository reservationRepository, IInvoiceRepository invoiceRepository)
        {
            _reservationRepository = reservationRepository;
            _invoiceRepository = invoiceRepository;
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
                DateIssued = DateTime.Now,
                TotalAmount = (float)reservation.TOTALPRICE,
                InvoiceDetails = new List<InvoiceDetail>()
            };

            // Crear los detalles de la factura
            foreach (var reservationRoom in reservation.ReservationRooms)
            {
                invoice.InvoiceDetails.Add(new InvoiceDetail
                {
                    ReservationID = reservationId,
                    RoomID = reservationRoom.Room.ID,
                    Price = (float)(reservationRoom.Room.PRICEPERNIGHT * (reservation.ENDDATE - reservation.STARTDATE).TotalDays)
                });
            }

            _invoiceRepository.Add(invoice);
            return invoice;
        }
    }
}
