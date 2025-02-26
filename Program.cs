using Hotel.src.ConsoleUI;
using System;
using Hotel.src.Application.Services;
using Hotel.src.Infrastructure.Repositories;
using Hotel.src.Infrastructure.Data;
using Hotel.src.Core.Interfaces.IServices;
using Microsoft.Extensions.DependencyInjection;
using Hotel.src.Core.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace Hotel.src.ConsoleUI
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Sistema de Gestión de Reservas de Hotel";
            ShowStartScreen();
        }

        public static void ShowStartScreen()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("     SISTEMA DE RESERVAS DE HOTEL        ");
            Console.WriteLine("=========================================");
            Console.WriteLine("\n1. Iniciar sesión\n2. Salir");
            Console.Write("Seleccione una opción: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    // Obtener el proveedor de servicios configurado en ServiceConfigurator.cs
                    var serviceProvider = ServiceConfigurator.ConfigureServices();
                    // Obtener el servicio de autenticación desde el contenedor DI 
                    var authService = serviceProvider.GetService<IAuthService>();
                    var jwtService = serviceProvider.GetService<JwtService>();


                    // Simulación de autenticación
                    Console.Write("Email: ");
                    string email = Console.ReadLine();
                    Console.Write("Password: ");
                    string password = Console.ReadLine();

                    string token = authService.Authenticate(email, password);
                    string role = jwtService.GetRoleFromToken(token);

                    if (Enum.Parse<RoleUser>(role) == RoleUser.Admin)
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                        Console.WriteLine("\n📜 Contenido del token:");
                        foreach (var claim in jsonToken.Claims)
                        {
                            Console.WriteLine($"➡ {claim.Type}: {claim.Value}");
                        }

                        //Admin admin = new Admin(new RoomService(new RoomRepository(new ApplicationDbContext())));
                        //admin.ShowMenu();

                    }


                    break;
                case "2":
                    Console.WriteLine("Saliendo del sistema...\n");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\nOpción inválida. Intente de nuevo.");
                    Console.ReadKey();
                    ShowStartScreen();
                    break;
            }
        }
    }
}
