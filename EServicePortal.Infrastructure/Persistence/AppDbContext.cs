using EServicePortal.Application.Common.Interfaces;
using EServicePortal.Domain.Entites;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EServicePortal.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext, IAppDbContext, IDataProtectionKeyContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Users = Set<User>();
        Roles = Set<Role>();
        UserRoles = Set<UserRole>();
        DataProtectionKeys = Set<DataProtectionKey>();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.Entity<Role>().HasData(Seeder.Roles);
    }
}
