using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Hotel.Core.Entities
{
    class ReservationRoom
    {
        // Hola soy Kim dejo los comentarios en espa;ol para que los entiendas ya que es una llave compuesta.

        [Key]
        [Column(Order = 1)]  // Indica que esta es la primera parte de la clave compuesta
        public int ReservationID { get; set; }

        [Key]
        [Column(Order = 2)]  // Indica que esta es la segunda parte de la clave compuesta
        public int RoomID { get; set; }

        [Required]
        public Reservation Reservation { get; set; }

        [Required]
        public Room Room { get; set; }

        public ReservationRoom() { }

    }
}
