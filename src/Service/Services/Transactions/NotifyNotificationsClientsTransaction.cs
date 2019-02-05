using Domain.AbstractionModels;
using Domain.Models;
using Domain.RequestModels;
using Repository.Repositories;
using Service.Contracts;
using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Service.Services.Transactions
{
    public class NotifyNotificationsClientsTransaction
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

        public void ExecuteNotifyNotificationsClientsTransaction(NotifyManyNotificationsClientsRequest request)
        {
            var connectionsIdList = new List<string>();
            foreach (var notifyClient in request.NotificationsClients)
            {
                RegisterNotification(notifyClient, 
                                     request.Owner, 
                                     request.ApplicationId, 
                                     request.UserContextId, 
                                     out Notification notification);

                RegisterClient(notifyClient, request.UserContextId, notification);

                AddConnectionsId(connectionsIdList, notifyClient.UsersId, request.ApplicationId);
            }

            if (HasClientConnected(connectionsIdList))
                NotificationClientService.NotifyClients(connectionsIdList);
        }

        private void AddConnectionsId(IList<string> connectionsIdList, IList<string> usersId, int applicationId)
        {
            var connectionsId = MemoryCacheService.GetConnectionId(usersId, applicationId);
            foreach (var connectionId in connectionsId)
                    connectionsIdList.Add(connectionId);
            
        }

        private void RegisterClient(NotifyClientAbstraction request, string userContextId, Notification notification)
        {
            foreach (var userId in request.UsersId)
            {
                var client = new Client(userId,
                                        userContextId);
                _clientRepository.Add(client);

                RegisterClientNotification(client.Id, notification.Id, userContextId);
            }
        }

        private void RegisterClientNotification(int clientId, int notificationId, string userContextId)
        => _clientNotificationRepository.Add(new ClientNotification(notificationId, clientId, userContextId));

        private void RegisterNotification(NotifyClientAbstraction request, 
                                          string owner, 
                                          int applicationId, 
                                          string userContextId, 
                                          out Notification notification)
        {
            notification = new Notification(request.Title,
                                            request.Message,
                                            owner,
                                            applicationId,
                                            userContextId);

            notification = _notificationRepository.Add(notification);
        }

        private bool HasClientConnected(IList<string> connectionsIds)
        => connectionsIds.Count > 0;
    }
}
