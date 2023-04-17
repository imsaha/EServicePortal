using EServicePortal.Domain.Entites;

namespace EServicePortal.Infrastructure.Persistence;

public static class Seeder
{
    public static IEnumerable<Role> Roles = new[]
    {
        new Role
            { Id = new RoleId(1), Name = "Admin" },
        new Role
            { Id = new RoleId(2), Name = "User" },
        new Role
            { Id = new RoleId(3), Name = "Customer" }
    };
}
