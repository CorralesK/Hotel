using System.ComponentModel.DataAnnotations;

namespace Hotel.src.Core.Entities
{
    public class ReservationRoom
    {
        public int ReservationID { get; set; }
        public int RoomID { get; set; }

        [Required]
        public Reservation Reservation { get; set; }

        [Required]
        public Room Room { get; set; }

        public ReservationRoom() { }
    }
}
