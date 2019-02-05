using Domain.Models;
using Domain.RequestModels;
using Domain.ResponseModels;
using System.Collections.Generic;

namespace Service.Contracts
{
    public interface IClientNotificationService : IGenericService<ClientNotification>
    {
        void NotifyClients(NotifyClientRequest request);
        void NotifyNotificationsClients(NotifyManyNotificationsClientsRequest request);
        void Checked(ClientNotificationRequest request, string userContextId);
        void Visualized(int id, string userContextId);
        void Disable(int id, string userContextId);
        int CountNotificationsUnchecked(ClientNotificationRequest request);
        IEnumerable<ClientNotificationsListResponse> List(ClientNotificationRequest request);
        IEnumerable<ClientNotificationsListResponse> ListNotVisualized(ClientNotificationRequest request);
        IEnumerable<ClientNotificationsListResponse> ListVisualized(ClientNotificationRequest request);
    }
}
