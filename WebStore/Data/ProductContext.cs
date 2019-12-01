using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStore.Models;
using WebStore.Models.Identity;

namespace WebStore.Data
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string moderatorRoleName = "moderator";
            string copywriterRoleName = "copywriter";
            string userRoleName = "user";

            var adminRole = new Role() { ID = 1, Name = adminRoleName };
            var moderatorRole = new Role() { ID = 2, Name = moderatorRoleName };
            var copywriterRole = new Role() { ID = 3, Name = copywriterRoleName };
            var userRole = new Role() { ID = 4, Name = userRoleName };

            var admin = new User() { ID = 1, Email = "and91@outlook.com", Password = "Rosinant1991", FirstName = "Andrey", LastName = "Osipov", DateRegistered = DateTime.Now, RoleId = adminRole.ID, Age = 28, Login = "AndrYOUsha" };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, moderatorRole, copywriterRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { admin });

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Characteristic>().ToTable("Characteristic");

            base.OnModelCreating(modelBuilder);
        }
    }
}
