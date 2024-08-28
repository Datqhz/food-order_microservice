using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderService.Data.Models.Configurations;

public class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> builder)
    {
        builder.ToTable("Food");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.ImageUrl).IsRequired();
        builder.Property(x => x.Describe);
    }
}
