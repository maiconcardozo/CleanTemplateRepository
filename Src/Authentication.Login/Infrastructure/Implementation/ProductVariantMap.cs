using Authentication.Login.Domain.Implementation;
using Foundation.Base.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Login.Infrastructure.Data
{
    internal class ProductVariantMap : EntityMap<ProductVariant>, IEntityTypeConfiguration<ProductVariant>
    {
        public override void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("ProductVariant");

            base.Configure(builder);

            builder.Property(e => e.IdProduct)
                .IsRequired();

            builder.Property(e => e.SKU)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(e => e.SKU)
                .IsUnique();

            builder.Property(e => e.Color)
                .HasMaxLength(50);

            builder.Property(e => e.Size)
                .HasMaxLength(20);

            builder.Property(e => e.StockQuantity)
                .IsRequired();

            // Configure relationship
            builder.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.IdProduct)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
