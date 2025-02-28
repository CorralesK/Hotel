using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Hotel.src.Application.Services;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Core.Interfaces.IServices;
using Hotel.src.Infrastructure.Repositories;
using Hotel.src.Infrastructure.Data;
using Hotel.src.ConsoleUI;
using Hotel.src.Core.Entities;
using Hotel.src.Application.Services.Jobs;
using Hotel.src.Application;
using Hotel.src.Core.Interfaces;

namespace Hotel
{
    public static class ServiceConfigurator
    {
        public static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<ApplicationDbContext>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IAuthService, AuthService>()
                .AddSingleton<JwtService>()
                .AddScoped<ICustomerRepository, CustomerRepository>()
                .AddScoped<IRegisterServices, CustomerServices>()
                .AddScoped<IReservationRepository, ReservationRepository>()
                .AddScoped<IRoomRepository, RoomRepository>()
                .AddScoped<IInvoiceRepository, InvoiceRepository>()
                .AddScoped<IBillingService, BillingService>()
                .AddSingleton<RoomService>()
                .AddScoped<Admin>()  // Registrar Admin
                .AddScoped<Customer>()   // Registrar User
                .AddScoped<INotificationSender, EmailNotificationSender>()
                .AddScoped<INotificationService, NotificationService>()
                .AddScoped<CheckInNotificationJob>()
                .AddScoped<ReservationMenu>()
                .AddScoped<ReservationService>()
                .BuildServiceProvider();
        }

    }
}
