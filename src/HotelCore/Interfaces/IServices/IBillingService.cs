using Hotel.src.HotelCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.HotelCore.Interfaces.IServices
{
    interface IBillingService
    {
        Invoice GenerateInvoice(int reservationId);
    }
}
