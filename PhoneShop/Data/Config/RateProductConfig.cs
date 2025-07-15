using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PhoneShop.Data.Config
{
    public class RateProductConfig : IEntityTypeConfiguration<RateProduct>
    {
        public void Configure(EntityTypeBuilder<RateProduct> builder)
        {
            builder.ToTable("RateProducts");
            builder.HasKey(rp => rp.Id);
            builder.Property(rp => rp.Id).ValueGeneratedOnAdd();
            builder.Property(rp => rp.UserId).IsRequired();
            builder.Property(rp => rp.ProductId).IsRequired();
            builder.Property(rp => rp.Rate).IsRequired();

            builder.HasOne(p => p.Product)
                .WithMany(p => p.RateProducts)
                .HasForeignKey(p => p.ProductId)
                .HasConstraintName("FK_RateProducts_Products");

            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .HasConstraintName("FK_RateProducts_Users");
        }
    }
}
