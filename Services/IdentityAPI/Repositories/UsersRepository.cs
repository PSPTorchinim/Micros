using Shared.Repositories;
using IdentityAPI.Data;
using IdentityAPI.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.Repositories
{
    public class UsersRepository : Repository<User, IdentityContext>, IUsersRepository
    {
        public UsersRepository(IdentityContext context, ILogger<IUsersRepository> logger) : base(context, logger)
        {
        }

        public override async Task<List<User>> Get(Expression<Func<User, bool>> expression){
            var users = _context.Users.Include(u => u.Passwords).Include(u => u.Blocks).Include(u => u.Roles).ThenInclude(r => r.Permissions);
            return await users.Where(expression).ToListAsync();
        }
    }

    public interface IUsersRepository : IRepository<User>
    {
        new Task<List<User>> Get(Expression<Func<User, bool>> expression);
    }
}
