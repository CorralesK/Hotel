using Hotel.src.Application.Services;
using Hotel.src.ConsoleUI.schemas;
using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using Hotel.src.Core.Interfaces.IServices;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.Results;
using Hotel.src.Core.Interfaces.IRepository;

namespace Hotel.src.ConsoleUI
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
            GenerateHeader("MENÚ DE ADMINISTRADOR");
            Console.WriteLine("\n1. Registrar habitación");
            Console.WriteLine("2. Registrar cliente");
            Console.WriteLine("3. Ver reportes");
            Console.WriteLine("4. Generar factura");
            Console.WriteLine("5. Ver logs de notificaciones check-in");
            Console.WriteLine("6. Salir");
            Console.Write("Seleccione una opción: ");

            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    RegisterRoom();
                    break;
                case "2":
                    RegisterCustumer();
                    break;
                case "3":
                    break;
                case "4":
                    ShowReservations();
                    break;
                case "5":
                    ReadLogs();
                    break;
                case "6":
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
                GenerateHeader("REGISTRO DE HABITACIÓN");

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

        private void RegisterCustumer()
        {
            Console.Clear();
            GenerateHeader("REGISTRO DE CLIENTE");
            var serviceProvider = ServiceConfigurator.ConfigureServices();
            var registerService = serviceProvider.GetRequiredService<IRegisterServices>();
            User user = new User();

            PrintLine("Ingrese el nombre del cliente: ");
            user.NAME = ReadLines();
            PrintLine("Ingrese el correo electrónica del cliente: ");
            user.EMAIL = ReadLines();
            PrintLine("Ingrese la contraseña del cliente: ");
            user.PASSWORD = ReadLines();
            user.ROLE = RoleUser.User;

            UserValidator validator = new UserValidator();
            ValidationResult result = validator.Validate(user);

            // Check if the object is valid or not
            if (result.IsValid)
            {
                registerService.RegisterCustomer(user);
                Console.WriteLine("Usuario registrado correctamente.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.ErrorMessage}");
                }
            }

            Console.ReadKey();
            ShowMenu();
        }

        public void GenerateHeader(string title)
        {
            Console.WriteLine(new string('=', 40));
            Console.WriteLine("\t" + title + "\t");
            Console.WriteLine(new string('=', 40));
        }

        public void PrintLine(string message)
        {
            Console.WriteLine(message);
        }
        public string ReadLines() => Console.ReadLine();


        private void ReadLogs()
        {
            try
            {
                Console.Clear();
                GenerateHeader("LEER ARCHIVO DE LOGS");

                // Solicitar fecha en formato yyyy-MM-dd
                Console.Write("Ingrese la fecha en formato (yyyy-MM-dd) para leer los logs: ");
                string dateInput = Console.ReadLine();
                DateTime selectedDate;

                if (DateTime.TryParse(dateInput, out selectedDate))
                {
                    string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", $"logs_{selectedDate:yyyy-MM-dd}.txt");

                    if (File.Exists(logFilePath))
                    {
                        string logContents = File.ReadAllText(logFilePath);
                        Console.WriteLine("\n=========================================");
                        Console.WriteLine($"      Logs del {selectedDate:yyyy-MM-dd}      ");
                        Console.WriteLine("=========================================");
                        Console.WriteLine(logContents);
                    }
                    else
                    {
                        Console.WriteLine("No se encontraron logs para la fecha seleccionada.");
                    }
                }
                else
                {
                    Console.WriteLine("Fecha no válida. Intente nuevamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer el archivo de log: {ex.Message}");
            }

            Console.ReadKey();
            ShowMenu();
        }
        private void GenerateInvoice(int reservationId)
        {
            try
            {
                var serviceProvider = ServiceConfigurator.ConfigureServices();
                var reservationRepository = serviceProvider.GetRequiredService<IReservationRepository>();
                var invoiceRepository = serviceProvider.GetRequiredService<IInvoiceRepository>();
                var invoiceDetailsRepository = serviceProvider.GetRequiredService<IInvoiceDetailsRepository>();
                var billingService = new BillingService(reservationRepository, invoiceRepository, invoiceDetailsRepository);
                var invoice = billingService.GenerateInvoice(reservationId);
                var reservation = reservationRepository.GetById(reservationId);
                if (reservation != null)
                {
                    reservation.STATUS = ReservationStatus.Pagada;
                    reservationRepository.Update(reservation);
                }

                Console.WriteLine("\nFactura generada con éxito:");
                Console.WriteLine($"ID: {invoice.ID}, Fecha: {invoice.DateIssued}, Total: {invoice.TotalAmount}");

                var uniqueDetails = new HashSet<InvoiceDetail>(invoice.InvoiceDetails);
                foreach (var detail in uniqueDetails)
                {
                    Console.WriteLine($"Habitación: {detail.Room.TYPE} | Precio Total: {detail.Price}" +
                        $"| Fecha de la reserva | Desde: {detail.Reservation.STARTDATE}| Hasta:{detail.Reservation.ENDDATE} ");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
            ShowMenu();
        }
        private void ShowReservations()
        {
            Console.Clear();
            GenerateHeader("LISTA DE RESERVAS");

            var serviceProvider = ServiceConfigurator.ConfigureServices();
            var reservationRepository = serviceProvider.GetRequiredService<IReservationRepository>();
            var customerRepository = serviceProvider.GetRequiredService<ICustomerRepository>();

            var reservations = reservationRepository.GetConfirmedReservations(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(30));

            if (reservations.Count == 0)
            {
                Console.WriteLine("No hay reservas disponibles.");
            }
            else
            {
                foreach (var reservation in reservations)
                {
                    var user = customerRepository.GetbyId(reservation.USERID);
                    var clientName = user != null ? user.NAME : "N/A";
                    Console.WriteLine($"ID: {reservation.ID}, Cliente: {clientName}, Desde: {reservation.STARTDATE:yyyy-MM-dd}, Hasta:{reservation.ENDDATE:yyyy-MM-dd} ");
                }

                Console.Write("\nIngrese el ID de la reserva para generar la factura: ");
                if (int.TryParse(Console.ReadLine(), out int reservationId))
                {
                    GenerateInvoice(reservationId);
                }
                else
                {
                    Console.WriteLine("ID inválido. Intente de nuevo.");
                }
            }

            Console.ReadKey();
            ShowMenu();
        }

    }

}