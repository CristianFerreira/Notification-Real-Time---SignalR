using Microsoft.AspNet.SignalR;
using System.Collections.Generic;

namespace Service.Services
{
    public static class NotificationClientService
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ConnectionHubService>();

        public static void NotifyClients(IList<string> connectionsIds)
             => hubContext.Clients.Clients(connectionsIds).MessageAdded("");
    }
}
