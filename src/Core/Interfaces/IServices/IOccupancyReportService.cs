using Hotel.src.Core.Entities;
using System;

namespace Hotel.src.Core.Interfaces.IServices
{
    public interface IOccupancyReportService
    {
        OccupancyReport GenerateOccupancyReport(DateTime startDate, DateTime endDate);
    }
}