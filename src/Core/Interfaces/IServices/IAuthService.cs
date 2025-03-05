namespace Hotel.src.Core.Interfaces.IServices
{
    public interface IAuthService
    {
        string Authenticate(string email, string password);
    }
}
