using Domain.RequestModels;
using Repository.Repositories;
using System.Data.Entity;

namespace Service.Services.Transactions
{
    public class ClientNotificationCheckTransaction
    {
        private ClientNotificationRepository _clientNotificationRepository;

        public void InstanceRepositories(DbContext context, DbContextTransaction transaction)
        {
            _clientNotificationRepository = new ClientNotificationRepository(context, transaction);
        }

        public void ExecuteClientNotificationCheckTransaction(ClientNotificationRequest request, string userContextId)
        {
            var listUnChecked = _clientNotificationRepository.GetUnChecked(request.UserId, request.ApplicationId);
            foreach (var clientNotification in listUnChecked)
            {
                clientNotification.Check();
                clientNotification.PutAudit(userContextId);

                _clientNotificationRepository.Update(clientNotification);
            }
        }

    }
}
