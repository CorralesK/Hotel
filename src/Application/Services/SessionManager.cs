﻿namespace Hotel.src.Application.Services
{
    public static class SessionManager
    {


        public static string Token { get; set; }
        public static int UserId { get; set; }
        public static string UserRole { get; private set; }


        public static void SetSession(string token)
        {
            Token = token;
            UserId = new JwtService().GetUserIdFromToken(token);
            UserRole = new JwtService().GetRoleFromToken(token);
        }

        public static void ClearSession()
        {
            Token = null;
            UserId = 0;
            UserRole = null;
        }
    }
}
