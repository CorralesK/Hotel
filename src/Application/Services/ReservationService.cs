﻿using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using Hotel.src.Core.Interfaces.IRepository;

namespace Hotel.src.Application.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        //Constructor que recibe el irepositorio como dependencia
        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }


        public void RegisterReservation(Reservation reservation)
        {
            _reservationRepository.Add(reservation);

        }
        public ReservationRoom AddRoomToReservation(ReservationRoom reservationRoom)
        {
            var reservationRoomAdd = _reservationRepository.AddRoomInReservation(reservationRoom);
            return reservationRoomAdd;
        }
        public void UpdateReservation(Reservation reservation)
        {
            _reservationRepository.Update(reservation);
        }
        public List<Reservation> GetReservationsByClientId(int clientId)
        {
            return _reservationRepository.GetByClientId(clientId);
        }

        public List<Reservation> GetReservationsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _reservationRepository.GetByDateRange(startDate, endDate);
        }
        public List<Reservation> GetActiveReservationsByClientId(int clientId)
        {
            return _reservationRepository.GetByClientId(clientId)
                .Where(r => r.STATUS == ReservationStatus.Confirmada)
                .ToList();
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

                    // Si ya no tiene habitaciones, cancelar toda la reserva
                    if (!reservation.ReservationRooms.Any())
                    {
                        reservation.STATUS = ReservationStatus.Cancelada;
                    }

                    _reservationRepository.Update(reservation);
                }
            }
        }
    }
}
