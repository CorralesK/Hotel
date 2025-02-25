using Hotel.src.Hotel.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Hotel.ConsoleUI
{
    class ReservationMenu
    {
        private readonly ReservationService _reservationService;

        public ReservationMenu(ReservationService reservationService)
        {
            _reservationService = reservationService;
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
                    _reservationService.RegisterReservation();
                    break;
                case "2":
                    _reservationService.CancelRoomInReservation();
                    break;
                case "3":
                    _reservationService.GetReservationsByClientId();
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
    }
}
