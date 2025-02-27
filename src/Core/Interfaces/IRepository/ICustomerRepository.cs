using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.src.Core.Entities;

namespace Hotel.src.Core.Interfaces.IRepository
{
    public interface ICustomerRepository
    {
        User AddCliente(User user);
    }
}
