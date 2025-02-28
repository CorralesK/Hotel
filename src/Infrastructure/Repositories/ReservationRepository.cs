using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Hotel.src.Infrastructure.Repositories
{
    class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository()
        {

        }
        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para agregar una nueva reserva
        public Reservation Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return reservation;
        }

        // Método para actualizar una reserva existente
        public void Update(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            _context.SaveChanges();
        }

        // Método para obtener una reserva por su ID
        public Reservation GetById(int id)
        {
            return _context.Reservations.FirstOrDefault(r => r.ID == id);
        }

        // Método para obtener las reservas de un cliente
        public List<Reservation> GetByClientId(int clientId)
        {
            return _context.Reservations.Where(r => r.USERID == clientId).ToList();
        }

        // Método para obtener las reservas en un rango de fechas
        public List<Reservation> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.Reservations
                .Where(r => r.STARTDATE >= startDate && r.ENDDATE <= endDate)
                .ToList();

        }
        public List<Reservation> GetUpcomingReservations(int daysAhead)
        {
            DateTime targetDate = DateTime.UtcNow.Date.AddDays(daysAhead);

            return _context.Reservations
                .Include(r => r.User)
                .Include(r => r.ReservationRooms)
                .ThenInclude(rr => rr.Room)
                .Where(r => r.STARTDATE.Date <= targetDate)
                .ToList();
        }

    }
}
