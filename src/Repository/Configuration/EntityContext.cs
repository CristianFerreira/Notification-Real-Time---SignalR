using Domain.Models;
using System.Data.Entity;

namespace Repository.Configuration
{
    public class EntityContext : DbContext
    {
        public EntityContext() : base("name=Entity") { Configuration.ProxyCreationEnabled = false; }

        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientNotification> ClientNotification { get; set; }

    }
}
