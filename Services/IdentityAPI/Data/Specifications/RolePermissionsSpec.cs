using Shared.Data.Specifications;
using IdentityAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdentityAPI.Specifications
{
    public class RolePermissionsSpec : BaseSpecifcation<Role>
    {
        public RolePermissionsSpec()
        {
            Includes.Add(x => x.Include(r => r.Permissions));
        }

        public RolePermissionsSpec(Expression<Func<Role, bool>> criteria) : base(criteria)
        {
            Includes.Add(x => x.Include(r => r.Permissions));
        }
    }
}
