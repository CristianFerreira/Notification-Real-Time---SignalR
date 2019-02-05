using AuditorHelper.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("Notification")]
    public class Notification : Auditable
    {
        protected Notification() { }

        public Notification(string title,
                            string message,
                            string owner,
                            int applicationId,
                            string userContextId) : base(userContextId)
        {
            Title = title;
            Message = message;
            Owner = owner;
            ApplicationId = applicationId;
            NotificationDate = DateTime.Now;
        }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public string Owner { get; private set; }
        public int ApplicationId { get; private set; }
        public DateTime NotificationDate { get; private set; }
    }
}
