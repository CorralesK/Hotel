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

        public User AddCliente(User user)
        {

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
