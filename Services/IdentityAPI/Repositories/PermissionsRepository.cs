using Shared.Repositories;
using IdentityAPI.Data;
using IdentityAPI.Entities;

namespace IdentityAPI.Repositories
{
    public class PermissionsRepository : Repository<Permission, IdentityContext>, IPermissionsRepository
    {
        public PermissionsRepository(IdentityContext context, ILogger<IPermissionsRepository> logger) : base(context, logger)
        {
        }
    }

    public interface IPermissionsRepository : IRepository<Permission>
    {
    }
}
