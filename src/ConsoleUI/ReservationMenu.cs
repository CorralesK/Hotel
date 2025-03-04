using Hotel.src.Application.Services;
using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;

namespace Hotel.src.ConsoleUI
{
    class ReservationMenu
    {
        private readonly ReservationService _reservationService;
        private readonly RoomService _roomService;
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
            Console.WriteLine("4. Cerrar Sesión");
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
                    Program.ShowStartScreen();
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

            int clientId = SessionManager.UserId;

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

            Reservation reservation = new Reservation();
            reservation.USERID = clientId;
            reservation.STARTDATE = startDate;
            reservation.ENDDATE = endDate;
            reservation.STATUS = ReservationStatus.Pendiente;

            _reservationService.RegisterReservation(reservation);

            // Get available rooms
            var availableRooms = _roomService.CheckAvailability(startDate, endDate);

            if (availableRooms.Count() == 0)
            {
                Console.WriteLine("\nNo hay habitaciones disponibles para las fechas seleccionadas.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                ShowReservationMenu();
                return;
            }

            // Show available rooms
            Console.WriteLine("\nHabitaciones disponibles para las fechas seleccionadas:");
            Console.WriteLine("--------------------------------------------------------");
            foreach (var room in availableRooms)
            {
                Console.WriteLine($"Número de habitación: {room.ROOMNUMBER} | Tipo: {room.TYPE} | Precio por noche: ${room.PRICEPERNIGHT}");
            }

            // Add rooms to the reservation
            bool addMoreRooms = true;
            while (addMoreRooms)
            {
                Console.Write("\nIngrese el número de la habitación que desea reservar: ");
                string roomNumber = Console.ReadLine().ToUpper();

                // Find the room by room number (ROOMNUMBER)
                var room = availableRooms.FirstOrDefault(r => r.ROOMNUMBER == roomNumber);

                if (room == null)
                {
                    Console.WriteLine("La habitación no existe o no está disponible. Intente con otro número.");
                    continue;
                }
                if (!availableRooms.Any())
                {
                    Console.WriteLine("\nNo hay habitaciones disponibles.");
                    _reservationService.CancelRoomInReservation(reservation.ID, null);
                    Console.ReadKey();
                    ShowReservationMenu();
                    return;
                }

                // Create and add the reservation-room relation
                ReservationRoom reservationRoom = new ReservationRoom
                {
                    ReservationID = reservation.ID,
                    RoomID = room.ID,
                    Reservation = reservation,
                    Room = room
                };

                var addreservationRoom = _reservationService.AddRoomToReservation(reservationRoom);

                Console.Write("¿Desea añadir otra habitación? (S/N): ");
                string response = Console.ReadLine().ToUpper();
                addMoreRooms = (response == "S");

            }
            // Check if rooms have been added
            if (reservation.ReservationRooms.Count == 0)
            {
                _reservationService.CancelRoomInReservation(reservation.ID, null);
                Console.WriteLine("\nNo se añadieron habitaciones a la reserva. La operación ha sido cancelada.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                ShowReservationMenu();
                return;
            }

            reservation.CalculateTotalPrice();
            reservation.STATUS = ReservationStatus.Confirmada;
            // Register the reservation 
            _reservationService.UpdateReservation(reservation);

            Console.WriteLine($"\nReserva registrada con éxito. Reserva desde: " +
                $"{reservation.STARTDATE} hasta {reservation.ENDDATE}");
            Console.WriteLine("Precio total: $" + reservation.TOTALPRICE);
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
            int clientId = SessionManager.UserId;

            // Get customer reservations
            var reservations = _reservationService.GetReservationsByClientId(clientId);

            if (reservations.Count == 0)
            {
                Console.WriteLine("\nNo tiene reservas activas.");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                ShowReservationMenu();
                return;
            }

            // Show only active reservations
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

            // Select reservation
            Console.Write("\nSeleccione el número (1,2,3..) de la reserva que desea modificar: ");
            int selectedReservationIndex;
            while (!int.TryParse(Console.ReadLine(), out selectedReservationIndex) || selectedReservationIndex < 1 || selectedReservationIndex > activeReservations.Count)
            {
                Console.Write("Número de reserva inválido. Intente de nuevo: ");
            }

            var selectedReservation = activeReservations[selectedReservationIndex - 1];

            // Show the rooms of the selected reservation
            Console.WriteLine("\nHabitaciones en la reserva seleccionada:");
            Console.WriteLine("----------------------------------------");
            int roomIndex = 1;
            foreach (var reservationRoom in selectedReservation.ReservationRooms)
            {
                var room = reservationRoom.Room;
                Console.WriteLine($"{roomIndex}. Habitación {room.ROOMNUMBER} | Tipo: {room.TYPE} | Precio por noche: ${room.PRICEPERNIGHT}");
                roomIndex++;
            }

            // Select room to cancel
            Console.Write("\nSeleccione el número (1,2,3..) de la habitación que desea cancelar: ");
            int selectedRoomIndex;
            while (!int.TryParse(Console.ReadLine(), out selectedRoomIndex) || selectedRoomIndex < 1 || selectedRoomIndex > selectedReservation.ReservationRooms.Count)
            {
                Console.Write("Número de habitación inválido. Intente de nuevo: ");
            }

            var selectedRoom = selectedReservation.ReservationRooms.ElementAt(selectedRoomIndex - 1).Room;

            // Confirm cancellation
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

            int clientId = SessionManager.UserId;

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
