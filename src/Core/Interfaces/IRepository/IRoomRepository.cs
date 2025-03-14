﻿using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;


namespace Hotel.src.Core.Interfaces.IRepository
{
    public interface IRoomRepository
    {
        Room Add(Room room);
        IEnumerable<Room> GetAll();
        Room GetById(int id);
        IEnumerable<Room> GetByType(RoomType type);
        IEnumerable<Room> GetByPriceRange(double minPrice, double maxPrice);
        IEnumerable<Room> GetByStatus(RoomStatus status);
        Room GetByRoomNumber(string roomNumber);
        IEnumerable<Room> HasReservationsInDateRange(DateTime startDate, DateTime endDate);
    }
}
