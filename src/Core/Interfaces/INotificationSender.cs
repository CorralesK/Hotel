namespace Hotel.src.Core.Interfaces
{
    public interface INotificationSender
    {
        bool Send(string subject, string message, string recipient);
    }
}
