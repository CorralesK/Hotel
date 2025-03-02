using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Infrastructure.Data;

namespace Hotel.src.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomRepository"/> class.
        /// </summary>
        /// <param name="context">The ApplicationDbContext instance used to interact with the database.</param>
        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new room to the database after validating that the room number is unique.
        /// </summary>
        /// <param name="room">The Room object to be added.</param>
        /// <returns>The added Room object.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the room number already exists in the database.</exception>
        /// <exception cref="Exception">Thrown when an error occurs while saving the room.</exception>
        public Room Add(Room room)
        {
            if (_context.Rooms.Any(r => r.ROOMNUMBER == room.ROOMNUMBER))
            {
                throw new InvalidOperationException("El número de habitación ya existe.");
            }

            try
            {
                _context.Rooms.Add(room);
                _context.SaveChanges();
                return room;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar la habitación.", ex);
            }
        }


        /// <summary>
        /// Retrieves all rooms from the database.
        /// </summary>
        /// <returns>A list of all Room objects.</returns>
        public IEnumerable<Room> GetAll() => _context.Rooms.ToList();


        /// <summary>
        /// Retrieves a room by its unique identifier.
        /// </summary>
        /// <param name="id">The unique ID of the room to retrieve.</param>
        /// <returns>The Room object corresponding to the specified ID, or null if not found.</returns>
        public Room GetById(int id) => _context.Rooms.Find(id);


        /// <summary>
        /// Retrieves rooms that match the specified room type.
        /// </summary>
        /// <param name="type">The type of room (e.g., Single, Double, Suite).</param>
        /// <returns>A list of Room objects that match the specified room type.</returns>
        public IEnumerable<Room> GetByType(RoomType type) => _context.Rooms.Where(r => r.TYPE == type).ToList();


        /// <summary>
        /// Retrieves rooms that are within the specified price range.
        /// </summary>
        /// <param name="minPrice">The minimum price per night for the rooms.</param>
        /// <param name="maxPrice">The maximum price per night for the rooms.</param>
        /// <returns>A list of Room objects that match the price range.</returns>
        public IEnumerable<Room> GetByPriceRange(double minPrice, double maxPrice) =>
            _context.Rooms.Where(r => r.PRICEPERNIGHT >= minPrice && r.PRICEPERNIGHT <= maxPrice).ToList();


        /// <summary>
        /// Retrieves rooms that have a specific status (e.g., available, occupied).
        /// </summary>
        /// <param name="status">The status of the room (e.g., Available, Occupied).</param>
        /// <returns>A list of Room objects that match the specified status.</returns>
        public IEnumerable<Room> GetByStatus(RoomStatus status) => _context.Rooms.Where(r => r.STATUS == status).ToList();


        /// <summary>
        /// Retrieves a room by its unique room number.
        /// </summary>
        /// <param name="roomNumber">The unique room number.</param>
        /// <returns>The Room object with the specified room number, or null if not found.</returns>
        public Room GetByRoomNumber(string roomNumber) => _context.Rooms.FirstOrDefault(r => r.ROOMNUMBER == roomNumber);


        /// <summary>
        /// Retrieves rooms that are not reserved during the specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the desired stay.</param>
        /// <param name="endDate">The end date of the desired stay.</param>
        /// <returns>A list of Room objects that are available within the specified date range.</returns>
        /// <exception cref="ArgumentException">Thrown if the end date is earlier than the start date.</exception>
        
      public IEnumerable<Room> HasReservationsInDateRange(DateTime startDate, DateTime endDate)
      {
          if (endDate < startDate)
        {
            throw new ArgumentException("La fecha de fin no puede ser menor a la fecha de inicio.");
        }

 
            var reservedRoomIds = _context.ReservationRooms
            .Join(_context.Reservations, 
                rr => rr.ReservationID, 
                r => r.ID, 
                (rr, r) => new { rr.RoomID, r.STARTDATE, r.ENDDATE, r.STATUS })
            .Where(x => x.STATUS == ReservationStatus.Confirmada &&
                    x.STARTDATE < endDate && x.ENDDATE > startDate) // Verificar el solapamiento de fechas
            .Select(x => x.RoomID) // Seleccionar solo el RoomID de las reservas confirmadas
            .Distinct()
            .ToList();

            // Retornar las habitaciones que no están reservadas en el rango de fechas
            return _context.Rooms
            .Where(r => !reservedRoomIds.Contains(r.ID)) // Filtrar habitaciones que no están en el listado de reservas
            .ToList();
        }
    }
}
