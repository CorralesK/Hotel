using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Core.Entities
{
    class ReservationRoom
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
