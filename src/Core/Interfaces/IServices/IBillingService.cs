using Hotel.src.Core.Entities;

namespace Hotel.src.Core.Interfaces.IServices
{
    interface IBillingService
    {
        Invoice GenerateInvoice(int reservationId);
    }
}
