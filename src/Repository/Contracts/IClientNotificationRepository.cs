using Domain.Models;
using Domain.ResponseModels;
using System.Collections.Generic;

namespace Repository.Contracts
{
    public interface IClientNotificationRepository : IRepository<ClientNotification>
    {
        int CountNotificationsUnchecked(string userId, int applicationId);
        ClientNotification Find(int id);
        IEnumerable<ClientNotificationsListResponse> List(string userId, int applicationId);
        IEnumerable<ClientNotificationsListResponse> ListNotVisualized(string userId, int applicationId);
        IEnumerable<ClientNotificationsListResponse> ListVisualized(string userId, int applicationId);
        IEnumerable<ClientNotification> GetUnChecked(string userId, int applicationId);
    }
}
