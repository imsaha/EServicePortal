using EServicePortal.Domain.Common;

namespace EServicePortal.Domain.Entites;

public record RoleId(long Value) : StronglyTypedId(Value);
public class Role : Entity<RoleId>
{
    public string Name { get; set; }
}
