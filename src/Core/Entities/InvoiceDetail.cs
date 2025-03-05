using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.src.Core.Entities
{
    public class InvoiceDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int InvoiceID { get; set; }
        public Invoice Invoice { get; set; }

        [Required]
        public int RoomID { get; set; }
        public Room Room { get; set; }

        [Required]
        public int ReservationID { get; set; }
        public Reservation Reservation { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public InvoiceDetail() { }

        public InvoiceDetail(int invoiceId, int roomId, int reservationId, decimal price)
        {
            InvoiceID = invoiceId;
            RoomID = roomId;
            ReservationID = reservationId;
            Price = price;
        }
    }
}
