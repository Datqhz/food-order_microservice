using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderService.Data.Models.Configurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.ToTable("OrderDetail");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.OrderId).IsRequired();
        builder.Property(x => x.FoodId).IsRequired();
        builder.HasOne(x => x.Food)
            .WithMany(x => x.OrderDetails)
            .HasForeignKey(x => x.FoodId);
        builder.HasOne(x => x.Order)
            .WithMany(x => x.OrderDetails)
            .HasForeignKey(x => x.OrderId);
    }
}
