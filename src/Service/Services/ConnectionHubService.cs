using Domain.RequestModels;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Service.Contracts;
using System;
using System.Threading.Tasks;

namespace Service.Services
{
    [HubName("notification")]
    public class ConnectionHubService : Hub, IConnectionHubService
    {
        public override Task OnConnected()
        {
            var request = new ClientConnectedRequest(Context.ConnectionId,
                                                     Context.QueryString["userId"],
                                                     int.Parse(Context.QueryString["applicationId"]));

            MemoryCacheService.Add(request);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            MemoryCacheService.Remove(Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        protected ClientService GetService() => Activator.CreateInstance<ClientService>();
    }
}
