using Authentication.Login.Domain.Implementation;
using Foundation.Base.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Login.Infrastructure.Data
{
    internal class ProductMap : EntityMap<Product>, IEntityTypeConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            base.Configure(builder);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(e => e.Name)
                .IsUnique();

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.Property(e => e.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        }
    }
}
