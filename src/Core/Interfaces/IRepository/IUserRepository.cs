using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;

namespace Hotel.src.Core.Interfaces.IRepository
{
    public interface IUserRepository
    {
        User GetUserByEmailAndRole(string email, string password);
        User GetById(int id);
    }
}
