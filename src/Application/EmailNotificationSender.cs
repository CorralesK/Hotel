using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using dotenv.net;
using Hotel.src.Core.Interfaces;

namespace Hotel.src.Application
{
    public class EmailNotificationSender : INotificationSender
    {
        private readonly string _fromEmail;
        private readonly string _emailPassword;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly ISmtpClient _smtpClient;


        public EmailNotificationSender(ISmtpClient smtpClient = null)
        {
            // Cargar las variables de entorno desde el archivo .env
            DotEnv.Load();
            // Obtener las variables de entorno
            _fromEmail = Environment.GetEnvironmentVariable("EMAIL_FROM_ADDRESS");
            _emailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
            _smtpServer = "smtp.gmail.com";
            _smtpPort = 587;
            _smtpClient = smtpClient ?? new SmtpClientWrapper();
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$", // Expresión regular para validar email
                    RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public bool Send(string subject, string message, string recipient)
        {

            if (string.IsNullOrEmpty(recipient))
            {
                Console.WriteLine("La dirección de correo no puede estar vacía.");
                return false;
            }

            if (!IsValidEmail(recipient))
            {
                Console.WriteLine("Dirección de correo inválida.");
                return false;
            }

            try
            {

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_fromEmail);
                    mail.To.Add(recipient);
                    mail.Subject = subject;
                    mail.Body = message;
                    mail.IsBodyHtml = true;

                    // Usar el cliente inyectado
                    _smtpClient.Host = _smtpServer;
                    _smtpClient.Port = _smtpPort;
                    _smtpClient.Credentials = new NetworkCredential(_fromEmail, _emailPassword);
                    _smtpClient.EnableSsl = true;

                    _smtpClient.Send(mail);
                }

                Console.WriteLine($"Correo enviado correctamente a {recipient}.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                return false;
            }
        }
    }
}
