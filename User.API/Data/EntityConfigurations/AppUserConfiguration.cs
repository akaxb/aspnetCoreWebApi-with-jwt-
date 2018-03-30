using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Models;

namespace User.API.Data.EntityConfigurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable($"sys{nameof(AppUser)}s");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).HasMaxLength(128);
            builder.Property(a => a.Company).HasMaxLength(128);
            builder.Property(a => a.Avatar).HasMaxLength(256);
        }
    }
}
