using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Models;
using System.Reflection.Emit;

namespace Store.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> orderProducts { get; set; }

        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use the connection string from appsettings.json if it's configured
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                // Use the connection string if available
                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    optionsBuilder.UseSqlite(connectionString);
                }
                else
                {
                    throw new Exception("No connection string found in appsettings.json");
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole("Admin");
            admin.NormalizedName = "ADMIN";

            var user = new IdentityRole("User");
            user.NormalizedName = "USER";

            builder.Entity<IdentityRole>().HasData(admin, user);

            builder.ApplyConfiguration(new UserConfiguration());


            builder.Entity<Order>()
            .HasMany(o => o.orderProducts)
            .WithOne(op => op.Order)
            .HasForeignKey(op => op.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(u => u.LastName)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(u => u.Address)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(u => u.Balance)
                .IsRequired();
        }
    }
}
