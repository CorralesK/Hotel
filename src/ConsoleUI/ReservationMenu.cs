using Hotel.src.Application.Services;
using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.ConsoleUI
{
    class ReservationMenu
    {
        private readonly ReservationService _reservationService;
        private readonly RoomService _roomService;
        private readonly JwtService _jwtService;

        public ReservationMenu(ReservationService reservationService, JwtService jwtService, RoomService roomService)
        {
            _reservationService = reservationService;
            _jwtService = jwtService;
            _roomService = roomService;
        }

        public void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("           MENÚ DE RESERVAS             ");
            Console.WriteLine("=========================================");
            Console.WriteLine("\n1. Registrar nueva reserva");
            Console.WriteLine("2. Cancelar habitación en reserva");
            Console.WriteLine("3. Historial de reservas");
            Console.WriteLine("4. Regresar al menú principal");
            Console.Write("Seleccione una opción: ");

            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    RegisterReservation();
                    break;
                case "2":
                    //CancelRoomInReservation();
                    break;
                case "3":
                   // GetReservationsByClientId();
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
        private void RegisterReservation()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("         REGISTRAR NUEVA RESERVA         ");
            Console.WriteLine("=========================================");

            // Obtener ID del cliente
            int clientId = _jwtService.GetUserIdFromToken(token);
            // Obtener fecha de inicio
            Console.Write("Ingrese la fecha de inicio (DD/MM/YYYY): ");
            DateTime startDate;
            while (!DateTime.TryParse(Console.ReadLine(), out startDate))
            {
                Console.Write("Fecha inválida. Intente de nuevo (DD/MM/YYYY): ");
            }
            // Obtener fecha de fin
            Console.Write("Ingrese la fecha de fin (DD/MM/YYYY): ");
            DateTime endDate;
            while (!DateTime.TryParse(Console.ReadLine(), out endDate) || endDate <= startDate)
            {
                Console.Write("Fecha inválida o anterior a la fecha de inicio. Intente de nuevo (DD/MM/YYYY): ");
            }
            
            // Crear la reserva
            Reservation reservation = new Reservation(clientId, startDate, endDate, 0, ReservationStatus.Confirmada);
            // Obtener habitaciones disponibles
            var availableRooms = _roomService.CheckAvailability(startDate, endDate);

            if (availableRooms.Count() == 0)
            {
                Console.WriteLine("\nNo hay habitaciones disponibles para las fechas seleccionadas.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                ShowMenu();
                return;
            }

            // Mostrar habitaciones disponibles
            Console.WriteLine("\nHabitaciones disponibles para las fechas seleccionadas:");
            Console.WriteLine("--------------------------------------------------------");
            foreach (var room in availableRooms)
            {
                Console.WriteLine($"ID: {room.ROOMNUMBER} | Tipo: {room.TYPE} | Precio por noche: ${room.PRICEPERNIGHT}");
            }

            // Añadir habitaciones a la reserva
            bool addMoreRooms = true;
            while (addMoreRooms)
            {
                Console.Write("\nIngrese el número de la habitación que desea reservar: ");
                int roomNumber;
                while (!int.TryParse(Console.ReadLine(), out roomNumber))
                {
                    Console.Write("Número de habitación inválido. Ingrese un número: ");
                }

                // Aquí deberíamos verificar que la habitación existe y está disponible
                var room = _roomService.GetRoomById(roomNumber);
                if (room == null)
                {
                    Console.WriteLine("La habitación no existe. Intente con otro ID.");
                    continue;
                }

                // Crear y añadir la relación reserva-habitación
                ReservationRoom reservationRoom = new ReservationRoom
                {
                    ReservationID = reservation.ID,
                    RoomID = room.ID,
                    Reservation = reservation,
                    Room = room
                };

                reservation.ReservationRooms.Add(reservationRoom);

                Console.Write("¿Desea añadir otra habitación? (S/N): ");
                string response = Console.ReadLine().ToUpper();
                addMoreRooms = (response == "S");
            }
            // Verificar si se añadieron habitaciones
            if (reservation.ReservationRooms.Count == 0)
            {
                Console.WriteLine("\nNo se añadieron habitaciones a la reserva. La operación ha sido cancelada.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                ShowMenu();
                return;
            }
            // Registrar la reserva usando el servicio
            _reservationService.RegisterReservation(clientId, startDate, endDate);

            Console.WriteLine("\nReserva registrada con éxito. ID de reserva: " + reservation.ID);
            Console.WriteLine("Precio total: $" + reservation.TOTALPRICE);
            // Generar Factura
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
            ShowMenu();
        }

    }
}
