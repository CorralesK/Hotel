using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Core.Interfaces.IServices;
using Hotel.src.Infrastructure.Loggins;
using System.Text.RegularExpressions;

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
            int notificationDaysAhead = 2)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _notificationDaysAhead = notificationDaysAhead;
        }

        public void Execute()
        {
            // Se obtienen las reservas usando el método del repositorio, que ya filtra por fecha objetivo
            List<Reservation> upcomingReservations = _reservationRepository.GetUpcomingReservations(_notificationDaysAhead);

            // Calcular la fecha objetivo de la misma forma que en el repositorio
            DateTime targetDateLocal = DateTime.Now.Date.AddDays(_notificationDaysAhead);
            DateTime targetDateUtc = TimeZoneInfo.ConvertTimeToUtc(targetDateLocal, TimeZoneInfo.Local);

            LogService.LogNotification($"Ejecutando job de notificaciones de check-in ({DateTime.Now})...");
            Console.WriteLine($"Ejecutando job de notificaciones de check-in ({DateTime.Now})...");
            LogService.LogNotification($"Se encontraron {upcomingReservations.Count} reservas próximas.");
            Console.WriteLine($"Se encontraron {upcomingReservations.Count} reservas próximas.");

            foreach (var reservation in upcomingReservations)
            {
                // Verificar que la reserva corresponda a la fecha objetivo
                if (reservation.STARTDATE.Date != targetDateUtc.Date)
                {
                    LogService.LogNotification($"Reserva ID {reservation.ID} descartada: fecha {reservation.STARTDATE.ToShortDateString()} no coincide con la fecha objetivo {targetDateUtc.ToShortDateString()}.");
                    Console.WriteLine($"Reserva ID {reservation.ID} descartada: fecha {reservation.STARTDATE.ToShortDateString()} no coincide con la fecha objetivo {targetDateUtc.ToShortDateString()}.");
                    continue;
                }

                User user = reservation.User;

                // Validación del email
                if (user != null && !string.IsNullOrEmpty(user.EMAIL) && IsValidEmail(user.EMAIL))
                {
                    string roomDetails = string.Join("<br><br>", reservation.ReservationRooms.Select(rr =>
                        $"<br><b>Habitación - {rr.Room.ROOMNUMBER}:</b><br>" +
                        $"• Tipo: {rr.Room.TYPE}<br>" +
                        $"• Precio por noche: {rr.Room.PRICEPERNIGHT}<br>" +
                        $"• Capacidad: {rr.Room.CAPACITY}<br>" +
                        $"• Estado: {rr.Room.STATUS}"
                    ));

                    bool notificationSent = _notificationService.SendCheckInNotification(
                        user.EMAIL,
                        user.NAME,
                        reservation.STARTDATE,
                        roomDetails
                    );

                    if (notificationSent)
                    {
                        LogService.LogNotification($"Notificación enviada a {user.NAME} ({user.EMAIL}) para la reserva del {reservation.STARTDATE.ToShortDateString()}");
                        Console.WriteLine($"Notificación enviada a {user.NAME} ({user.EMAIL}) para la reserva del {reservation.STARTDATE.ToShortDateString()}");
                    }
                    else
                    {
                        LogService.LogNotification($"Error al enviar la notificación a {user.NAME} ({user.EMAIL}) para la reserva del {reservation.STARTDATE.ToShortDateString()}");
                        Console.WriteLine($"Error al enviar la notificación a {user.NAME} ({user.EMAIL}) para la reserva del {reservation.STARTDATE.ToShortDateString()}");
                    }
                }
                else
                {
                    LogService.LogNotification($"No se pudo enviar notificación para la reserva ID {reservation.ID}: información de usuario incompleta o email inválido.");
                    Console.WriteLine($"No se pudo enviar notificación para la reserva ID {reservation.ID}: información de usuario incompleta o email inválido.");
                }
            }

            LogService.LogNotification("Job de notificaciones de check-in completado.");
            Console.WriteLine("Job de notificaciones de check-in completado.");
        }

        // Método auxiliar para validar el email
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }





    }
}
