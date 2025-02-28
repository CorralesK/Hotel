using Hotel.src.Application.Services;
using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.ConsoleUI
{
    class ReservationMenu
    {
        private readonly ReservationService _reservationService;
        private readonly RoomService _roomService;
        private readonly Customer _customerMenu;
        public User user;

        public ReservationMenu(ReservationService reservationService, RoomService roomService)
        {
            _reservationService = reservationService;
            _roomService = roomService;
        }

        public void ShowReservationMenu()
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
                    CancelRoomInReservation();
                    break;
                case "3":
                    ShowReservationsHistory();
                    break;
                case "4":
                    _customerMenu.ShowMenu();
                    break;
                default:
                    Console.WriteLine("\nOpción inválida. Intente de nuevo.");
                    Console.ReadKey();
                    ShowReservationMenu();
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
            user = new User();
            int clientId = user.ID;
            
            Console.Write("Ingrese la fecha de inicio (DD/MM/YYYY): ");
            DateTime startDate;
            while (!DateTime.TryParse(Console.ReadLine(), out startDate))
            {
                Console.Write("Fecha inválida. Intente de nuevo (DD/MM/YYYY): ");
            }

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
                ShowReservationMenu();
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
                ShowReservationMenu();
                return;
            }
            // Registrar la reserva usando el servicio
            _reservationService.RegisterReservation(clientId, startDate, endDate);

            Console.WriteLine("\nReserva registrada con éxito. ID de reserva: " + reservation.ID);
            Console.WriteLine("Precio total: $" + reservation.TOTALPRICE);
            // Generar Factura
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
            ShowReservationMenu();
        }
        private void CancelRoomInReservation()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("    CANCELAR HABITACIÓN EN RESERVA       ");
            Console.WriteLine("=========================================");
            user = new User();
            int clientId = user.ID;

            // Obtener las reservas del cliente
            var reservations = _reservationService.GetReservationsByClientId(clientId);

            if (reservations.Count == 0)
            {
                Console.WriteLine("\nNo tiene reservas activas.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                ShowReservationMenu();
                return;
            }

            // Mostrar solo las reservas activas
            var activeReservations = _reservationService.GetActiveReservationsByClientId(clientId);

            if (activeReservations.Count == 0)
            {
                Console.WriteLine("\nNo tiene reservas activas que pueda modificar.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                ShowReservationMenu();
                return;
            }

            Console.WriteLine("\nSus reservas activas:");
            Console.WriteLine("----------------------");
            for (int i = 0; i < activeReservations.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Reserva | Desde: {activeReservations[i].STARTDATE.ToShortDateString()} | Hasta: {activeReservations[i].ENDDATE.ToShortDateString()} | Precio: ${activeReservations[i].TOTALPRICE}");
            }

            // Seleccionar reserva
            Console.Write("\nSeleccione el número de la reserva que desea modificar: ");
            int selectedReservationIndex;
            while (!int.TryParse(Console.ReadLine(), out selectedReservationIndex) || selectedReservationIndex < 1 || selectedReservationIndex > activeReservations.Count)
            {
                Console.Write("Número de reserva inválido. Intente de nuevo: ");
            }

            var selectedReservation = activeReservations[selectedReservationIndex - 1];

            // Mostrar las habitaciones de la reserva seleccionada
            Console.WriteLine("\nHabitaciones en la reserva seleccionada:");
            Console.WriteLine("----------------------------------------");
            int roomIndex = 1;
            foreach (var reservationRoom in selectedReservation.ReservationRooms)
            {
                var room = reservationRoom.Room;
                Console.WriteLine($"{roomIndex}. Habitación {room.ROOMNUMBER} | Tipo: {room.TYPE} | Precio por noche: ${room.PRICEPERNIGHT}");
                roomIndex++;
            }

            // Seleccionar habitación a cancelar
            Console.Write("\nSeleccione el número de la habitación que desea cancelar: ");
            int selectedRoomIndex;
            while (!int.TryParse(Console.ReadLine(), out selectedRoomIndex) || selectedRoomIndex < 1 || selectedRoomIndex > selectedReservation.ReservationRooms.Count)
            {
                Console.Write("Número de habitación inválido. Intente de nuevo: ");
            }

            var selectedRoom = selectedReservation.ReservationRooms.ElementAt(selectedRoomIndex - 1).Room;

            // Confirmar cancelación
            Console.Write($"\n¿Está seguro que desea cancelar la habitación {selectedRoom.ROOMNUMBER}? (S/N): ");
            string confirmation = Console.ReadLine().ToUpper();

            if (confirmation == "S")
            {
                _reservationService.CancelRoomInReservation(selectedReservation.ID, selectedRoom.ID);
                Console.WriteLine("\nHabitación cancelada con éxito.");
            }
            else
            {
                Console.WriteLine("\nCancelación de habitación cancelada.");
            }

            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
            ShowReservationMenu();
        }

        private void ShowReservationsHistory()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("         HISTORIAL DE RESERVAS          ");
            Console.WriteLine("=========================================");

            // Obtener ID del cliente
            user = new User();
            int clientId = user.ID;

            // Obtener las reservas del cliente
            var reservations = _reservationService.GetReservationsByClientId(clientId);

            if (reservations.Count == 0)
            {
                Console.WriteLine("\nNo tiene reservas registradas.");
            }
            else
            {
                Console.WriteLine($"\nSe encontraron {reservations.Count} reservas:");
                foreach (var reservation in reservations)
                {
                    Console.WriteLine("\n-----------------------------------------");
                    Console.WriteLine($"Fecha inicio: {reservation.STARTDATE.ToShortDateString()}");
                    Console.WriteLine($"Fecha fin: {reservation.ENDDATE.ToShortDateString()}");
                    Console.WriteLine($"Precio total: ${reservation.TOTALPRICE}");
                    Console.WriteLine($"Estado: {reservation.STATUS}");

                    Console.WriteLine("\nHabitaciones:");
                    foreach (var roomReservation in reservation.ReservationRooms)
                    {
                        Console.WriteLine($"- Número de habitación: {roomReservation.Room.ROOMNUMBER} | Tipo: {roomReservation.Room.TYPE} | Precio: ${roomReservation.Room.PRICEPERNIGHT}/noche");
                    }
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
            ShowReservationMenu();
            
        }

    }
}
