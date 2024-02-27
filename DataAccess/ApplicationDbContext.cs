using Clothing_Store.ViewModels;
using Clothing_Store.ViewModels.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clothing_Store.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<RegisterUserEntity>
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<RegisterUserEntity> AspNetUsers { get; set; } = null!;

        public virtual DbSet<ProductsEntity> tblProducts { get; set; } = null!;

        public virtual DbSet<UserCartEntity> tblUserCartEntities { get; set; } = null!;

        public virtual DbSet<OrderEntity> tblOrderEntities { get; set; } = null!;

        public virtual DbSet<OrderItemEntity> tblOrderItemEntities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
