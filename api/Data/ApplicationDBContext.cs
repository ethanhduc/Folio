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
        protected override void OnModelCreating(ModelBuilder builder) // when model is being created, configures the model and seed data
        {
            base.OnModelCreating(builder);
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