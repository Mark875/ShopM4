using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ShopM4.Models;

namespace ShopM4.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<MyModel> MyModel { get; set; }

        public DbSet<Product> Product { get; set; }
    }
}

