using EServicePortal.Domain.Common;

namespace EServicePortal.Domain.Entites;

public record UserId(long Value) : StronglyTypedId(Value);

public class User : Entity<UserId>
{
    public string Username { get; set; } = default!;

    public string? DisplayName { get; set; }
    public string? EmailAddress { get; set; }
    public string? Mobile { get; set; }

    public string PasswordHash { get; set; } = default!;
    public string SecurityStamp { get; set; } = Guid.NewGuid().ToString("N");

    public ICollection<UserRole> Roles { get; } = new List<UserRole>();
}

public class UserRole
{
    public UserId UserId { get; set; } = default!;
    public User User { get; set; } = default!;

    public RoleId RoleId { get; set; } = default!;
    public Role Role { get; set; } = default!;
}
