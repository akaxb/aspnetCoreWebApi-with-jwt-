using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Data.EntityConfigurations;
using User.API.Models;

namespace User.API.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<UserProperty> UserProperties { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new UserPropertyConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
