using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Hotel.Core.Interfaces.IServices
{
    public interface IAuthService
    {
        string Authenticate(string email, string password);
    }
}
