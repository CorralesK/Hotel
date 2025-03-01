﻿namespace Hotel.src.Core.Interfaces.IServices
{
    public interface INotificationService
    {
        void SendCheckInNotification(string recipientEmail, string recipientName, DateTime checkInDate, string roomDetails);
    }
}
