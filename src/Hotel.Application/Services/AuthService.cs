using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.src.Hotel.Core.Interfaces.IRepository;
using Hotel.src.Hotel.Core.Interfaces.IServices;

namespace Hotel.src.Hotel.Application.Services
{
    class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public AuthService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public string Authenticate(string email, string password)
        {
            var user = _userRepository.GetUserByEmailAndRole(email, password);
            if (user == null || user.PASSWORD != password)
            {
                Console.WriteLine("User not found or incorrect password");
                return null; // ❌ User not found or incorrect password
            }

            return _jwtService.GenerateToken(user); // ✅ return JWT
        }
    }
}

