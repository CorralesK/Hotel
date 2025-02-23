using Hotel.src.Hotel.Core.Entities;
using Microsoft.EntityFrameworkCore;
using dotenv.net;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hotel.src.Hotel.Infrastructure.Data
{
    class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Configures the database connection using the connection string stored in the DATABASE_URL environment variable.
        /// This method is used to establish the connection to PostgreSQL database using the Npgsql library.
        /// The connection string is read from an .env file using the DotEnv library.
        /// </summary>
        /// <param name="options">Object that represents the database connection configuration options.</param>

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var envVars = DotEnv.Read();
            options.UseNpgsql(envVars["DATABASE_URL"]);
        }

        /// <summary>
        /// Gets or sets the Customers DbSet which represents the collection of all Customer entities in the context.
        /// </summary>
        /// <value>
        /// A DbSet of Customer entities.
        /// </value>
        public DbSet<User> Users { get; set; }
    }
}

