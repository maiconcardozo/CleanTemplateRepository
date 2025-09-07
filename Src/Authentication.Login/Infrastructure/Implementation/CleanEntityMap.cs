using Authentication.Login.Domain.Implementation;
using Foundation.Base.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Login.Infrastructure.Implementation
{
    /// <summary>
    /// Entity Framework configuration for the CleanEntity entity.
    /// Defines table structure, constraints, and relationships.
    /// </summary>
    internal class CleanEntityMap : EntityMap<CleanEntity>, IEntityTypeConfiguration<CleanEntity>
    {
        /// <summary>
        /// Configures the CleanEntity entity for Entity Framework.
        /// </summary>
        /// <param name="builder">Entity type builder for CleanEntity</param>
        public override void Configure(EntityTypeBuilder<CleanEntity> builder)
        {
            builder.ToTable("CleanEntity");
            base.Configure(builder);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            // Create unique index on Name for business logic enforcement
            builder.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("IX_CleanEntity_Name_Unique");
        }
    }
}