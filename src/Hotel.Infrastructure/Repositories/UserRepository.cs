using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.src.Hotel.Core.Entities;
using Hotel.src.Hotel.Core.Enums;
using Hotel.src.Hotel.Core.Interfaces.IRepository;
using Hotel.src.Hotel.Infrastructure.Data;

namespace Hotel.src.Hotel.Infrastructure.Repositories
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
            return _context.Users.FirstOrDefault(u => u.EMAIL == email && u.PASSWORD == password);
        }
    }
}
