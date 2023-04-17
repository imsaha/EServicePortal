namespace EServicePortal.Domain.Common;

public record StronglyTypedId<TType>(TType Value) where TType : IEquatable<TType>
{
    public static implicit operator TType(StronglyTypedId<TType> id)
    {
        return id.Value;
    }
}

public record StronglyTypedId(long Value) : StronglyTypedId<long>(Value);
