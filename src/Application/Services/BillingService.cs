using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Core.Interfaces.IServices;

namespace Hotel.src.Application.Services
{
    public class BillingService : IBillingService
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

            if (reservation.ReservationRooms == null || !reservation.ReservationRooms.Any())
            {
                throw new Exception("No hay habitaciones asociadas a esta reserva.");
            }


            // Create invoice without details yet
            var invoice = new Invoice
            {
                DateIssued = DateTime.UtcNow,
                TotalAmount = (float)reservation.TOTALPRICE,
                InvoiceDetails = new List<InvoiceDetail>()
            };

            // Save the invoice in the database to get a real ID
            _invoiceRepository.AddInvoice(invoice);

            // Generate invoice details
            foreach (var reservationRoom in reservation.ReservationRooms)
            {
                var nights = (reservation.ENDDATE - reservation.STARTDATE).TotalDays;
                decimal roomPrice = (decimal)reservationRoom.Room.PRICEPERNIGHT * (decimal)nights;

                InvoiceDetail invoiceDetail = new InvoiceDetail(invoice.ID, reservationRoom.Room.ID, reservationId, roomPrice);

                // Save the invoice detail in the database
                var detail = _invoiceDetailsRepository.AddInvoiceDetail(invoiceDetail);
                invoice.InvoiceDetails.Add(detail);
            }

            return invoice;
        }
    }
}
