using Hotel.src.Core.Interfaces;
using Hotel.src.Core.Interfaces.IServices;

namespace Hotel.src.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationSender _sender;

        public NotificationService(INotificationSender sender)
        {
            _sender = sender;
        }

        public void SendCheckInNotification(string recipientEmail, string recipientName, DateTime checkInDate, string roomDetails)
        {
            string subject = "Recordatorio de su próxima reserva";

            string message = $@"
            <html>
            <body>
                <h2>Recordatorio de Check-In</h2>
                <p>Estimado/a {recipientName},</p>
                <p>Le recordamos que su check-in está programado para el <strong>{checkInDate.ToShortDateString()}</strong>.</p>
                <p>Detalles de su habitación: {roomDetails}</p>
                <p>Por favor, asegúrese de tener a mano su identificación y la tarjeta utilizada para la reserva.</p>
                <p>Si necesita modificar su reserva, contáctenos lo antes posible.</p>
                <p>¡Esperamos darle la bienvenida pronto!</p>
                <p>Atentamente,<br>El equipo del Hotel</p>
            </body>
            </html>";

            _sender.Send(subject, message, recipientEmail);
        }
    }
}
