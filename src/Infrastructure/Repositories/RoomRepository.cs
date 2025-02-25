using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using Hotel.src.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.src.Core.Interfaces.IRepository;

namespace Hotel.src.Infrastructure.Repositories
{
    class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Room Add(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return room;
        }

        public IEnumerable<Room> GetAll() => _context.Rooms.ToList();

        public Room GetById(int id) => _context.Rooms.Find(id);

        public IEnumerable<Room> GetByType(RoomType type) => _context.Rooms.Where(r => r.TYPE == type).ToList();

        public IEnumerable<Room> GetByPriceRange(double minPrice, double maxPrice) =>
            _context.Rooms.Where(r => r.PRICEPERNIGHT >= minPrice && r.PRICEPERNIGHT <= maxPrice).ToList();
        public IEnumerable<Room> GetByStatus(RoomStatus status) => _context.Rooms.Where(r => r.STATUS == status).ToList();


        /// <summary>
        /// Esto tengo que cambiarlo para la tabla intermedia
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public bool HasReservationsInDateRange(int roomId, DateTime startDate, DateTime endDate)
        {
            //var overlappingReservations = _context.Reservations
            //    .Where(r => r.RoomId == roomId &&
            //                r.Status == ReservationStatus.Confirmada &&
            //                ((r.StartDate < endDate && r.EndDate > startDate)))
            //    .Any();
            //return overlappingReservations;
            return false;
        }


    }
}
