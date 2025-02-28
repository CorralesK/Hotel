using Hotel.src.Application.Services;
using Hotel.src.ConsoleUI.schemas;
using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using Hotel.src.Core.Interfaces.IServices;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.Results;
using Sprache;
using System.Data;
using Moq;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Metadata;
using Hotel.src.Infrastructure.Repositories;
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
            Console.WriteLine("=========================================");
            Console.WriteLine("             MENÚ DE ADMINISTRADOR       ");
            Console.WriteLine("=========================================");
            Console.WriteLine("\n1. Registrar habitación");
            Console.WriteLine("2. Registrar cliente");
            Console.WriteLine("3. Ver reportes");
            Console.WriteLine("4. Generar factura");
            Console.WriteLine("5. Salir");
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
                    GenerateInvoice();
                    break;
                case "5":
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
            PrintLine("Ingrese el Tipo de Cliente (1 - Administrador, 0 - Cliente): ");
            string role = ReadLines();
            bool esRolValido = role == "1" || role == "0";
            if (esRolValido)
                user.ROLE = Enum.Parse<RoleUser>(role);
            else
                ShowMenu();

            UserValidator validator = new UserValidator();
            ValidationResult result = validator.Validate(user);

            // Verificar si el objeto es válido o no
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
            Console.WriteLine("\t"+title+"\t");
            Console.WriteLine(new string('=', 40));
        }
      
        public void PrintLine(string message)
        {
            Console.WriteLine(message);
        }
        public string ReadLines() => Console.ReadLine();

        private void GenerateInvoice()
        {
            Console.Clear();
            Console.Write("Ingrese el ID de la reserva: ");
            int reservationId = int.Parse(Console.ReadLine());

            try
            {
                var reservationRepository = new ReservationRepository();
                var invoiceRepository = new InvoiceRepository();
                var billingService = new BillingService(reservationRepository, invoiceRepository);

                var invoice = billingService.GenerateInvoice(reservationId);

                Console.WriteLine("\nFactura generada con éxito:");
                Console.WriteLine($"ID: {invoice.ID}, Fecha: {invoice.DateIssued}, Total: {invoice.TotalAmount}");

                Console.WriteLine("\nDetalles:");
                foreach (var detail in invoice.InvoiceDetails)
                {
                    Console.WriteLine($"Habitación: {detail.RoomID}, Precio: {detail.Price}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
            ShowMenu();
        }


    }

}
