using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderService.Data.Models.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(e => e.UserId);
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.DisplayName).IsRequired();
        builder.Property(e => e.PhoneNumber).IsRequired();
    }
}
