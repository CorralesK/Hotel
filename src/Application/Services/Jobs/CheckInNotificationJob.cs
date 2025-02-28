using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Application.Services.Jobs
{
    public class CheckInNotificationJob
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly int _notificationDaysAhead;

        public CheckInNotificationJob(
            IReservationRepository reservationRepository,
            IUserRepository userRepository,
            INotificationService notificationService,
            int notificationDaysAhead = 3)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _notificationDaysAhead = notificationDaysAhead;
        }

        public void Execute()
        {
            Console.WriteLine($"Ejecutando job de notificaciones de check-in ({DateTime.Now})...");

            // Obtener las reservas que se aproximan
            List<Reservation> upcomingReservations = _reservationRepository.GetUpcomingReservations(_notificationDaysAhead);

            Console.WriteLine($"Se encontraron {upcomingReservations.Count} reservas próximas.");

            foreach (var reservation in upcomingReservations)
            {
                // Obtener información del usuario
                User user = _userRepository.GetById(reservation.USERID);

                if (user != null && !string.IsNullOrEmpty(user.EMAIL))
                {
                    string roomDetails = string.Join("<br><br>", reservation.ReservationRooms.Select(rr =>
                        $"<br><b>Habitación - {rr.Room.ROOMNUMBER}:</b><br>" +
                        $"• Tipo: {rr.Room.TYPE}<br>" +
                        $"• Precio por noche: {rr.Room.PRICEPERNIGHT}<br>" +
                        $"• Capacidad: {rr.Room.CAPACITY}<br>" +
                        $"• Estado: {rr.Room.STATUS}"
                    ));

                    // Enviar la notificación
                    _notificationService.SendCheckInNotification(
                        user.EMAIL,
                        user.NAME,
                        reservation.STARTDATE,
                        roomDetails);

                    Console.WriteLine($"Notificación enviada a {user.NAME} ({user.EMAIL}) para la reserva del {reservation.STARTDATE.ToShortDateString()}");
                }
                else
                {
                    Console.WriteLine($"No se pudo enviar notificación para la reserva ID {reservation.ID}: información de usuario incompleta");
                }
            }

            Console.WriteLine("Job de notificaciones de check-in completado.");
        }
    }
}
