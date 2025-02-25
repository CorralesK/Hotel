using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.src.Hotel.Core.Entities;
using Hotel.src.Hotel.Core.Enums;

namespace Hotel.src.Hotel.Core.Interfaces.IRepository
{
    internal interface IUserRepository
    {
        User GetUserByEmailAndRole(string email, string password);
    }
}
