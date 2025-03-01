using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.src.Core.Entities
{
    public class InvoiceDetail
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Invoice")]
        public int InvoiceID { get; set; }
        public Invoice Invoice { get; set; }

        [ForeignKey("Room")]
        public int RoomID { get; set; }
        public Room Room { get; set; }

        [ForeignKey("Reservation")]
        public int ReservationID { get; set; }
        public Reservation Reservation { get; set; }

        [Required]
        public float Price { get; set; }
    }
}
