using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Infrastructure.Data;

namespace Hotel.src.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User AddClient(User user)
        {
            if (_dbContext.Users.Any(u => u.EMAIL == user.EMAIL))
            {
                Console.WriteLine("❌ Error: El correo electrónico ya está registrado.");
                Console.WriteLine("⚠️ Presione (Enter o Intro) para Intentar de Nuevo.");
                return null; // Evita que el programa continúe agregando el usuario
            }

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public User GetbyId(int id)
        {
            return _dbContext.Users.Find(id);
        }
    }
}
