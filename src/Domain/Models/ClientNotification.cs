using AuditorHelper.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("ClientNotification")]
    public class ClientNotification : Auditable
    {
        protected ClientNotification() { }
        public ClientNotification(int notificationId,
                                  int clientId,
                                  string userContextId) : base(userContextId)
        {
            NotificationId = notificationId;
            ClientId = clientId;
            Enabled = true;
        }

        public int Id { get; private set; }
        public int NotificationId { get; private set; }
        public int ClientId { get; private set; }
        public bool Checked { get; private set; }
        public bool Visualized { get; private set; }
        public bool Enabled { get; private set; }

        public void Check()
        => Checked = true;

        public void Visualize()
        => Visualized = true;

        public void Disable()
       => Enabled = false;
    }
}
