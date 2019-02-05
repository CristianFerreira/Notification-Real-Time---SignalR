using Domain.Models;
using Domain.ResponseModels;
using Repository.Configuration;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository.Repositories
{
    public class ClientNotificationRepository : GenericRepository<ClientNotification>, IClientNotificationRepository
    {
        public ClientNotificationRepository() : base(new EntityContext(), null) { }
        public ClientNotificationRepository(DbContext context, DbContextTransaction transaction)
                                : base(context, transaction) { }

        public int CountNotificationsUnchecked(string userId, int applicationId)
        => ExecuteQuery<int>(@"  SELECT COUNT(CN.Checked) as Unchecked  
                                 FROM ClientNotification as CN
                                 INNER JOIN Client AS C on CN.ClientId = C.Id
                                 INNER JOIN Notification AS N on CN.NotificationId = N.Id
                                 WHERE UserId = @UserId AND CN.Checked = 0 AND N.ApplicationId = @ApplicationId", 
                                 new { UserId = userId, ApplicationId = applicationId }).ToList().FirstOrDefault();

        public IEnumerable<ClientNotificationsListResponse> List(string userId, int applicationId)
        => ExecuteQuery<ClientNotificationsListResponse>(@"SELECT   CN.Id as ClientNotificationId,
                                                                    CN.Checked,
                                                                    CN.Visualized,
	                                                                N.Owner,
	                                                                N.ApplicationId,
                                                                    N.Title,
	                                                                N.Message,
	                                                                N.NotificationDate 
                                                            FROM ClientNotification as CN
                                                            INNER JOIN Notification as N on CN.NotificationId = N.ID
                                                            INNER JOIN Client as C on CN.ClientId = C.Id
                                                            WHERE C.UserId = @UserId AND CN.Enabled = 1
                                                                  AND N.ApplicationId = @ApplicationId
                                                            ORDER BY N.NotificationDate desc",
                                                            new { UserId = userId, ApplicationId = applicationId });

        public IEnumerable<ClientNotificationsListResponse> ListNotVisualized(string userId, int applicationId)
        => ExecuteQuery<ClientNotificationsListResponse>(@"SELECT   CN.Id as ClientNotificationId,
                                                                    CN.Checked,
                                                                    CN.Visualized,
	                                                                N.Owner,
	                                                                N.ApplicationId,
                                                                    N.Title,
	                                                                N.Message,
	                                                                N.NotificationDate 
                                                            FROM ClientNotification as CN
                                                            INNER JOIN Notification as N on CN.NotificationId = N.ID
                                                            INNER JOIN Client as C on CN.ClientId = C.Id
                                                            WHERE C.UserId = @UserId AND CN.Visualized = 0 AND CN.Enabled = 1
                                                                  AND N.ApplicationId = @ApplicationId
                                                            ORDER BY N.NotificationDate desc",
                                                            new { UserId = userId, ApplicationId = applicationId });

        public IEnumerable<ClientNotificationsListResponse> ListVisualized(string userId, int applicationId)
        => ExecuteQuery<ClientNotificationsListResponse>(@"SELECT   CN.Id as ClientNotificationId,
                                                                    CN.Checked,
                                                                    CN.Visualized,
	                                                                N.Owner,
	                                                                N.ApplicationId,
                                                                    N.Title,
	                                                                N.Message,
	                                                                N.NotificationDate 
                                                            FROM ClientNotification as CN
                                                            INNER JOIN Notification as N on CN.NotificationId = N.ID
                                                            INNER JOIN Client as C on CN.ClientId = C.Id
                                                            WHERE C.UserId = @UserId AND CN.Visualized = 1 AND CN.Enabled = 1
                                                                  AND N.ApplicationId = @ApplicationId
                                                            ORDER BY N.NotificationDate desc",
                                                            new { UserId = userId, ApplicationId = applicationId });

        public IEnumerable<ClientNotification> GetUnChecked(string userId, int applicationId)
        => ExecuteQuery<ClientNotification>(@"SELECT CN.Id, 
                                                     CN.NotificationId, 
                                                     CN.ClientId, 
                                                     CN.Checked,
                                                     CN.CreatedDate,
                                                     CN.CreatedUserId,
                                                     CN.ModifiedDate,
                                                     CN.ModifiedUserId,
                                                     CN.Visualized,
                                                     CN.Enabled
                                              FROM ClientNotification as CN
                                              INNER JOIN Client as C on CN.ClientId = C.Id
                                              INNER JOIN Notification as N on CN.NotificationId = N.Id
                                              WHERE C.UserId = @UserId AND CN.Checked = 0 AND N.ApplicationId = @ApplicationId",
                                              new { UserId = userId, ApplicationId = applicationId });

        public ClientNotification Find(int id)
        => ExecuteQuery<ClientNotification>(@"SELECT *
                                              FROM ClientNotification
                                              WHERE Id = @Id",
                                              new { Id = id })
                                              .ToList()
                                              .FirstOrDefault();

        protected override void SpecificHandlingDatabaseException(Exception ex, ClientNotification entity)
        {
            throw new NotImplementedException();
        }
    }
}
