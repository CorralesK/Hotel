using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Core.Interfaces.IServices
{
    public interface INotificationService
    {
        void SendCheckInNotification(string recipientEmail, string recipientName, DateTime checkInDate, string roomDetails);
    }
}
