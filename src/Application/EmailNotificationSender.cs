using dotenv.net;
using Hotel.src.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Application
{
    public class EmailNotificationSender : INotificationSender
    {
        private readonly string _fromEmail;
        private readonly string _emailPassword;
        private readonly string _smtpServer;
        private readonly int _smtpPort;

        public EmailNotificationSender()
        {
            // Cargar las variables de entorno desde el archivo .env
            DotEnv.Load();

            // Obtener las variables de entorno
            _fromEmail = Environment.GetEnvironmentVariable("EMAIL_FROM_ADDRESS");
            _emailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
            _smtpServer = "smtp.gmail.com";
            _smtpPort = 587;
        }

        public bool Send(string subject, string message, string recipient)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_fromEmail);
                    mail.To.Add(recipient);
                    mail.Subject = subject;
                    mail.Body = message;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(_smtpServer))
                    {
                        smtp.Port = _smtpPort;
                        smtp.Credentials = new System.Net.NetworkCredential(_fromEmail, _emailPassword);
                        smtp.EnableSsl = true;

                        smtp.Send(mail);
                    }
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
