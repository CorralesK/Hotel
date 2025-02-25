using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Hotel.src.HotelCore.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hotel.src.HotelCore.Entities
{
    [Index(nameof(ROOMNUMBER), IsUnique = true)] // Unique_key
    class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string ROOMNUMBER { get; set; }

        [Required]
        public RoomType TYPE { get; set; }

        [Required]
        public double PRICEPERNIGHT { get; set; }

        [Required]
        public int CAPACITY { get; set; }

        [Required]
        public RoomStatus STATUS { get; set; }


        /*Soy keisy, en las migraciones no se puede usar la lista da error, entonces
         * por eso se usa una entidad intermedia, para que con fk se pueda hacer la relación
         * de muchos a muchos ya que una reserva puede estar en muchas habitaciones, y muchas habitaciones
         * pueden estar en una reserva, entonces por eso se usa el ICollection.
         */
        public ICollection<ReservationRoom> ReservationRooms { get; set; } = new List<ReservationRoom>();


        public Room() { }

        public Room(string roomNumber, RoomType type, double pricePerNight, int capacity, RoomStatus status)
        {
            ROOMNUMBER = roomNumber;
            TYPE = type;
            PRICEPERNIGHT = pricePerNight;
            CAPACITY = capacity;
            STATUS = status;
        }
    }
}
