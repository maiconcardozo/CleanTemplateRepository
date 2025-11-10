using Authentication.Login.Domain.Implementation;
using Foundation.Base.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Login.Infrastructure.Data
{
    internal class EntityTemplateExampleMap : EntityMap<EntityTemplateExample>, IEntityTypeConfiguration<EntityTemplateExample>
    {
        public override void Configure(EntityTypeBuilder<EntityTemplateExample> builder)
        {
            builder.ToTable("EntityTemplateExample");

            base.Configure(builder);

            builder.Property(e => e.Pro1)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Pro2)
                .IsRequired();

            builder.Property(e => e.Pro3)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Pro4)
                .IsRequired();

            builder.Property(e => e.Pro5)
                .IsRequired();
        }
    }
}
