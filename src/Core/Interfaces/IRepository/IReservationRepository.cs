using Hotel.src.Core.Entities;

namespace Hotel.src.Core.Interfaces.IRepository
{
    public interface IReservationRepository
    {
        Reservation Add(Reservation reservation);
        void Update(Reservation reservation);
        Reservation GetById(int id);
        List<Reservation> GetByClientId(int clientId);
        List<Reservation> GetByDateRange(DateTime startDate, DateTime endDate);
        List<Reservation> GetUpcomingReservations(int daysAhead);

    }
}
