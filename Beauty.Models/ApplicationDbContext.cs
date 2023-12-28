using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Beauty.Models;
using Microsoft.AspNetCore.Identity;

namespace Beauty.Repository
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        
        
        
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
      
        public DbSet<Category> Categories{ get; set; }
        public DbSet<TypeService> TypeServices  { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Master> Masters { get; set; }

        public DbSet<BService> BServices { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Record> Records  { get; set; }
        public DbSet<RecordDetail> RecordDetails  { get; set; } 
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> orderStatuses { get; set; }

    }

}
