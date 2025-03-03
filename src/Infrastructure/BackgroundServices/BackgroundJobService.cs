using Hotel.src.Application.Services.Jobs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Infrastructure.BackgroundServices
{
    public class BackgroundJobService
    {
        public async Task RunJobInBackground(CancellationToken token)
        {
            DateTime currentDate = DateTime.Today;

            while (!token.IsCancellationRequested)
            {
                DateTime today = DateTime.Today;

                if (today != currentDate)
                {
                    ExecuteJob();
                    currentDate = today;
                }

                // Evita consumir demasiada CPU al ceder el control al sistema
                await Task.Yield();
            }
        }

        private void ExecuteJob()
        {
            var serviceProvider = ServiceConfigurator.ConfigureServices();
            var job = serviceProvider.GetRequiredService<CheckInNotificationJob>();

            // Ejecutar siempre en modo silencioso
            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);
                job.Execute();
                Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            }
        }

    }
}
