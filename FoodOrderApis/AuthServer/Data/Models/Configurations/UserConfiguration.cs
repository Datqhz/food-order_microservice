using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Data.Models.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "auth");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Username).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ClientId).IsRequired();
        builder.Property(x => x.Displayname).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(10);
        builder.Property(x => x.PasswordHash).IsRequired();
        builder.Property(x => x.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        builder.Property(x => x.IsActive).HasDefaultValue(true);
    }
}
