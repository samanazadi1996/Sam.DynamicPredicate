using Microsoft.EntityFrameworkCore;
using Sam.DynamicPredicateAPI.Models;

namespace Sam.DynamicPredicateAPI.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>().HasKey(x => x.Id);

            builder.Entity<Product>().Property(p => p.Name).HasMaxLength(100);
            builder.Entity<Product>().Property(p => p.BarCode).HasMaxLength(50);

            base.OnModelCreating(builder);
        }
    }

}
