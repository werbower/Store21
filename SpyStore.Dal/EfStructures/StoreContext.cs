using Microsoft.EntityFrameworkCore;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpyStore.Dal.EfStructures
{
    public class StoreContext: DbContext
    {
        public int CustomerId { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCartRecord> ShoppingCartRecords { get; set; }

        public StoreContext(DbContextOptions<StoreContext> dbContextOptions): base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<Customer>(e =>
            {
                e.HasIndex(i => i.EmailAddress).HasName("IX_Customers").IsUnique();
            });

            modelBuilder.Entity<Order>(e =>
            {
                e.HasQueryFilter(x => x.CustomerId == CustomerId);
                e.Property(i => i.OrderDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
                e.Property(i => i.ShipDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<OrderDetail>(e =>
            {
                e.Property(i => i.UnitCost).HasColumnType("money");
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.Property(i => i.UnitCost).HasColumnType("money");
                e.Property(i => i.CurrentPrice).HasColumnType("money");
            });

            modelBuilder.Entity<ShoppingCartRecord>(e =>
            {
                e.Property(i => i.DateCreated).HasColumnType("datetime").HasDefaultValueSql("getdate()");
                e.Property(i => i.Quantity).HasDefaultValue(1);
                e.HasIndex(i => new { ShoppingCartRecordId = i.Id, i.CustomerId, i.ProductId }).HasName("IX_ShoppingCart");
                e.HasQueryFilter(x => x.CustomerId == CustomerId);
            });

        }

       
    }
}
