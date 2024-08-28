using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerService.Data.Models.Configurations;

public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable(nameof(UserInfo));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ClientId).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.UserName).IsRequired();
        builder.Property(x => x.PhoneNumber).IsRequired();
        builder.Property(x => x.CreatedDate)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
    }
}