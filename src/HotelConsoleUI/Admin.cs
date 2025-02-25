using Hotel.src.HotelApplication.Services;
using Hotel.src.HotelCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.HotelConsoleUI
{
    class Admin
    {
        private readonly RoomService _roomService;

        public Admin(RoomService roomService)
        {
            _roomService = roomService;
        }

        public void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("             MENÚ DE ADMINISTRADOR       ");
            Console.WriteLine("=========================================");
            Console.WriteLine("\n1. Registrar habitación");
            Console.WriteLine("2. Registrar cliente");
            Console.WriteLine("3. Ver reportes");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");

            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    RegisterRoom();
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    Program.ShowStartScreen();
                    break;
                default:
                    Console.WriteLine("\nOpción inválida. Intente de nuevo.");
                    Console.ReadKey();
                    ShowMenu();
                    break;
            }
        }

        private void RegisterRoom()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=========================================");
                Console.WriteLine("     REGISTRO DE HABITACIÓN             ");
                Console.WriteLine("=========================================");
                Console.Write("Ingrese el número de la habitación: ");
                string roomNumber = Console.ReadLine();

                Console.Write("Ingrese el tipo de habitación (1 - SIMPLE, 2 - DOBLE, 3 - SUITE): ");
                int roomType = int.Parse(Console.ReadLine());
                RoomType type = (RoomType)(roomType - 1);  // Convert to enum value

                Console.Write("Ingrese el precio por noche: ");
                double pricePerNight = double.Parse(Console.ReadLine());

                Console.Write("Ingrese la capacidad de la habitación: ");
                int capacity = int.Parse(Console.ReadLine());

                var room = _roomService.RegisterRoom(roomNumber, type, pricePerNight, capacity);

                Console.WriteLine("\n¡Habitación registrada exitosamente!");
                Console.WriteLine($"Número de habitación: {room.ROOMNUMBER}, Tipo: {room.TYPE}, Precio: {room.PRICEPERNIGHT}, Capacidad: {room.CAPACITY}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("\nError: Formato de dato inválido. Por favor, ingrese los valores correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError inesperado: {ex.Message}");
            }

            Console.ReadKey();
            ShowMenu();
        }
    }

}
