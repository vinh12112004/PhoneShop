using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PhoneShop.Data.Config
{
    public class CartItemConfig : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id).UseIdentityColumn();
            builder.Property(ci => ci.CartId)
                .IsRequired();
            builder.Property(ci => ci.ProductId).IsRequired();
            builder.Property(ci => ci.Quantity)
                .IsRequired();
            builder.HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .HasConstraintName("FK_CartItems_Carts");

        }
    }
}
