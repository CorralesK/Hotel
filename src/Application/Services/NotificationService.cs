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

        public bool SendCheckInNotification(string recipientEmail, string recipientName, DateTime checkInDate, string roomDetails)
        {
            string subject = "Recordatorio de su próxima reserva";


            if (string.IsNullOrEmpty(recipientName))
            {
                recipientName = "usuario";
            }

            string message = $@"
            <html>
            <body>
                <h2>Recordatorio de Check-In</h2>
                <p>Estimado/a {recipientName},</p>
                <p>Le recordamos que su check-in está programado para el <strong>{checkInDate.ToShortDateString()}</strong>.</p>
                <p>Detalles de su reserva: {roomDetails}</p>
                <p>Por favor, asegúrese de tener a mano su identificación y la tarjeta utilizada para la reserva.</p>
                <p>Si necesita modificar su reserva, contáctenos lo antes posible.</p>
                <p>¡Esperamos darle la bienvenida pronto!</p>
                <p>Atentamente,<br>El equipo del Hotel</p>
            </body>
            </html>";
            string formattedRoomDetails = roomDetails.Replace("\n", ", ");

            string plainMessage = $@"
        Recordatorio de Check-In

        Estimado/a {recipientName},
        
        Le recordamos que su check-in está programado para el {checkInDate.ToShortDateString()}.

        Detalles de su reserva: {formattedRoomDetails}

        Por favor, asegúrese de tener a mano su identificación y la tarjeta utilizada para la reserva.

        Si necesita modificar su reserva, contáctenos lo antes posible.

        ¡Esperamos darle la bienvenida pronto!

        Atentamente,
        El equipo del Hotel";

            Console.WriteLine("Mensaje a enviar:");
            Console.WriteLine(plainMessage);

            try
            {
                // Send the email using the sender
                Console.WriteLine("Enviando correo...");
                Console.WriteLine($"Destinatario: {recipientEmail}");
                Console.WriteLine($"Asunto: {subject}");
                Console.WriteLine($"Fecha de Check-in: {checkInDate}");


                bool result = _sender.Send(subject, message, recipientEmail);

                Console.WriteLine($"Resultado del envío: {(result ? "Éxito" : "Fallo")}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                return false;  // Return false in case of any error
            }
        }
    }
}
