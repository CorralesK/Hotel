using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.src.Application.Services;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Core.Interfaces.IServices;
using Hotel.src.Infrastructure.Data;
using Hotel.src.Infrastructure.Repositories;

namespace Hotel.src.ConsoleUI.Menu
{
    class Login
    {
        public void UserLogin()
        {
            // Instanciamos el contexto de la base de datos
            ApplicationDbContext dbContext = new ApplicationDbContext();

            // Creamos el repositorio de usuarios

            IUserRepository userRepository = new UserRepository(dbContext);

            // Creamos el servicio de JWT
            JwtService jwtService = new JwtService(); // Asegúrate de que su constructor no requiere parámetros adicionales

            // Instanciamos el servicio de autenticación con las dependencias correctas
            IAuthService authService = new AuthService(userRepository, jwtService);

            Console.WriteLine("Login");
        }

    }
}
