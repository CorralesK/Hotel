using Hotel.src.Hotel.Core.Entities;
using Hotel.src.Hotel.Core.Enums;
using Hotel.src.Hotel.Core.Interfaces.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Hotel.Application.Services
{
    class ReservationService
    {
        /*
        reservar
        cancelar
        obtener reserva por cliente (idCliente)

        obtener resevas por rango de fecha
        */
        private readonly IReservationRepository _reservationRepository;

        //Constructor que recibe el irepositorio como dependencia
        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }
        // Registrar reserva
        public void RegisterReservation(Reservation reservation)
        {
            reservation.CalculateTotalPrice();
            _reservationRepository.Add(reservation);
        }

        // Obtener reserva por cliente(idCliente)
        public List<Reservation> GetReservationsByClientId(int clientId)
        {
            return _reservationRepository.GetByClientId(clientId);
        }

        // Obtener resevas por rango de fecha
        public List<Reservation> GetReservationsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _reservationRepository.GetByDateRange(startDate, endDate);
        }

        // Cancelar reserva
        public void CancelReservation(long reservationId)
        {
            var reservation = _reservationRepository.GetById(reservationId);
            if (reservation != null)
            {
                reservation.STATUS = ReservationStatus.Cancelada;
                _reservationRepository.Update(reservation);
            }
        }

    }
}
