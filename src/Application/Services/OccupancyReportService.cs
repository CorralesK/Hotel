using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IServices;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Application.Services
{

    public class OccupancyReportService : IOccupancyReportService
    {
        // Connection string hardcodeado
        private const string ConnectionString = "Host=localhost;Database=Hotel;UserName=postgres;Password=123;";

        public OccupancyReport GenerateOccupancyReport(DateTime startDate, DateTime endDate)
        {
            var report = new OccupancyReport();
            int daysInPeriod = (endDate - startDate).Days + 1;

            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();
            // 1. Total de habitaciones
            report.TotalRooms = GetTotalRooms(conn);
            // 2. Ingresos totales (solo reservas pagadas, por ejemplo status 1 y 3)
            report.TotalIncome = CalculateTotalIncome(conn, startDate, endDate);
            // 3. Tasa de ocupación general (excluyendo reservas canceladas, es decir, status distinto de 0)
            int totalOccupiedDays = CalculateTotalOccupiedDays(conn, startDate, endDate);
            report.OccupancyRate = CalculateOccupancyRate(totalOccupiedDays, report.TotalRooms, daysInPeriod);
            // 4. Ocupación por tipo de habitación
           report.OccupancyByType = GetOccupancyByType(conn, startDate, endDate, daysInPeriod);
            // 5. Ocupación diaria
            report.DailyOccupancy = GetDailyOccupancy(conn, startDate, endDate, report.TotalRooms);
            return report;
        }

        private int GetTotalRooms(NpgsqlConnection conn)
        {
            using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM public.\"Rooms\"", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private decimal CalculateTotalIncome(NpgsqlConnection conn, DateTime start, DateTime end)
        {
            const string sql = @"
            SELECT COALESCE(SUM(""TOTALPRICE""), 0)
            FROM public.""Reservations""
            WHERE ""STATUS"" IN (1, 3)
              AND ""STARTDATE""::DATE >= @startDate::DATE
              AND ""ENDDATE""::DATE <= @endDate::DATE";


            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("startDate", NpgsqlTypes.NpgsqlDbType.Date, start);
            cmd.Parameters.AddWithValue("endDate", NpgsqlTypes.NpgsqlDbType.Date, end);
            return Convert.ToDecimal(cmd.ExecuteScalar());
        }

        private int CalculateTotalOccupiedDays(NpgsqlConnection conn, DateTime start, DateTime end)
        {
            const string sql = @"
            SELECT COALESCE(SUM(
                (LEAST(r.""ENDDATE""::DATE, @endDate::DATE) - GREATEST(r.""STARTDATE""::DATE, @startDate::DATE))
            ), 0)
            FROM public.""Reservations"" r
            JOIN public.""ReservationRooms"" rr ON r.""ID"" = rr.""ReservationID""
            WHERE r.""STATUS"" <> 0
              AND r.""STARTDATE""::DATE <= @endDate::DATE
              AND r.""ENDDATE""::DATE >= @startDate::DATE";


            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("startDate", NpgsqlTypes.NpgsqlDbType.Date, start);
            cmd.Parameters.AddWithValue("endDate", NpgsqlTypes.NpgsqlDbType.Date, end);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private decimal CalculateOccupancyRate(int occupiedDays, int totalRooms, int daysInPeriod)
        {
            int totalPossibleDays = totalRooms * daysInPeriod;
            return totalPossibleDays > 0 ? (decimal)occupiedDays / totalPossibleDays : 0;
        }

        private List<OccupancyReport.ReportByType> GetOccupancyByType(NpgsqlConnection conn, DateTime start, DateTime end, int daysInPeriod)
        {
            var list = new List<OccupancyReport.ReportByType>();
            const string sql = @"
            SELECT r.""TYPE"" AS RoomType,
                   COUNT(*) AS ReservationsCount,
                   COALESCE(SUM(LEAST(res.""ENDDATE""::DATE, @endDate::DATE) - GREATEST(res.""STARTDATE""::DATE, @startDate::DATE)), 0) AS OccupiedDays,
                   (SELECT COUNT(*) FROM public.""Rooms"" WHERE ""TYPE"" = r.""TYPE"") AS TotalRooms
            FROM public.""ReservationRooms"" rr
            JOIN public.""Reservations"" res ON rr.""ReservationID"" = res.""ID""
            JOIN public.""Rooms"" r ON rr.""RoomID"" = r.""ID""
            WHERE res.""STATUS"" <> 0
              AND res.""STARTDATE""::DATE <= @endDate::DATE
              AND res.""ENDDATE""::DATE >= @startDate::DATE
            GROUP BY r.""TYPE""";


            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("startDate", NpgsqlTypes.NpgsqlDbType.Date, start);
            cmd.Parameters.AddWithValue("endDate", NpgsqlTypes.NpgsqlDbType.Date, end);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int roomTypeValue = reader.GetInt32(0);
                string roomType = roomTypeValue switch
                {
                    0 => "SIMPLE",
                    1 => "DOBLE",
                    2 => "SUITE"
                };

                int reservationsCount = reader.GetInt32(1);
                int occupiedDays = reader.GetInt32(2);
                int totalRoomsType = reader.GetInt32(3);

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

        private List<OccupancyReport.DailyReport> GetDailyOccupancy(NpgsqlConnection conn, DateTime start, DateTime end, int totalRooms)
        {
            var dailyList = new List<OccupancyReport.DailyReport>();
            const string sql = @"
                WITH date_series AS (
                    SELECT GENERATE_SERIES(@startDate::DATE, @endDate::DATE, '1 day') AS DATE
                )
                SELECT 
                    ds.DATE, 
                    COALESCE(COUNT(rr.""ReservationID""), 0) AS COUNT
                FROM date_series ds
                LEFT JOIN ""Reservations"" res
                    ON res.""STARTDATE""::DATE <= ds.DATE 
                    AND res.""ENDDATE""::DATE >= ds.DATE
                LEFT JOIN ""ReservationRooms"" rr 
                    ON res.""ID"" = rr.""ReservationID""
                GROUP BY ds.DATE
                ORDER BY ds.DATE;
                ";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("startDate", NpgsqlTypes.NpgsqlDbType.Date, start);
            cmd.Parameters.AddWithValue("endDate", NpgsqlTypes.NpgsqlDbType.Date, end);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DateTime day = reader.GetDateTime(0);
                int occupied = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);

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
