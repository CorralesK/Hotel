using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using Hotel.src.Core.Interfaces.IRepository;


namespace Hotel.src.Application.Services
{
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomService"/> class.
        /// </summary>
        /// <param name="roomRepository">The repository used to interact with the room data layer.</param>
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        /// <summary>
        /// Registers a new room in the system after validating the room details.
        /// </summary>
        /// <param name="roomNumber">The unique number of the room.</param>
        /// <param name="type">The type of the room (e.g., Single, Double, Suite).</param>
        /// <param name="pricePerNight">The price per night for the room.</param>
        /// <param name="capacity">The capacity of the room (number of people it can accommodate).</param>
        /// <returns>A Room object representing the newly registered room.</returns>
        /// <exception cref="ArgumentException">Thrown when room details are invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a room with the same number already exists.</exception>
        public Room RegisterRoom(string roomNumber, RoomType type, double pricePerNight, int capacity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roomNumber))
                    throw new ArgumentException("El número de habitación es obligatorio");
                if (pricePerNight <= 0)
                    throw new ArgumentException("El precio debe ser mayor que 0");
                if (capacity <= 0)
                    throw new ArgumentException("La capacidad debe ser mayor que 0");

                // Check if the room number already exists in the repository
                var existingRoom = _roomRepository.GetByRoomNumber(roomNumber);
                if (existingRoom != null)
                    throw new InvalidOperationException("El número de habitación ya existe");

                var newRoom = new Room(roomNumber, type, pricePerNight, capacity, RoomStatus.DISPONIBLE);

                return _roomRepository.Add(newRoom);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar la habitación: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a room by its unique identifier.
        /// </summary>
        /// <param name="id">The unique ID of the room to retrieve.</param>
        /// <returns>A Room object corresponding to the specified ID.</returns>
        public Room GetRoomById(int id)
        {
            return _roomRepository.GetById(id);
        }

        /// <summary>
        /// Checks the availability of rooms for a given date range.
        /// </summary>
        /// <param name="startDate">The start date of the desired stay.</param>
        /// <param name="endDate">The end date of the desired stay.</param>
        /// <returns>An IEnumerable of rooms that are available in the specified date range.</returns>
        /// <exception cref="ArgumentException">Thrown if the end date is earlier than the start date.</exception>
        public IEnumerable<Room> CheckAvailability(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException("La fecha de fin no puede ser menor a la fecha de inicio.");
            }

            return _roomRepository.HasReservationsInDateRange(startDate, endDate);
        }

        /// <summary>
        /// Searches for rooms based on various filters like type, price range, and availability dates.
        /// </summary>
        /// <param name="type">The type of room to search for (optional).</param>
        /// <param name="minPrice">The minimum price per night for the room (optional).</param>
        /// <param name="maxPrice">The maximum price per night for the room (optional).</param>
        /// <param name="startDate">The start date for the availability check (optional).</param>
        /// <param name="endDate">The end date for the availability check (optional).</param>
        /// <returns>An IEnumerable of rooms that match the search criteria.</returns>
        public IEnumerable<Room> SearchRooms(RoomType? type = null, double? minPrice = null, double? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            IEnumerable<Room>? rooms = null;

            // If availability dates are provided, check availability for that period
            if (startDate.HasValue && endDate.HasValue)
            {
                rooms = CheckAvailability(startDate.Value, endDate.Value);
            }
            // If no filters are provided, default to checking availability for today
            else if (!type.HasValue && !minPrice.HasValue && !maxPrice.HasValue)
            {
                rooms = CheckAvailability(DateTime.Today, DateTime.Today);
            }

            // Apply filters for room type, if specified
            if (type.HasValue)
            {
                rooms = _roomRepository.GetByType(type.Value);
            }

            // Apply filters for price range, if specified
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                rooms = _roomRepository.GetByPriceRange(minPrice.Value, maxPrice.Value);
            }

            return rooms;
        }

    }
}
