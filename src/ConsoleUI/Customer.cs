using Hotel.src.Application.Services;
using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;

namespace Hotel.src.ConsoleUI
{
    class Customer
    {
        private readonly RoomService _roomService;
        private readonly ReservationMenu _reservationMenu;
        public User user;

        public Customer(RoomService roomService, ReservationMenu reservationMenu)
        {
            _roomService = roomService;
            _reservationMenu = reservationMenu;
        }


        public void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("             MENÚ DE CLIENTE            ");
            Console.WriteLine("=========================================");
            Console.WriteLine("\n1. Buscar habitaciones");
            Console.WriteLine("2. Ver disponibilidad");
            Console.WriteLine("3. Reservar");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");

            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    SearchRooms();
                    break;
                case "2":
                    ShowDisponibility();
                    break;
                case "3":

                    _reservationMenu.ShowReservationMenu();
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

        private void SearchRooms()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=========================================");
                Console.WriteLine("       BÚSQUEDA DE HABITACIONES        ");
                Console.WriteLine("=========================================");
                Console.WriteLine("\nSelecciona un filtro de búsqueda:");

                // Opción de filtro: tipo de habitación
                Console.WriteLine("1. Filtrar por tipo de habitación");
                Console.WriteLine("2. Filtrar por rango de precio");
                Console.WriteLine("3. Filtrar por disponibilidad");
                Console.WriteLine("4. No aplicar filtro");
                Console.Write("\nSeleccione el filtro (1-4): ");
                string filterChoice = Console.ReadLine();

                RoomType? type = null;
                double? minPrice = null;
                double? maxPrice = null;
                DateTime? startDate = null;
                DateTime? endDate = null;

                switch (filterChoice)
                {
                    case "1":
                        // Filtro por tipo de habitación
                        Console.WriteLine("\nSeleccione el tipo de habitación:");
                        Console.WriteLine("1 - SIMPLE\n2 - DOBLE\n3 - SUITE");
                        Console.Write("Tipo de habitación: ");
                        string roomTypeInput = Console.ReadLine();
                        if (int.TryParse(roomTypeInput, out int roomType) && roomType >= 1 && roomType <= 3)
                        {
                            type = (RoomType)(roomType - 1);  // Convierte la opción a RoomType
                        }
                        else
                        {
                            Console.WriteLine("Opción de tipo de habitación no válida.");
                            return;  // Regresa al menú si se ingresa un valor inválido
                        }
                        break;

                    case "2":
                        // Filtro por precio
                        Console.Write("\nIngrese el precio mínimo: ");
                        string minPriceInput = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(minPriceInput) && double.TryParse(minPriceInput, out double minPriceValue))
                        {
                            minPrice = minPriceValue;
                        }

                        Console.Write("Ingrese el precio máximo: ");
                        string maxPriceInput = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(maxPriceInput) && double.TryParse(maxPriceInput, out double maxPriceValue))
                        {
                            maxPrice = maxPriceValue;
                        }
                        break;

                    case "3":
                        // Filtro por disponibilidad con fechas
                        Console.Write("\nIngrese la fecha de entrada (DD/MM/AAAA): ");
                        string startDateInput = Console.ReadLine();
                        if (DateTime.TryParseExact(startDateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedStartDate))
                        {
                            if (parsedStartDate >= DateTime.Now)
                            {
                                startDate = parsedStartDate;
                            }
                            else
                            {
                                Console.WriteLine("La fecha de entrada no puede ser anterior a la fecha actual.");
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Fecha de entrada inválida.");
                            return;
                        }

                        Console.Write("\nIngrese la fecha de salida (DD/MM/AAAA): ");
                        string endDateInput = Console.ReadLine();
                        if (DateTime.TryParseExact(endDateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedEndDate))
                        {
                            if (parsedEndDate > startDate)
                            {
                                endDate = parsedEndDate;
                            }
                            else
                            {
                                Console.WriteLine("La fecha de salida debe ser posterior a la de entrada.");
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Fecha de salida inválida.");
                            return;
                        }
                        break;

                    case "4":
                        break;

                    default:
                        Console.WriteLine("Opción no válida.");
                        return;
                }

                // Realizar la búsqueda
                var rooms = _roomService.SearchRooms(type, minPrice, maxPrice, startDate, endDate);

                // Mostrar los resultados
                if (rooms.Any())
                {
                    Console.WriteLine("\nResultados encontrados:");
                    foreach (var room in rooms)
                    {
                        Console.WriteLine($"Habitación: {room.ROOMNUMBER}, Tipo: {room.TYPE}, Precio: {room.PRICEPERNIGHT} por noche, Capacidad: {room.CAPACITY}");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo se encontraron habitaciones que coincidan con los criterios de búsqueda.");
                }
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

        private void ShowDisponibility()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=========================================");
                Console.WriteLine("        CONSULTA DE DISPONIBILIDAD       ");
                Console.WriteLine("=========================================");

                // Solicitar fecha de entrada
                Console.Write("\nIngrese la fecha de entrada (DD/MM/AAAA): ");
                string startDateInput = Console.ReadLine();
                if (!DateTime.TryParseExact(startDateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
                {
                    Console.WriteLine("Fecha de entrada inválida. Presione cualquier tecla para volver al menú.");
                    Console.ReadKey();
                    ShowMenu();
                    return;
                }

                // Solicitar fecha de salida
                Console.Write("Ingrese la fecha de salida (DD/MM/AAAA): ");
                string endDateInput = Console.ReadLine();
                if (!DateTime.TryParseExact(endDateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime endDate))
                {
                    Console.WriteLine("Fecha de salida inválida. Presione cualquier tecla para volver al menú.");
                    Console.ReadKey();
                    ShowMenu();
                    return;
                }

                // Validar que la fecha de salida sea posterior a la de entrada
                if (endDate <= startDate)
                {
                    Console.WriteLine("La fecha de salida debe ser posterior a la de entrada. Presione cualquier tecla para volver al menú.");
                    Console.ReadKey();
                    ShowMenu();
                    return;
                }

                // Llamar al servicio para obtener las habitaciones disponibles
                var availableRooms = _roomService.CheckAvailability(startDate, endDate);

                // Mostrar resultados
                Console.Clear();
                Console.WriteLine("=========================================");
                Console.WriteLine("        HABITACIONES DISPONIBLES         ");
                Console.WriteLine("=========================================");

                if (availableRooms.Any())
                {
                    foreach (var room in availableRooms)
                    {
                        Console.WriteLine($"Habitación: {room.ROOMNUMBER}, Tipo: {room.TYPE}, Precio: {room.PRICEPERNIGHT} por noche, Capacidad: {room.CAPACITY}");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo hay habitaciones disponibles para las fechas seleccionadas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError inesperado: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para volver al menú.");
            Console.ReadKey();
            ShowMenu();
        }
        public void ReturnToMenu()
        {

            ShowMenu();

        }
    }
}
