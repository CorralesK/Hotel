using Hotel.src.Application.Services;
using Hotel.src.Core.Enums;
using Hotel.src.Core.Interfaces.IServices;
using Microsoft.Extensions.DependencyInjection;

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
            Console.WriteLine("\n1. Iniciar sesión\ne. Salir");
            Console.Write("Seleccione una opción: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":

                    var serviceProvider = ServiceConfigurator.ConfigureServices();
                    var authService = serviceProvider.GetService<IAuthService>();
                    var jwtService = serviceProvider.GetService<JwtService>();

                    Console.Write("Email: ");
                    string email = Console.ReadLine();
                    Console.Write("Password: ");
                    string password = Console.ReadLine();

                    string token = authService.Authenticate(email, password);
                    string role = jwtService.GetRoleFromToken(token);
                    SessionManager.SetSession(token);

                    if (Enum.Parse<RoleUser>(role) == RoleUser.Admin)
                    {
                        var admin = serviceProvider.GetRequiredService<Admin>();
                        admin.ShowMenu();
                    }
                    else if (Enum.Parse<RoleUser>(role) == RoleUser.User)
                    {
                        var customer = serviceProvider.GetRequiredService<Customer>();
                        customer.ShowMenu();
                    }

                    break;
                case "e":
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
