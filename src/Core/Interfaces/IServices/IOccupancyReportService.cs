using Hotel.src.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Core.Interfaces.IServices
{
    public interface IOccupancyReportService
    {
        OccupancyReport GenerateOccupancyReport(DateTime startDate, DateTime endDate);
    }
}
