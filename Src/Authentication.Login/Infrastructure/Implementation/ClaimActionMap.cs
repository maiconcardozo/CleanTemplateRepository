using Authentication.Login.Domain.Implementation;
using Foundation.Base.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Login.Infrastructure.Implementation
{
    internal class ClaimActionMap : EntityMap<ClaimAction>, IEntityTypeConfiguration<ClaimAction>
    {
        public override void Configure(EntityTypeBuilder<ClaimAction> builder)
        {
            builder.ToTable("ClaimAction");
            base.Configure(builder);

            builder.Property(e => e.IdClaim).IsRequired();
            builder.Property(e => e.IdAction).IsRequired();

            builder.HasOne(e => e.Claim)
                .WithMany(c => c.LstClaimAction)
                .HasForeignKey(e => e.IdClaim);

            builder.HasOne(e => e.Action)
                .WithMany(a => a.LstClaimAction)
                .HasForeignKey(e => e.IdAction);

            builder.HasMany(e => e.LstAccountClaimAction)
                .WithOne(aca => aca.ClaimAction)
                .HasForeignKey(aca => aca.IdClaimAction);
        }
    }
}