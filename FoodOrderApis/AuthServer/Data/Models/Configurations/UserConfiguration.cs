using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Data.Models.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.ClientId).IsRequired();
        builder.Property(x => x.Displayname).IsRequired().HasMaxLength(100);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.CreatedDate)
            .HasConversion(
            v => v.ToUniversalTime(), // Convert to UTC before saving
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Specify kind when reading
        );
    }
}
