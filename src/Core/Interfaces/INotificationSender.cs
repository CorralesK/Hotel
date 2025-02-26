using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.src.Core.Interfaces
{
    public interface INotificationSender
    {
        bool Send(string subject, string message, string recipient);
    }
}
