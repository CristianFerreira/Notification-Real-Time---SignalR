using Domain.Models;
using Repository.Configuration;
using Repository.Contracts;
using System;
using System.Data.Entity;

namespace Repository.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository() : base(new EntityContext(), null) { }

        public NotificationRepository(DbContext context, DbContextTransaction transaction) : base(context, transaction)
        {
        }

        protected override void SpecificHandlingDatabaseException(Exception ex, Notification entity)
        {
            throw new NotImplementedException();
        }
    }
}
