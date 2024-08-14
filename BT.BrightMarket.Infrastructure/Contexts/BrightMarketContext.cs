using BT.BrightMarket.Domain.Models.Conversations;
using BT.BrightMarket.Domain.Models.Products;
using BT.BrightMarket.Domain.Models.Users;
using BT.BrightMarket.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BT.BrightMarket.Infrastructure.Contexts
{
    public class BrightMarketContext : DbContext
    {
        public BrightMarketContext(DbContextOptions<BrightMarketContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configuration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // seeding
            modelBuilder.Entity<Region>().Seed();
            modelBuilder.Entity<Category>().Seed();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

    }
}
