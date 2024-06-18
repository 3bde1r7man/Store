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
        public DbSet<UserOrder> UserOrders { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string projectRootDirectory = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
            string databaseDirectory = Path.Combine(projectRootDirectory, "db");

            // Ensure the 'dp' directory exists
            if (!Directory.Exists(databaseDirectory))
            {
                Directory.CreateDirectory(databaseDirectory);
            }

            string databasePath = Path.Combine(databaseDirectory, "StoreDB.db");
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
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


            builder.Entity<UserOrder>()
                .HasOne(uo => uo.User)
                .WithMany() 
                .HasForeignKey(uo => uo.UserId);

            builder.Entity<UserOrder>()
                .HasOne(uo => uo.Order)
                .WithMany() 
                .HasForeignKey(uo => uo.OrderId);

            builder.Entity<UserOrder>()
                .HasOne(uo => uo.Product)
                .WithMany() 
                .HasForeignKey(uo => uo.ProductId);
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
