using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.src.Core.Entities;

namespace Hotel.src.Core.Entities
{
    public class InvoiceDetail
    {
        [Key]
        public long ID { get; set; }

        [ForeignKey("Invoice")]
        public long InvoiceID { get; set; }
        public Invoice Invoice { get; set; }

        [ForeignKey("Room")]
        public int RoomID { get; set; }
        public Room Room { get; set; }
        
        [ForeignKey("Reservation")]
        public long ReservationID { get; set; }
        public Reservation Reservation { get; set; }

        [Required]
        public float Price { get; set; }
    }
}
