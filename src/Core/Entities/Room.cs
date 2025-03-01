using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hotel.src.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hotel.src.Core.Entities
{
    /// <summary>
    /// Represents a Room entity in the hotel database.
    /// This class defines the properties and relationships related to hotel rooms.
    /// </summary>
    /// 

    // Unique constraint on the ROOMNUMBER column
    [Index(nameof(ROOMNUMBER), IsUnique = true)]
    public class Room
    {
        /// <summary>
        /// Gets or sets the unique identifier for the room.
        /// This is the primary key for the Room entity.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the room number.
        /// This value must be unique and is used for identifying a room.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the collection of ReservationRoom entities associated with this room.
        /// This represents the many-to-many relationship between rooms and reservations.
        /// </summary>
        public ICollection<ReservationRoom> ReservationRooms { get; set; } = new List<ReservationRoom>();


        public Room() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class with specified parameters.
        /// </summary>
        /// <param name="roomNumber">The unique room number.</param>
        /// <param name="type">The type of the room (e.g., Single, Double, Suite).</param>
        /// <param name="pricePerNight">The price per night for the room.</param>
        /// <param name="capacity">The maximum capacity of the room (number of guests it can accommodate).</param>
        /// <param name="status">The current status of the room (e.g., Available, Occupied).</param>
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
