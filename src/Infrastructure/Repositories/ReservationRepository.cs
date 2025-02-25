using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using Hotel.src.Core.Interfaces.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Infrastructure.Repositories
{
    class ReservationRepository : IReservationRepository
    {
        private readonly List<Reservation> _reservations = new List<Reservation>();

        // Método para agregar una nueva reserva
        public void Add(Reservation reservation)
        {
            reservation.ID = _reservations.Count + 1;
            _reservations.Add(reservation);
        }

        // Método para actualizar una reserva existente
        public void Update(Reservation reservation)
        {
            var existingReservation = _reservations.FirstOrDefault(r => r.ID == reservation.ID);
            if (existingReservation != null)
            {
                existingReservation = reservation;
            }
        }

        // Método para obtener una reserva por su ID
        public Reservation GetById(int id)
        {
            return _reservations.FirstOrDefault(r => r.ID == id);
        }

        // Método para obtener las reservas de un cliente
        public List<Reservation> GetByClientId(int clientId)
        {
            return _reservations.Where(r => r.CLIENTID == clientId).ToList();
        }

        // Método para obtener las reservas en un rango de fechas
        public List<Reservation> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return _reservations.Where(r => r.STARTDATE >= startDate && r.ENDDATE <= endDate).ToList();
        }
    }
}
