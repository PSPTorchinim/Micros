using IdentityAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityAPI.Data
{
    public class IdentityContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<Block> Blocks { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(p => p.Email).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.CreatedDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Password>().Property(x => x.CreatedDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Role>().Property(x => x.CreatedDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Permission>().Property(x => x.CreatedDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Block>().Property(x => x.CreatedDate).HasDefaultValueSql("getdate()");
        }
    }
}