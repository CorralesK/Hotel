using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using Hotel.src.Core.Interfaces.IRepository;

namespace Hotel.src.Application.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ReservationService"/>.
        /// </summary>
        /// <param name="reservationRepository">Repositorio para acceder a los datos de las reservas.</param>
        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }
        /// <summary>
        /// Registra una nueva reserva en el sistema.
        /// </summary>
        /// <param name="reservation">La reserva a registrar.</param>
        /// <exception cref="ArgumentNullException">Se lanza si la reserva es nula.</exception>
        /// <exception cref="Exception">Se lanza si las fechas de la reserva son inválidas o si ocurre un error durante el registro.</exception>
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
        /// Agrega una habitación a una reserva existente.
        /// </summary>
        /// <param name="reservationRoom">Objeto que relaciona la reserva con la habitación.</param>
        /// <returns>La relación de reserva y habitación agregada.</returns>
        /// <exception cref="ArgumentNullException">Se lanza si el objeto reservationRoom es nulo.</exception>
        public ReservationRoom AddRoomToReservation(ReservationRoom reservationRoom)
        {
            if (reservationRoom == null)
            {
                throw new ArgumentNullException(nameof(reservationRoom), "La relación de reserva y habitación no puede ser nula.");
            }

            return _reservationRepository.AddRoomInReservation(reservationRoom);
        }

        /// <summary>
        /// Actualiza una reserva existente.
        /// </summary>
        /// <param name="reservation">La reserva a actualizar.</param>
        /// <exception cref="ArgumentNullException">Se lanza si la reserva es nula.</exception>
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

        /// <summary>
        /// Obtiene las reservas realizadas por un cliente.
        /// </summary>
        /// <param name="clientId">El identificador del cliente.</param>
        /// <returns>Una lista de reservas del cliente.</returns>
        public List<Reservation> GetReservationsByClientId(int clientId)
        {
            return _reservationRepository.GetByClientId(clientId);
        }

        /// <summary>
        /// Obtiene las reservas en un rango de fechas específico.
        /// </summary>
        /// <param name="startDate">La fecha de inicio del rango.</param>
        /// <param name="endDate">La fecha de fin del rango.</param>
        /// <returns>Una lista de reservas que se encuentran dentro del rango de fechas.</returns>
        /// <exception cref="ArgumentException">Se lanza si la fecha de fin es anterior a la fecha de inicio.</exception>
        public List<Reservation> GetReservationsByDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException("La fecha de fin no puede ser menor a la fecha de inicio.");
            }

            return _reservationRepository.GetByDateRange(startDate, endDate);
        }

        /// <summary>
        /// Obtiene las reservas activas (confirmadas) de un cliente.
        /// </summary>
        /// <param name="clientId">El identificador del cliente.</param>
        /// <returns>Una lista de reservas confirmadas del cliente.</returns>
        public List<Reservation> GetActiveReservationsByClientId(int clientId)
        {
            return _reservationRepository.GetByClientId(clientId)
                .Where(r => r.STATUS == ReservationStatus.Confirmada)
                .ToList();
        }

        /// <summary>
        /// Cancela una habitación dentro de una reserva.
        /// </summary>
        /// <param name="reservationId">El identificador de la reserva.</param>
        /// <param name="roomId">El identificador de la habitación a cancelar.</param>
        /// <exception cref="Exception">
        /// Se lanza si la reserva no existe, ya se encuentra cancelada o si la habitación no se encuentra en la reserva.
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
