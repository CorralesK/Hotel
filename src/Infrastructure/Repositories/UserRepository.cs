using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Infrastructure.Data;

namespace Hotel.src.Infrastructure.Repositories
{
    class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetUserByEmailAndRole(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.EMAIL == email && u.PASSWORD == password);

            if (user == null)
            {
                Console.WriteLine("❌ Usuario no encontrado. (email o contraseña incorrectos) Presione (Enter o Intro) para Intentar de Nuevo.");
            }
            else
            {
                Console.WriteLine($"✅ Usuario encontrado: {user.NAME}");
            }

            return user;
        }

    }
}
