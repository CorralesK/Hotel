using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;


namespace Hotel.src.HotelCore.Interfaces.IRepository
{
    interface IRoomRepository
    {
        Room Add(Room room);
        IEnumerable<Room> GetAll();
        Room GetById(int id);
        IEnumerable<Room> GetByType(RoomType type);
        IEnumerable<Room> GetByPriceRange(double minPrice, double maxPrice);
        IEnumerable<Room> GetByStatus(RoomStatus status);
        bool HasReservationsInDateRange(int roomId, DateTime startDate, DateTime endDate);
    }
}
