using Jumia.Model;
using Jumia.Dtos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Context
{
    public class JumiaContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> products { get; set; }
        public DbSet<Address> addresses { get; set; }
        public DbSet<CartItem> cartItems { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Item> items { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<ProductImage> productImages { get; set; }
        public DbSet<Review> reviews { get; set; }
        public JumiaContext() : base()
        {

        }
        public JumiaContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CartItem>()
          .HasOne(c => c.Product) // Assuming CartItem has a navigation property called Product
          .WithMany(p => p.OrderDetails) // Assuming Product has a collection property called CartItems
          .HasForeignKey(c => c.ProductID) // Assuming CartItem has a foreign key called ProductId
          .OnDelete(DeleteBehavior.Restrict);

        }


    }
}
