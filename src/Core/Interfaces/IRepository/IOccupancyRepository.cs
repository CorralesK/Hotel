using Hotel.src.Core.Entities;
using System;
using System.Collections.Generic;

namespace Hotel.src.Core.Interfaces.IRepositories
{
    public interface IOccupancyRepository
    {
        int GetTotalRooms();
        double GetTotalIncome(DateTime start, DateTime end);
        int GetTotalOccupiedDays(DateTime start, DateTime end);
        List<(string RoomType, int ReservationsCount, int OccupiedDays, int TotalRoomsType)> GetOccupancyByType(DateTime start, DateTime end);
        List<(DateTime Day, int OccupiedRooms)> GetDailyOccupancy(DateTime start, DateTime end);
    }
}