using Hotel.src.Core.Entities;

namespace Hotel.src.Core.Interfaces.IRepository
{
    public interface IUserRepository
    {
        User GetUserByEmailAndRole(string email, string password);
    }
}
