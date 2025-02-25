using Hotel.src.HotelConsoleUI;
using System;
using Hotel.src.HotelApplication.Services;
using Hotel.src.HotelInfrastructure.Repositories;
using Hotel.src.HotelInfrastructure.Data;

namespace Hotel.src.HotelConsoleUI
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
                    //Aqui se llama el metodo que muestra ek login pero lo voy a usar pata probar mis funcionalidades.

                    // Crear una instancia de Admin y mostrar su menú
                    Admin admin = new Admin(new RoomService(new RoomRepository(new ApplicationDbContext())));
                    admin.ShowMenu();

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
