using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models; // import the Stock and Comment models, which represent the database tables
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions) // constructor that takes in DbContextOptions and passes it to the base DbContext class
        {
            
        }
        public DbSet<Stock> Stocks{ get; set; } // represents the Stocks table in the database
        public DbSet<Comment> Comments{ get; set; } // maps the Comments table in the database from the Comment model
        public DbSet<Portfolio> Portfolios{ get; set; } // maps the Portfolios table in the database from the Portfolio model
        protected override void OnModelCreating(ModelBuilder builder) // when model is being created, configures the model and seed data
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId })); //set composite key

            builder.Entity<Portfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);
            
            builder.Entity<Portfolio>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.StockId);
            
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "Admin",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "User",
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles); // seeds DB w/Admin and User roles when DB is created
        }
    }
}