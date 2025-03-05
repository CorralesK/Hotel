using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Infrastructure.Loggins
{
    public static class LogService
    {
        private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        // Método para obtener el archivo de log para hoy
        private static string GetLogFilePath()
        {
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }

            // Nombre del archivo de log con la fecha actual
            return Path.Combine(LogDirectory, $"logs_{DateTime.Now:yyyy-MM-dd}.txt");
        }

        public static void LogNotification(string message)
        {
            try
            {
                string logFilePath = GetLogFilePath();
                string logMessage = $"{DateTime.Now}: {message}";

                // Escribir el mensaje de log al final del archivo
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al escribir el log: {ex.Message}");
            }
        }
    }
}
