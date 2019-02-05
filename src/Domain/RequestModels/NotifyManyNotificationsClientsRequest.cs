using Domain.AbstractionModels;
using System.Collections.Generic;

namespace Domain.RequestModels
{
    public class NotifyManyNotificationsClientsRequest : UserContextRequest
    {
        public NotifyManyNotificationsClientsRequest()
        {
            NotificationsClients = new List<NotifyClientAbstraction>();
        }

        public string Owner { get; set; }
        public int ApplicationId { get; set; }
        public IList<NotifyClientAbstraction> NotificationsClients { get; set; }
    }
}
