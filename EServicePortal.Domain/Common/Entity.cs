namespace EServicePortal.Domain.Common;

public interface IEntity
{
}

public abstract class BaseEntity<TId>
{
    public TId Id { get; set; } = default!;
}

public abstract class Entity<TStronglyTypedId> : BaseEntity<TStronglyTypedId>, IEntity where TStronglyTypedId : StronglyTypedId
{
}
