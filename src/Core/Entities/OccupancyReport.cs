using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Core.Entities
{
    public class OccupancyReport
    {
        public int TotalRooms { get; set; }
        public double TotalIncome { get; set; }
        public double OccupancyRate { get; set; }
        public List<ReportByType> OccupancyByType { get; set; } = new List<ReportByType>();
        public List<DailyReport> DailyOccupancy { get; set; } = new List<DailyReport>();

        public class ReportByType
        {
            public string RoomType { get; set; }
            public int ReservationsCount { get; set; }
            public int OccupiedDays { get; set; }
            public int AvailableDays { get; set; }
            public double OccupancyRateType { get; set; }
        }

        public class DailyReport
        {
            public DateTime Day { get; set; }
            public int OccupiedRooms { get; set; }
            public int TotalRooms { get; set; }
            public decimal OccupancyRateDay { get; set; }
        }
    }
}