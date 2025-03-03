using Hotel.src.Core.Entities;

namespace Hotel.src.Core.Interfaces.IRepository
{
    public interface ICustomerRepository
    {
        User AddClient(User user);
        User GetbyId(int id);
    }
}
