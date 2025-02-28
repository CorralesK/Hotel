using Hotel.src.Core.Entities;
using Microsoft.EntityFrameworkCore;
using dotenv.net;

namespace Hotel.src.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Configures the database connection using the connection string stored in the DATABASE_URL environment variable.
        /// This method is used to establish the connection to PostgreSQL database using the Npgsql library.
        /// The connection string is read from an .env file using the DotEnv library.
        /// </summary>
        /// <param name="options">Object that represents the database connection configuration options.</param>

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
          

            DotNetEnv.Env.Load(@"C:\Users\USER\source\repos\Hotel\.env");
            string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

            // Verificar si la cadena de conexión está vacía o nula
            if (string.IsNullOrEmpty(connectionString))
            {
                var envVars = DotEnv.Read();
                connectionString = envVars["DATABASE_URL"];
            }

            options.UseNpgsql(connectionString);
        }

        /// <summary>
        /// Gets or sets the Users DbSet which represents the collection of all Customer entities in the context.
        /// </summary>
        /// <value>
        /// A DbSet of User entities.
        /// </value>
        public virtual DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the Rooms DbSet which represents the collection of all Room entities in the context.
        /// </summary>
        /// <value>
        /// A DbSet of Room entities.
        /// </value>
        
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<ReservationRoom> ReservationRooms { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // Configurar la clave primaria compuesta para ReservationRoom
            modelBuilder.Entity<ReservationRoom>()
                .HasKey(rr => new { rr.ReservationID, rr.RoomID });

            // Configurar la relación entre Reservation y ReservationRoom
            modelBuilder.Entity<ReservationRoom>()
                .HasOne(rr => rr.Reservation)
                .WithMany(r => r.ReservationRooms)
                .HasForeignKey(rr => rr.ReservationID);

            // Configurar la relación entre Room y ReservationRoom
            modelBuilder.Entity<ReservationRoom>()
                .HasOne(rr => rr.Room)
                .WithMany(r => r.ReservationRooms)
                .HasForeignKey(rr => rr.RoomID);

        }
       
    }
}

