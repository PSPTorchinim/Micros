using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyAPI.Data;
using CompanyAPI.Entities;
using Shared.Repositories;

namespace CompanyAPI.Repositories
{
    public interface IClientsRepository:IRepository<Client>{

    }

    public class ClientsRepository : Repository<Client, BrandContext>, IClientsRepository
    {
        public ClientsRepository(BrandContext context, ILogger<IClientsRepository> logger) : base(context, logger)
        {
        }
    }
}