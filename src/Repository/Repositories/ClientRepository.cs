using Domain.Models;
using Domain.ResponseModels;
using Repository.Configuration;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Repository.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository() : base(new EntityContext(), null) { }
        public ClientRepository(DbContext context, DbContextTransaction transaction)
                                : base(context, transaction) { }

   
        protected override void SpecificHandlingDatabaseException(Exception ex, Client entity)
        {
            throw new NotImplementedException();
        }
    }
}
