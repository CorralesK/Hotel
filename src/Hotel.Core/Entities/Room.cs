using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hotel.src.Hotel.Core.Enums;

namespace Hotel.src.Hotel.Core.Entities
{
    class Room
    {
        private int ID { get; }
        private RoomType TYPE { get; set; }
        private int ROOMNUMBER { get; set; }
        private float PRICE { get; set; }
        private RoomStatus STATUS { get; set; }


        /*Soy keisy, en las migraciones no se puede usar la lista da error, entonces
         * por eso se usa una entidad intermedia, para que con fk se pueda hacer la relación
         * de muchos a muchos ya que una reserva puede estar en muchas habitaciones, y muchas habitaciones
         * pueden estar en una reserva, entonces por eso se usa el ICollection.
         */
        public ICollection<ReservationRoom> ReservationRooms { get; set; } = new List<ReservationRoom>();
    }
}
