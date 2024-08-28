using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Enums;

namespace OrderService.Data.Models.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        builder.Property(x => x.EaterId)
            .IsRequired();
        builder.Property(x => x.MerchantId)
            .IsRequired();
        builder.Property(x => x.OrderedDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        builder.Property(x => x.OrderStatus)
            .HasDefaultValue(1);

    }
}
