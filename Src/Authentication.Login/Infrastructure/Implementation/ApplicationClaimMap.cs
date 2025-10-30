using Authentication.Login.Domain.Implementation;
using Foundation.Base.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Login.Infrastructure.Implementation
{
    internal class ApplicationClaimMap : EntityMap<ApplicationClaim>, IEntityTypeConfiguration<ApplicationClaim>
    {
        public override void Configure(EntityTypeBuilder<ApplicationClaim> builder)
        {
            builder.ToTable("ApplicationClaim");
            base.Configure(builder);

            builder.Property(e => e.IdApplication).IsRequired();
            builder.Property(e => e.IdClaim).IsRequired();

            builder.HasOne(e => e.Application)
                .WithMany(a => a.LstApplicationClaim)
                .HasForeignKey(e => e.IdApplication);

            builder.HasOne(e => e.Claim)
                .WithMany()
                .HasForeignKey(e => e.IdClaim);
        }
    }
}
