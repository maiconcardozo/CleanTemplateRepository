using Authentication.Login.Domain.Implementation;
using Foundation.Base.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Login.Infrastructure.Implementation
{
    internal class AccountClaimActionMap : EntityMap<AccountClaimAction>, IEntityTypeConfiguration<AccountClaimAction>
    {
        public override void Configure(EntityTypeBuilder<AccountClaimAction> builder)
        {
            builder.ToTable("AccountClaimAction");
            base.Configure(builder);

            builder.Property(e => e.IdAccount).IsRequired();
            builder.Property(e => e.IdClaimAction).IsRequired();

            builder.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.IdAccount);

            builder.HasOne(e => e.ClaimAction)
                .WithMany(ca => ca.LstAccountClaimAction)
                .HasForeignKey(e => e.IdClaimAction);
        }
    }
}
