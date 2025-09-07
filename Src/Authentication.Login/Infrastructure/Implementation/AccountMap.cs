using Authentication.Login.Domain.Implementation;
using Foundation.Base.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Login.Infrastructure.Data
{
    internal class AccountMap : EntityMap<Account>, IEntityTypeConfiguration<Account>
    {
        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");

            base.Configure(builder);

            builder.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.HasIndex(e => e.UserName)
                .IsUnique();
            
            builder.Property(e => e.Password)
               .IsRequired()
               .HasMaxLength(128);
        }
    }
}
