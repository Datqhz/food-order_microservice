using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodService.Data.Models.Configurations;

public class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> builder)
    {
        builder.ToTable(nameof(Food));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.ImageUrl).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.HasOne(x => x.User)
            .WithMany(x => x.Foods)
            .HasForeignKey(x => x.UserId);
    }
}
