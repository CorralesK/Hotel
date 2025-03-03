using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepositories;
using Hotel.src.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;

namespace Hotel.src.Application.Services
{
    public class OccupancyReportService : IOccupancyReportService
    {
        private readonly IOccupancyRepository _occupancyRepository;

        public OccupancyReportService(IOccupancyRepository occupancyRepository)
        {
            _occupancyRepository = occupancyRepository;
        }

        public OccupancyReport GenerateOccupancyReport(DateTime startDate, DateTime endDate)
        {
            var report = new OccupancyReport();
            int daysInPeriod = (endDate - startDate).Days + 1;

            // 1. Total de habitaciones
            report.TotalRooms = _occupancyRepository.GetTotalRooms();

            // 2. Ingresos totales (solo reservas pagadas, por ejemplo status 1 y 3)
            report.TotalIncome = _occupancyRepository.GetTotalIncome(startDate, endDate);

            // 3. Tasa de ocupación general 
            int totalOccupiedDays = _occupancyRepository.GetTotalOccupiedDays(startDate, endDate);
            report.OccupancyRate = CalculateOccupancyRate(totalOccupiedDays, report.TotalRooms, daysInPeriod);

            // 4. Ocupación por tipo de habitación
            report.OccupancyByType = GetOccupancyByType(startDate, endDate, daysInPeriod);

            // 5. Ocupación diaria
            report.DailyOccupancy = GetDailyOccupancy(startDate, endDate, report.TotalRooms);

            return report;
        }

        private double CalculateOccupancyRate(int occupiedDays, int totalRooms, int daysInPeriod)
        {
            int totalPossibleDays = totalRooms * daysInPeriod;
            return totalPossibleDays > 0 ? (double)occupiedDays / totalPossibleDays : 0;
        }

        private List<OccupancyReport.ReportByType> GetOccupancyByType(DateTime start, DateTime end, int daysInPeriod)
        {
            var list = new List<OccupancyReport.ReportByType>();
            var roomTypeData = _occupancyRepository.GetOccupancyByType(start, end);

            foreach (var (roomType, reservationsCount, occupiedDays, totalRoomsType) in roomTypeData)
            {
                var reportByType = new OccupancyReport.ReportByType
                {
                    RoomType = roomType,
                    ReservationsCount = reservationsCount,
                    OccupiedDays = occupiedDays,
                    AvailableDays = totalRoomsType * daysInPeriod,
                    OccupancyRateType = CalculateOccupancyRate(occupiedDays, totalRoomsType, daysInPeriod)
                };
                list.Add(reportByType);
            }

            return list;
        }

        private List<OccupancyReport.DailyReport> GetDailyOccupancy(DateTime start, DateTime end, int totalRooms)
        {
            var dailyList = new List<OccupancyReport.DailyReport>();
            var dailyData = _occupancyRepository.GetDailyOccupancy(start, end);

            foreach (var (day, occupied) in dailyData)
            {
                dailyList.Add(new OccupancyReport.DailyReport
                {
                    Day = day,
                    OccupiedRooms = occupied,
                    TotalRooms = totalRooms,
                    OccupancyRateDay = totalRooms > 0 ? (decimal)occupied / totalRooms : 0
                });
            }

            return dailyList;
        }
    }
}