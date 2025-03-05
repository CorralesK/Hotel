namespace Hotel.src.Core.Interfaces.IServices
{
    public interface INotificationService
    {
        bool SendCheckInNotification(string recipientEmail, string recipientName, DateTime checkInDate, string roomDetails);
    }
}
