using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Domain.ResponseModels;
using Repository.Contracts;
using Repository.Repositories;
using Service.Contracts;

namespace Service.Services
{
    public class ClientService : GenericService<Client, ClientRepository>, IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService() { }

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        
    }
}
