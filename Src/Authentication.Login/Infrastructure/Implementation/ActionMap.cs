using Authentication.Login.Domain.Implementation;
using Foundation.Base.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Login.Infrastructure.Implementation
{
    internal class ActionMap : EntityMap<Domain.Implementation.Action>, IEntityTypeConfiguration<Domain.Implementation.Action>
    {
        public override void Configure(EntityTypeBuilder<Domain.Implementation.Action> builder)
        {
            builder.ToTable("Action");
            base.Configure(builder);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(e => e.LstClaimAction)
                .WithOne(ca => ca.Action)
                .HasForeignKey(ca => ca.IdAction);
        }
    }
}