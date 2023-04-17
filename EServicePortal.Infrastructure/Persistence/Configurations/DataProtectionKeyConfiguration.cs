using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EServicePortal.Infrastructure.Persistence.Configurations;

public class DataProtectionKeyConfiguration : IEntityTypeConfiguration<DataProtectionKey>
{
    public void Configure(EntityTypeBuilder<DataProtectionKey> builder)
    {
        builder.ToTable("data_protection_keys");
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Xml).HasColumnName("xml").HasMaxLength(1000);
        builder.Property(x => x.FriendlyName).HasColumnName("friendly_name");
    }
}
