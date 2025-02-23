using Hotel.src.Hotel.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Hotel.Core.Entities
{
    class Reservation
    {
        public int ID { get; set; }
        public int CLIENTID { get; set; }
        public DateTime STARTDATE { get; set; }
        public DateTime ENDDATE { get; set; }
        public List<Room> Rooms { get; set; }
        public float TOTALPRICE {get; set; }
        public ReservationStatus STATUS { get; set; }
        public Reservation()
        {
            Rooms = new List<Room>();
        }
        public Reservation(int id, int clientId, DateTime startDate, DateTime endDate, List<Room> rooms, float totalPrice, ReservationStatus status)
        {
            ID = id;
            CLIENTID = clientId;
            STARTDATE = startDate;
            ENDDATE = endDate;
            Rooms = rooms;
            TOTALPRICE = totalPrice;
            STATUS = status;
        }
        public void CalculateTotalPrice()
        {
            // Logica para calcular el total de todas las reservas
        }
    }
 }
