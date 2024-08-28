using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodService.Data.Models.Configurations;

public class UserConfiguration :IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        /*public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }*/
        builder.ToTable(nameof(User));
        builder.HasKey(x => x.UserId);
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.DisplayName).IsRequired();
        builder.Property(x => x.PhoneNumber).IsRequired();
    }
}
