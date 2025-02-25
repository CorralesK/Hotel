using Moq;
using System;
using System.Linq;
using NUnit.Framework;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hotel.src.Application.Services;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;

namespace Hotel.tests.Hotel.Application.Tests
{
    [TestFixture]
    class AuthServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<JwtService> _jwtServiceMock;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtServiceMock = new Mock<JwtService>();
            _authService = new AuthService(_userRepositoryMock.Object, _jwtServiceMock.Object);
        }
        [Test]

        public void Authenticate_ShouldReturnToken_WhenUserExists()
        {
            // Arrange 🔹 Simulamos un usuario en la BD
            var testUser = new User { EMAIL = "admin@example.com", PASSWORD = "123", ROLE = RoleUser.Admin };
            _userRepositoryMock
                .Setup(repo => repo.GetUserByEmailAndRole("admin@example.com", "123"))
                .Returns(testUser);

            _jwtServiceMock
                .Setup(jwt => jwt.GenerateToken(testUser))
                .Returns("mocked_jwt_token");

            // Act 🔹 Llamamos al método que queremos probar
            var token = _authService.Authenticate("admin@example.com", "123");

            // Assert 🔹 Verificamos que el token no sea null
            Assert.That(token, Is.EqualTo("mocked_jwt_token"));

        }
    }
}
