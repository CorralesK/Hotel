
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hotel.src.Core.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Hotel.src.Application.Services
{
    public class JwtService
    {
        private const string SecretKey = "mi-clave-super-secreta-de-mas-de-32-caracteres!"; // 📌 Secret of token

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Email, user.EMAIL),
                new Claim(ClaimTypes.Role, user.ROLE.ToString()) // 📌 The role is saved in the token
                }),
                Expires = DateTime.UtcNow.AddHours(1), // ⏳ Token expires in 1 hour
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // ✅ Extrae el rol desde un JWT
        public string GetRoleFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, // 📌 Validate the key
                IssuerSigningKey = new SymmetricSecurityKey(key), // 📌 // Sets the symmetric signing key used to validate the token signature.
                ValidateIssuer = false,
                ValidateAudience = false
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal.FindFirst(ClaimTypes.Role)?.Value; // 📌 return the rol
        }
        public int GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            // 📌 Find the claim that contains the user ID (nameid)
            var userIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "nameid"); //'nameid'


            if (userIdClaim == null)
            {
                Console.WriteLine(userIdClaim);
                throw new Exception("El token no contiene el ID del usuario.");
            }

            int userId = int.Parse(userIdClaim.Value);

            return userId;
        }
    }
}
