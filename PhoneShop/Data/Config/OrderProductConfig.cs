using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PhoneShop.Data.Config
{
    public class OrderProductConfig : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.ToTable("OrderProduct");
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id).UseIdentityColumn();
            builder.Property(ci => ci.OrderId)
                .IsRequired();
            builder.Property(ci => ci.ProductId).IsRequired();
            builder.Property(ci => ci.Quantity)
                .IsRequired();
            builder.HasOne(ci => ci.Order)
                .WithMany(c => c.OrderProducts)
                .HasForeignKey(ci => ci.OrderId)
                .HasConstraintName("FK_OrderProducts_Orders");

        }
    }
}
