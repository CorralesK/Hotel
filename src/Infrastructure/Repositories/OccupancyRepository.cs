using dotenv.net;
using Hotel.src.Core.Interfaces.IRepositories;
using Npgsql;
using System;
using System.Collections.Generic;

namespace Hotel.src.Infrastructure.Repositories
{
    public class OccupancyRepository : IOccupancyRepository
    {
        private readonly string _connectionString;

        public OccupancyRepository()
        {
            DotEnv.Load();
            _connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
        }

        public int GetTotalRooms()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM public.\"Rooms\"", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public double GetTotalIncome(DateTime start, DateTime end)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            const string sql = @"
            SELECT COALESCE(SUM(""TOTALPRICE""), 0)
            FROM public.""Reservations""
            WHERE ""STATUS"" IN (1, 3)
              AND ""STARTDATE""::DATE >= @startDate::DATE
              AND ""ENDDATE""::DATE <= @endDate::DATE";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("startDate", NpgsqlTypes.NpgsqlDbType.Date, start);
            cmd.Parameters.AddWithValue("endDate", NpgsqlTypes.NpgsqlDbType.Date, end);
            return Convert.ToDouble(cmd.ExecuteScalar());
        }

        public int GetTotalOccupiedDays(DateTime start, DateTime end)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
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

        public List<(string RoomType, int ReservationsCount, int OccupiedDays, int TotalRoomsType)> GetOccupancyByType(DateTime start, DateTime end)
        {
            var list = new List<(string RoomType, int ReservationsCount, int OccupiedDays, int TotalRoomsType)>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
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

                list.Add((roomType, reservationsCount, occupiedDays, totalRoomsType));
            }

            return list;
        }

        public List<(DateTime Day, int OccupiedRooms)> GetDailyOccupancy(DateTime start, DateTime end)
        {
            var dailyList = new List<(DateTime Day, int OccupiedRooms)>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
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

                dailyList.Add((day, occupied));
            }

            return dailyList;
        }
    }
}