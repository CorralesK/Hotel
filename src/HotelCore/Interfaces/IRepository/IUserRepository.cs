using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.src.HotelCore.Entities;
using Hotel.src.HotelCore.Enums;

namespace Hotel.src.HotelCore.Interfaces.IRepository
{
    internal interface IUserRepository
    {
        User GetUserByEmailAndRole(string email, string password);
    }
}
