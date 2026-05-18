using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Entities;

namespace SubscriptionManager.Data
{
    /// <summary>
    /// EF Core database context for Subscription Manager.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship Customer -> Subscription
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Subscriptions)
                .WithOne(s => s.Customer)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: unique constraint to prevent duplicate subscriptions per customer+name
            modelBuilder.Entity<Subscription>()
                .HasIndex(s => new { s.CustomerId, s.Name })
                .IsUnique();
        }
    }
}
