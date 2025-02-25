using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.HotelCore.Interfaces.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Application.Services
{
    class RoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        /// <summary>
        /// Registers a new room in the system.
        /// </summary>
        /// <param name="roomNumber">The room number.</param>
        /// <param name="type">The type of the room.</param>
        /// <param name="pricePerNight">The price per night.</param>
        /// <param name="capacity">The room capacity.</param>
        /// <returns>The newly created room.</returns>
        public Room RegisterRoom(string roomNumber, RoomType type, double pricePerNight, int capacity)
        {
            if (string.IsNullOrWhiteSpace(roomNumber))
                throw new ArgumentException("El número de habitación es obligatorio");
            if (pricePerNight <= 0)
                throw new ArgumentException("El precio debe ser mayor que 0");
            if (capacity <= 0)
                throw new ArgumentException("La capacidad debe ser mayor que 0");

            var newRoom = new Room(roomNumber, type, pricePerNight, capacity, RoomStatus.DISPONIBLE);

            return _roomRepository.Add(newRoom);
        }


        public IEnumerable<Room> SearchRooms(RoomType? type, double? minPrice, double? maxPrice, DateTime? startDate = null, DateTime? endDate = null)
        {
            var rooms = _roomRepository.GetAll();

            if (type.HasValue)
            {
                rooms = rooms.Where(r => r.TYPE == type.Value);
            } else if (minPrice.HasValue && maxPrice.HasValue)
            {
                rooms = rooms.Where(r => r.PRICEPERNIGHT >= minPrice.Value && r.PRICEPERNIGHT <= maxPrice.Value);
            } else if (startDate.HasValue && endDate.HasValue)
            {
                rooms = rooms.Where(r => !_roomRepository.HasReservationsInDateRange(r.ID, startDate.Value, endDate.Value)).ToList();
            } else
            {
                rooms = rooms.Where(r => r.STATUS == RoomStatus.DISPONIBLE);
            }

            return rooms;
        }

        public IEnumerable<Room> CheckAvailability(DateTime? startDate, DateTime? endDate)
        {
            var availableRooms = _roomRepository.GetByStatus(RoomStatus.DISPONIBLE);

            if (startDate.HasValue && endDate.HasValue)
            {
                availableRooms = availableRooms.Where(r => !_roomRepository.HasReservationsInDateRange(r.ID, startDate.Value, endDate.Value)).ToList();
            }

            return availableRooms;
        }

    }
}
