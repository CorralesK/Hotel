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
        /// Inicializa una nueva instancia de la clase <see cref="ReservationRepository"/>.
        /// </summary>
        /// <param name="context">Contexto de la aplicación para interactuar con la base de datos.</param>
        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Agrega una nueva reserva a la base de datos.
        /// </summary>
        /// <param name="reservation">Objeto de reserva a agregar.</param>
        /// <returns>La reserva agregada.</returns>
        /// <exception cref="ArgumentNullException">Se lanza si la reserva es nula.</exception>
        /// <exception cref="Exception">Se lanza si ocurre un error al guardar la reserva.</exception>
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
        /// Agrega una habitación a una reserva.
        /// </summary>
        /// <param name="reservationRoom">Objeto que relaciona una reserva con una habitación.</param>
        /// <returns>La relación de reserva y habitación agregada.</returns>
        /// <exception cref="ArgumentNullException">Se lanza si el objeto reservationRoom es nulo.</exception>
        /// <exception cref="Exception">Se lanza si ocurre un error al guardar la relación.</exception>
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
        /// Actualiza una reserva existente en la base de datos.
        /// </summary>
        /// <param name="reservation">La reserva a actualizar.</param>
        /// <exception cref="ArgumentNullException">Se lanza si la reserva es nula.</exception>
        /// <exception cref="Exception">Se lanza si ocurre un error al actualizar la reserva.</exception>
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
        /// Obtiene una reserva por su ID.
        /// </summary>
        /// <param name="id">El identificador único de la reserva.</param>
        /// <returns>La reserva correspondiente o null si no se encuentra.</returns>
        public Reservation GetById(int id)
        {
            return _context.Reservations
                .Include(r => r.ReservationRooms)
                .ThenInclude(rr => rr.Room)
                .FirstOrDefault(r => r.ID == id);
        }

        /// <summary>
        /// Obtiene las reservas realizadas por un cliente específico.
        /// </summary>
        /// <param name="clientId">El identificador del cliente.</param>
        /// <returns>Una lista de reservas del cliente.</returns>
        public List<Reservation> GetByClientId(int clientId)
        {
            return _context.Reservations
                .Where(r => r.USERID == clientId)
                .ToList();
        }

        /// <summary>
        /// Obtiene las reservas en un rango de fechas específico.
        /// </summary>
        /// <param name="startDate">La fecha de inicio del rango.</param>
        /// <param name="endDate">La fecha de fin del rango.</param>
        /// <returns>Una lista de reservas que se encuentran dentro del rango de fechas.</returns>
        /// <exception cref="ArgumentException">Se lanza si la fecha de fin es anterior a la fecha de inicio.</exception>
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
        /// Obtiene las reservas próximas según la cantidad de días especificada.
        /// </summary>
        /// <param name="daysAhead">Cantidad de días a partir de hoy para buscar reservas próximas.</param>
        /// <returns>Una lista de reservas próximas.</returns>
        /// <exception cref="ArgumentException">Se lanza si daysAhead es negativo.</exception>
        public List<Reservation> GetUpcomingReservations(int daysAhead)
        {
            if (daysAhead < 0)
            {
                throw new ArgumentException("El parámetro daysAhead no puede ser negativo.");
            }

            DateTime today = DateTime.UtcNow.Date;

            return _context.Reservations
                .Include(r => r.User)
                .Include(r => r.ReservationRooms)
                .ThenInclude(rr => rr.Room)
                .Where(r => (r.STARTDATE.Date - today).Days == daysAhead)
                .ToList();
        }
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
