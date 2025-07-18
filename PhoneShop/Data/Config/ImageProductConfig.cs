using Microsoft.EntityFrameworkCore;

namespace PhoneShop.Data.Config
{
    public class ImageProductConfig :IEntityTypeConfiguration<ImageProduct>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ImageProduct> builder)
        {
            builder.ToTable("ImageProducts");
            builder.HasKey(ip => ip.Id);
            builder.Property(ip => ip.Id).UseIdentityColumn();
            builder.Property(ip => ip.ProductId).IsRequired();
            builder.Property(ip => ip.ImageUrl).IsRequired().HasMaxLength(500);
            builder.HasOne(ip => ip.Product)
                .WithMany(p => p.ImageProducts)
                .HasForeignKey(ip => ip.ProductId)
                .HasConstraintName("FK_ImageProducts_Products");
        }
    }
}
