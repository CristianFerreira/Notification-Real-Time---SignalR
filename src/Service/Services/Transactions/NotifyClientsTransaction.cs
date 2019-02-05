using AuditorHelper.Services;
using Domain.Models;
using Domain.RequestModels;
using Repository.Repositories;
using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net.Http;

namespace Service.Services.Transactions
{
    public class NotifyClientsTransaction
    {
        private NotificationRepository _notificationRepository;
        private ClientNotificationRepository _clientNotificationRepository;
        private ClientRepository _clientRepository;

        public void InstanceRepositories(DbContext context, DbContextTransaction transaction)
        {
            _notificationRepository = new NotificationRepository(context, transaction);
            _clientNotificationRepository = new ClientNotificationRepository(context, transaction);
            _clientRepository = new ClientRepository(context, transaction);
        }

        public void ExecuteNotifyClientsTransaction(NotifyClientRequest request)
        {
            RegisterNotification(request, out Notification notification);
            RegisterClient(request, notification);
            Notify(request.UsersId, request.ApplicationId);
        }

        private void Notify(IList<string> usersId, int applicationId)
        {
            var connectionsId = MemoryCacheService.GetConnectionId(usersId, applicationId);
            NotificationClientService.NotifyClients(connectionsId);
        }

        private void RegisterClient(NotifyClientRequest request, Notification notification)
        {
            foreach (var userId in request.UsersId)
            {
                var client = new Client(userId, 
                                        request.UserContextId);
                _clientRepository.Add(client);

                RegisterClientNotification(client.Id, notification.Id, request.UserContextId);
            }
        }

        private void RegisterClientNotification(int clientId, int notificationId, string userContextId)
        => _clientNotificationRepository.Add(new ClientNotification(notificationId, clientId, userContextId));
        

        private void RegisterNotification(NotifyClientRequest request, out Notification notification)
        {
            notification = new Notification(request.Title,
                                            request.Message,
                                            request.Owner,
                                            request.ApplicationId,
                                            request.UserContextId);

            notification = _notificationRepository.Add(notification);
        }
    }
}
