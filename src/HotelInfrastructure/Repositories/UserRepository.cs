using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.src.HotelCore.Entities;
using Hotel.src.HotelCore.Enums;
using Hotel.src.HotelCore.Interfaces.IRepository;
using Hotel.src.HotelInfrastructure.Data;

namespace Hotel.src.HotelInfrastructure.Repositories
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
