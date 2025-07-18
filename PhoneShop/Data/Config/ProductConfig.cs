using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;


namespace PhoneShop.Data.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();

            builder.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.Description)
                .IsRequired(false)
                .HasMaxLength(500);
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.Rate).IsRequired();
            builder.Property(p => p.Category)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
