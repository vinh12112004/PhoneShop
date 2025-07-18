using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PhoneShop.Data.Config
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).UseIdentityColumn();
            
            builder.Property(c => c.ShippingAddress)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(c => c.CreatedAt).IsRequired();
            builder.Property(c => c.UserId)
                .IsRequired();
            builder.HasOne(c => c.User).WithOne(u => u.Cart)
                .HasForeignKey<Order>(c => c.UserId)
                .HasConstraintName("FK_Orders_Users");
        }
    }
}
