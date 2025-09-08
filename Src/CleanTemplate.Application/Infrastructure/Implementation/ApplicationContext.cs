using CleanTemplate.Application.Domain.Implementation;
using CleanTemplate.Application.Infrastructure.Implementation;
using CleanTemplate.Application.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Infrastructure.Data
{
    public class ApplicationContext : DbContext, IApplicationContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<CleanEntity> dbCleanEntity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            LoadModel(modelBuilder);
        }

        public static void LoadModel(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CleanEntityMap());
        }
    }
}