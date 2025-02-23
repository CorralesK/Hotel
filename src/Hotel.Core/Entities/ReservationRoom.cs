using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Hotel.Core.Entities
{
    class ReservationRoom
    {
        public int ReservationID { get; set; }
        public Reservation Reservation { get; set; }

        public int RoomID { get; set; }
        public Room Room { get; set; }

    }
}
