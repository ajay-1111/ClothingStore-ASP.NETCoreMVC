using Clothing_Store.ViewModels;
using Clothing_Store.ViewModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clothing_Store.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<RegisterUserEntity> tblUserRegistration { get; set; } = null!;

        public virtual DbSet<ProductsEntity> tblProducts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base method
            base.OnModelCreating(modelBuilder);

            // Specify the SQL Server column type for the Price property
            modelBuilder.Entity<ProductsEntity>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)"); // Adjust the precision and scale as per your requirements
        }
    }
}
