using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.src.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationRepository"/> class.
        /// </summary>
        /// <param name="context">Application context for interacting with the database.</param>
        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new reservation to the database.
        /// </summary>
        /// <param name="reservation">Reservation object to be added.</param>
        /// <returns>The added reservation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the reservation is null.</exception>
        /// <exception cref="Exception">Thrown if an error occurs while saving the reservation.</exception>
        public Reservation Add(Reservation reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation), "La reserva no puede ser nula.");
            }

            try
            {
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
                return reservation;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar la reserva.", ex);
            }
        }

        /// <summary>
        /// Adds a room to a reservation.
        /// </summary>
        /// <param name="reservationRoom">Object that links a reservation with a room.</param>
        /// <returns>The added reservation-room relationship.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the reservationRoom object is null.</exception>
        /// <exception cref="Exception">Thrown if an error occurs while saving the relationship.</exception>
        public ReservationRoom AddRoomInReservation(ReservationRoom reservationRoom)
        {
            if (reservationRoom == null)
            {
                throw new ArgumentNullException(nameof(reservationRoom), "La relación de reserva y habitación no puede ser nula.");
            }

            try
            {
                _context.ReservationRooms.Add(reservationRoom);
                _context.SaveChanges();
                return reservationRoom;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar la habitación a la reserva.", ex);
            }
        }

        /// <summary>
        /// Updates an existing reservation in the database.
        /// </summary>
        /// <param name="reservation">The reservation to update.</param>
        /// <exception cref="ArgumentNullException">Thrown if the reservation is null.</exception>
        /// <exception cref="Exception">Thrown if an error occurs while updating the reservation.</exception>
        public void Update(Reservation reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation), "La reserva no puede ser nula.");
            }

            try
            {
                _context.Reservations.Update(reservation);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la reserva.", ex);
            }
        }

        /// <summary>
        /// Retrieves a reservation by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the reservation.</param>
        /// <returns>The corresponding reservation or null if not found.</returns>
        public Reservation GetById(int id)
        {
            return _context.Reservations
                .Include(r => r.ReservationRooms)
                .ThenInclude(rr => rr.Room)
                .FirstOrDefault(r => r.ID == id);
        }

        /// <summary>
        /// Retrieves reservations made by a specific client.
        /// </summary>
        /// <param name="clientId">The client's identifier.</param>
        /// <returns>A list of the client's reservations.</returns>
        public List<Reservation> GetByClientId(int clientId)
        {
            return _context.Reservations
                .Where(r => r.USERID == clientId)
                .ToList();
        }

        /// <summary>
        /// Retrieves reservations within a specific date range.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A list of reservations within the specified date range.</returns>
        /// <exception cref="ArgumentException">Thrown if the end date is earlier than the start date.</exception>
        public List<Reservation> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException("La fecha de fin no puede ser menor a la fecha de inicio.");
            }

            return _context.Reservations
                .Where(r => r.STARTDATE >= startDate && r.ENDDATE <= endDate)
                .ToList();
        }

        /// <summary>
        /// Retrieves upcoming reservations based on the specified number of days ahead.
        /// </summary>
        /// <param name="daysAhead">Number of days from today to search for upcoming reservations.</param>
        /// <returns>A list of upcoming reservations.</returns>
        /// <exception cref="ArgumentException">Thrown if daysAhead is negative.</exception>
        public List<Reservation> GetUpcomingReservations(int daysAhead)
        {
            if (daysAhead < 0)
            {
                throw new ArgumentException("La cantidad de días no puede ser negativa.");
            }
            // Get local target date (no time)
            DateTime targetDateLocal = DateTime.Now.Date.AddDays(daysAhead);

            // Convert local target date to UTC
            DateTime targetDateUtc = TimeZoneInfo.ConvertTimeToUtc(targetDateLocal, TimeZoneInfo.Local);

            return _context.Reservations
                .Include(r => r.User)
                .Include(r => r.ReservationRooms)
                .ThenInclude(rr => rr.Room)
                .Where(r => r.STARTDATE.Date == targetDateUtc.Date)
                .ToList();
        }
        /// <summary>
        /// Retrieves confirmed reservations within a specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A list of confirmed reservations.</returns>
        /// <exception cref="ArgumentException">Thrown if the end date is earlier than the start date.</exception>
        public List<Reservation> GetConfirmedReservations(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException("La fecha de fin no puede ser menor a la fecha de inicio.");
            }
            return _context.Reservations
                .AsTracking()
                .Where(r => r.STARTDATE.Date >= startDate
                         && r.ENDDATE.Date <= endDate
                         && r.STATUS == ReservationStatus.Confirmada)
                .ToList();
        }
    }
}
