using System;

namespace Domain.ResponseModels
{
    public class ClientNotificationsListResponse
    {
        public int ClientNotificationId { get; set; }
        public bool Checked { get; set; }
        public bool Visualized { get; set; }
        public string Owner { get; set; }
        public int ApplicationId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
