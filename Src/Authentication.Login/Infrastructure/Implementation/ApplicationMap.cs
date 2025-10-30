using Authentication.Login.Domain.Implementation;
using Foundation.Base.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Login.Infrastructure.Implementation
{
    internal class ApplicationMap : EntityMap<Application>, IEntityTypeConfiguration<Application>
    {
        public override void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.ToTable("Application");
            base.Configure(builder);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasMany(e => e.LstApplicationClaim)
                .WithOne(ac => ac.Application)
                .HasForeignKey(ac => ac.IdApplication);
        }
    }
}
