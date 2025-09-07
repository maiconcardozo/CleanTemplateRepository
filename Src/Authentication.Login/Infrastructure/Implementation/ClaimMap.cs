using Authentication.Login.Domain.Implementation;
using Foundation.Base.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Login.Infrastructure.Implementation
{
    internal class ClaimMap : EntityMap<Claim>, IEntityTypeConfiguration<Claim>
    {
        public override void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder.ToTable("Claim");
            base.Configure(builder);

            builder.Property(e => e.Type)
                .IsRequired();

            builder.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(255);

            builder.HasMany(e => e.LstClaimAction)
                .WithOne(ca => ca.Claim)
                .HasForeignKey(ca => ca.IdClaim);
        }
    }
}