using Microsoft.EntityFrameworkCore;
using OrderSolution.Infrastructure.Entities;

namespace OrderSolution.Infrastructure.Data;

public class CfContext(DbContextOptions<CfContext> options) : DbContext(options)
{
    public DbSet<CustomerAddressEntity> CustomerAddresses { get; set; }
    public DbSet<CustomerDetailEntity> CustomerDetails { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItemEntity>()
            .HasKey(x => new {x.OrderId, x.ArticleNumber });
        
        modelBuilder.Entity<OrderItemEntity>().HasOne(x => x.Order)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.OrderId);

        modelBuilder.Entity<CustomerDetailEntity>()
            .HasIndex(x => x.Email).IsUnique();
    }
}