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
        private readonly IReservationRepository _reservationRepository;

        //Constructor que recibe el irepositorio como dependencia
        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }


        public void RegisterReservation(int clientId, DateTime startDate, DateTime endDate)
        {
            var reservation = new Reservation(clientId, startDate, endDate, 0, ReservationStatus.Confirmada);
            reservation.CalculateTotalPrice();
            _reservationRepository.Add(reservation);
        }
        
        public List<Reservation> GetReservationsByClientId(int clientId)
        {
            return _reservationRepository.GetByClientId(clientId);
        }

        public List<Reservation> GetReservationsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _reservationRepository.GetByDateRange(startDate, endDate);
        }

        // Cancelar habitación de una reserva
        public void CancelRoomInReservation(int reservationId, int roomId)
        {
            var reservation = _reservationRepository.GetById(reservationId);
            if (reservation != null)
            {
                var roomToRemove = reservation.ReservationRooms.FirstOrDefault(rr => rr.RoomID == roomId);
                if (roomToRemove != null)
                {
                    reservation.ReservationRooms.Remove(roomToRemove);
                    reservation.CalculateTotalPrice();
                    _reservationRepository.Update(reservation);
                }
            }
        }
    }
}
