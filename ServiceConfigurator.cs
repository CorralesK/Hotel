using Hotel.src.Application;
using Hotel.src.Application.Services;
using Hotel.src.Application.Services.Jobs;
using Hotel.src.ConsoleUI;
using Hotel.src.Core.Interfaces;
using Hotel.src.Core.Interfaces.IRepositories;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Core.Interfaces.IServices;
using Hotel.src.Infrastructure.Data;
using Hotel.src.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

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
                .AddScoped<IInvoiceDetailsRepository, InvoiceDetailsRepository>()
                .AddScoped<IBillingService, BillingService>()
                .AddSingleton<RoomService>()
                .AddSingleton<ReservationService>()
                .AddScoped<Admin>()  // Registrar Admin
                .AddScoped<Customer>()   // Registrar User
                .AddScoped<INotificationSender, EmailNotificationSender>()
                .AddScoped<INotificationService, NotificationService>()
                .AddScoped<IOccupancyRepository, OccupancyRepository>()
                .AddScoped<IOccupancyReportService, OccupancyReportService>()
                .AddScoped<ReservationMenu>()
                .AddScoped<ReservationService>()
                .AddScoped<CheckInNotificationJob>()
                .BuildServiceProvider();
        }

    }
}