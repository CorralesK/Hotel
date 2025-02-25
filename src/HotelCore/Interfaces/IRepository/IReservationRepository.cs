using Hotel.src.HotelCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.HotelCore.Interfaces.IRepository
{
    interface IReservationRepository
    {
        void Add(Reservation reservation);
        void Update(Reservation reservation);
        Reservation GetById(int id);
        List<Reservation> GetByClientId(int clientId);
        List<Reservation> GetByDateRange(DateTime startDate, DateTime endDate);
    }
}
