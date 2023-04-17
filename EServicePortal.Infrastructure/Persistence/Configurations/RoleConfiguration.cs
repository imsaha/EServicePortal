using EServicePortal.Domain.Entites;
using EServicePortal.Infrastructure.Persistence.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EServicePortal.Infrastructure.Persistence.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(x=>x.Id).HasStronglyTypedId();
        builder.HasIndex(x => x.Name).IsUnique();
    }
}
