using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using Hotel.src.Core.Interfaces.IRepository;

namespace Hotel.src.Application.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        /// <summary>
        /// Initializes a new instance of <see cref="ReservationService"/>.
        /// </summary>
        /// <param name="reservationRepository">Repository for accessing reservation data.</param>
        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }
        /// <summary>
        /// Register a new reservation in the system.
        /// </summary>
        /// <param name=“reservation”>The reservation to register.
        /// <exception cref=“ArgumentNullException”>Throws if the reservation is null.</exception>.
        /// <exception cref=“Exception”>Thrown if the reservation dates are invalid or if an error occurs during registration.</exception>
        public void RegisterReservation(Reservation reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation), "La reserva no puede ser nula.");
            }

            if (reservation.STARTDATE > reservation.ENDDATE)
            {
                throw new Exception("Las fechas de la reserva son inválidas.");
            }

            _reservationRepository.Add(reservation);
        }

        /// <summary>
        /// Add a room to an existing reservation.
        /// </summary>
        /// <param name=“reservationRoom”>Object that relates the reservation to the room.</param>
        /// <returns>The relationship of reservation and room added.</returns>.
        /// <exception cref=“ArgumentNullException”>Thrown if the reservationRoom object is null.</exception>.
        public ReservationRoom AddRoomToReservation(ReservationRoom reservationRoom)
        {
            if (reservationRoom == null)
            {
                throw new ArgumentNullException(nameof(reservationRoom), "La relación de reserva y habitación no puede ser nula.");
            }

            return _reservationRepository.AddRoomInReservation(reservationRoom);
        }

        /// <summary>
        //// Updates an existing reservation.
        /// </summary>
        /// <param name=“reservation”>The reservation to update.
        /// <exception cref=“ArgumentNullException”>Thrown if the reservation is null.</exception>
        public void UpdateReservation(Reservation reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation), "La reserva no puede ser nula.");
            }
            if(reservation.ReservationRooms == null || !reservation.ReservationRooms.Any())
            {
                CancelRoomInReservation(reservation.ID, null);
                return;
            }

            _reservationRepository.Update(reservation);
        }

        /// <summary>.
        /// Gets the reservations made by a customer.
        /// </summary>
        /// <param name=“clientId”>The client's identifier.
        /// <returns>A list of reservations made by the client.</returns>.
        public List<Reservation> GetReservationsByClientId(int clientId)
        {
            return _reservationRepository.GetByClientId(clientId);
        }

        /// <summary>
        /// Gets the bookings in a specific date range.
        /// </summary>
        /// <param name=“startDate”>The start date of the range.</param> /// <param name=“startDate”>The start date of the range.
        /// <param name=“endDate”>The end date of the range.
        /// <returns>A list of bookings that fall within the date range.</returns>
        /// <exception cref=“ArgumentException”>Thrown if the end date is before the start date.</exception>.
        public List<Reservation> GetReservationsByDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException("La fecha de fin no puede ser menor a la fecha de inicio.");
            }

            return _reservationRepository.GetByDateRange(startDate, endDate);
        }

        /// <summary>
        /// Gets the active (confirmed) reservations for a customer.
        /// </summary>
        /// <param name=“clientId”>The client's identifier.
        /// <returns>A list of confirmed reservations of the client.</returns>.
        public List<Reservation> GetActiveReservationsByClientId(int clientId)
        {
            return _reservationRepository.GetByClientId(clientId)
                .Where(r => r.STATUS == ReservationStatus.Confirmada)
                .ToList();
        }

        /// <summary>.
        //// Cancels a room within a reservation.
        /// </summary>
        /// <param name=“reservationId”>The reservation identifier.</param> /// <param name=“reservationId”>The reservation identifier.
        /// <param name=“roomId”>The identifier of the room to be canceled.</param>
        /// <exception cref=“Exception”>
        /// Thrown if the reservation does not exist, is already cancelled or if the room is not found in the reservation.
        /// </exception>
        public void CancelRoomInReservation(int reservationId, int? roomId)
        {
            var reservation = _reservationRepository.GetById(reservationId);
            if (reservation == null)
            {
                throw new Exception("La reserva no existe.");
            }
            if (reservation.STATUS == ReservationStatus.Cancelada)
            {
                throw new Exception("No se puede cancelar una habitación de una reserva ya cancelada.");
            }
            var roomToRemove = reservation.ReservationRooms.FirstOrDefault(rr => rr.RoomID == roomId);
            if (roomToRemove == null)
            {
                throw new Exception("La habitación no está en la reserva.");
            }
            if (reservation.STATUS == ReservationStatus.Pendiente)
            {
                reservation.STATUS = ReservationStatus.Cancelada;
                throw new Exception("La reserva esta cancelada.");
            }

            reservation.ReservationRooms.Remove(roomToRemove);
            reservation.CalculateTotalPrice();

            if (!reservation.ReservationRooms.Any())
            {
                reservation.STATUS = ReservationStatus.Cancelada;
            }
            _reservationRepository.Update(reservation);
        }
    }
}
