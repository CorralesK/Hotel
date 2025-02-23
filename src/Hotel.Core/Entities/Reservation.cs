using Hotel.src.Hotel.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Hotel.Core.Entities
{
    class Reservation
    {
        [Key]
        public long ID { get; set; }
        [ForeignKey("Customer")]
        [Required]
        public int CLIENTID { get; set; }
        [Required]
        public DateTime STARTDATE { get; set; }
        [Required]
        public DateTime ENDDATE { get; set; }
        [Required]
        public float TOTALPRICE {get; set; }
        [Required]
        public ReservationStatus STATUS { get; set; }

        // Relación muchos a muchos con la tabla de habitaciones (a través de la tabla intermedia ReservationRoom)
        public ICollection<ReservationRoom> ReservationRooms { get; set; } = new List<ReservationRoom>();
        public Reservation()
        {
            
        }
        public Reservation(int id, int clientId, DateTime startDate, DateTime endDate, float totalPrice, ReservationStatus status)
        {
            ID = id;
            CLIENTID = clientId;
            STARTDATE = startDate;
            ENDDATE = endDate;
            TOTALPRICE = totalPrice;
            STATUS = status;
        }
        public void CalculateTotalPrice()
        {
            // Logica para calcular el total de todas las reservas
        }
    }
 }
