using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace PhoneShop.Data.Config
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).UseIdentityColumn();
            builder.Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(r => r.Description)
                .IsRequired(false)
                .HasMaxLength(200);
            builder.HasData(
                new Role { Id = 1, RoleName = "Admin", Description = "Quản trị viên" },
                new Role { Id = 2, RoleName = "Customer", Description = "Khách hàng" }
            );
        }
    }
}
