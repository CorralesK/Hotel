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
            job.Execute();
        }
    }
}
