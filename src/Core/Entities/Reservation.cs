using Hotel.src.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace Hotel.src.Core.Entities
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [ForeignKey(nameof(User))]
        public int USERID { get; set; }
        [Required]
        public DateTime STARTDATE { get; set; }
        [Required]
        public DateTime ENDDATE { get; set; }
        [Required]
        public double TOTALPRICE {get; set; }
        [Required]
        public ReservationStatus STATUS { get; set; }
        public User User { get; init; }
        // Relación muchos a muchos con la tabla de habitaciones (a través de la tabla intermedia ReservationRoom)
        public ICollection<ReservationRoom> ReservationRooms { get; set; } = new List<ReservationRoom>();
        public Reservation()
        {
            
        }
        public Reservation(int clientId, DateTime startDate, DateTime endDate, float totalPrice, ReservationStatus status)
        {
            USERID = clientId;
            STARTDATE = startDate;
            ENDDATE = endDate;
            TOTALPRICE = totalPrice;
            STATUS = status;
        }

        
        public double CalculateTotalPrice()
        {
            TOTALPRICE = 0;
            foreach (var reservationRoom in ReservationRooms)
            {
                // Calcular el número de días de la reserva
                int days = (int)(ENDDATE - STARTDATE).TotalDays;
                // Sumar el precio de cada habitación multiplicado por los días
                TOTALPRICE += reservationRoom.Room.PRICEPERNIGHT * days;
            }
            return TOTALPRICE;
        }
    }
 }
