using Shared.Helpers;
using Shared.Services.Database;
using IdentityAPI.Entities;
using IdentityAPI.Repositories;

namespace IdentityAPI.Data
{
    internal class SeedData : IDatabaseInitializer
    {
        private readonly IUsersRepository usersRepository;
        private readonly IRolesRepository rolesRepository;
        private readonly IPermissionsRepository permissionsRepository;


        public SeedData(IUsersRepository usersRepository, IRolesRepository rolesRepository, IPermissionsRepository permissionsRepository)
        {
            this.usersRepository = usersRepository;
            this.rolesRepository = rolesRepository;
            this.permissionsRepository = permissionsRepository;
        }

        public async Task InitializeAsync()
        {
            if (await permissionsRepository.Empty()) await SeedPermissions();
            if (await rolesRepository.Empty()) await SeedRoles();
            if (await usersRepository.Empty()) await SeedUsers();
        }

        private async Task SeedUsers()
        {
            await usersRepository.Add(new User()
            {
                Email = "huberttroc@gmail.com",
                Passwords = new List<Password>() { 
                    new Password() { Value = "testPassword1234".computeHash() } 
                },
                Roles = await rolesRepository.Get(x => x.Name.Equals("SuperOwner")),
                Activated = true, 
                ActivationCode = StringHelper.GenerateRandomPassword(5)
            });
        }

        private async Task SeedRoles()
        {
            await rolesRepository.Add(new Role()
            {
                Name = "SuperOwner",
                Description = "Full access to all functions",
                Permissions = await permissionsRepository.Get(x => x.Name.Equals("Full"))
            });
        }

        private async Task SeedPermissions()
        {
            await permissionsRepository.Add(new Permission()
            {
                Name = "Full",
                Description = "Full access to all functions"
            });
            await permissionsRepository.Add(new Permission()
            {
                Name = "GetRoles",
                Description = "Get roles from database"
            });
            await permissionsRepository.Add(new Permission()
            {
                Name = "AddRole",
                Description = "Add new role"
            });
            await permissionsRepository.Add(new Permission()
            {
                Name = "GetPermissions",
                Description = "Gets list of permissions"
            });
            await permissionsRepository.Add(new Permission()
            {
                Name = "AddPermission",
                Description = "Add permission"
            });
        }
    }
}