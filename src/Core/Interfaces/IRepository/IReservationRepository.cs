using Hotel.src.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Core.Interfaces.IRepository
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
