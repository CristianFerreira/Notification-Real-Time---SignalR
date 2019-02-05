using Domain.Models;
using Domain.RequestModels;
using Domain.ResponseModels;
using Microsoft.AspNet.SignalR;
using Repository.Contracts;
using Repository.Repositories;
using Service.Contracts;
using Service.Helpers;
using Service.Services.Transactions;
using System;
using System.Collections.Generic;

namespace Service.Services
{
    public class ClientNotificationService : GenericService<ClientNotification, ClientNotificationRepository>, IClientNotificationService
    {
        private readonly IClientNotificationRepository _clientNotificationRepository;

        public ClientNotificationService(IClientNotificationRepository clientNotificationRepository)
        {            
            _clientNotificationRepository = clientNotificationRepository;
        }

        public void NotifyClients(NotifyClientRequest request)
        {
            var transaction = new NotifyClientsTransaction();
            transactionHelper.ExecuteComplexTransactionalOperation(()
                => transaction.ExecuteNotifyClientsTransaction(request), transaction.InstanceRepositories); 
        }

        public void NotifyNotificationsClients(NotifyManyNotificationsClientsRequest request)
        {
            var transaction = new NotifyNotificationsClientsTransaction();
            transactionHelper.ExecuteComplexTransactionalOperation(()
                => transaction.ExecuteNotifyNotificationsClientsTransaction(request), transaction.InstanceRepositories);
        }

        public void Checked(ClientNotificationRequest request, string userContextId)
        {
            var transaction = new ClientNotificationCheckTransaction();
            transactionHelper.ExecuteComplexTransactionalOperation(()
                => transaction.ExecuteClientNotificationCheckTransaction(request, userContextId), transaction.InstanceRepositories);
        }

        public void Visualized(int id, string userContextId)
        {
            var clientNotification = _clientNotificationRepository.Find(id);
            if (clientNotification == null)
                throw new ArgumentException("Notificação do cliente não foi encontrada!");

            clientNotification.Visualize();
            clientNotification.PutAudit(userContextId);

            _clientNotificationRepository.Update(clientNotification);
        }

        public void Disable(int id, string userContextId)
        {
           var clientNotification = _clientNotificationRepository.Find(id);
            if (clientNotification == null)
                throw new ArgumentException("Notificação do cliente não foi encontrada!");

            clientNotification.Disable();
            clientNotification.PutAudit(userContextId);

           _clientNotificationRepository.Update(clientNotification);
        }

        public int CountNotificationsUnchecked(ClientNotificationRequest request)
        => _clientNotificationRepository.CountNotificationsUnchecked(request.UserId, request.ApplicationId);

        public IEnumerable<ClientNotificationsListResponse> List(ClientNotificationRequest request)
        => _clientNotificationRepository.List(request.UserId, request.ApplicationId);

        public IEnumerable<ClientNotificationsListResponse> ListNotVisualized(ClientNotificationRequest request)
        => _clientNotificationRepository.ListNotVisualized(request.UserId, request.ApplicationId);

        public IEnumerable<ClientNotificationsListResponse> ListVisualized(ClientNotificationRequest request)
        => _clientNotificationRepository.ListVisualized(request.UserId, request.ApplicationId);

        private bool HasClientConnected(IList<string> connectionsIds)
        => connectionsIds.Count > 0;
    }
}
