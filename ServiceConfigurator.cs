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

namespace Hotel
{
    public static class ServiceConfigurator
    {
        public static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<ApplicationDbContext>()  // Instancia única de la BD
                .AddScoped<IUserRepository, UserRepository>() // Se instancia por cada solicitud
                .AddScoped<IAuthService, AuthService>() // Servicio de autenticación
                .AddSingleton<JwtService>() // Servicio JWT (puede ser singleton si no almacena estado)
                .BuildServiceProvider();
        }

    }
}
