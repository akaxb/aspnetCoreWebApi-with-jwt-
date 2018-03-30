using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Models;

namespace User.API.Data.EntityConfigurations
{
    public class UserPropertyConfiguration : IEntityTypeConfiguration<UserProperty>
    {
        public void Configure(EntityTypeBuilder<UserProperty> builder)
        {
            builder.ToTable("tbUserProperties");
            builder.HasKey(a => new { a.AppUserId, a.Value, a.Key });
            builder.Property(a => a.Text).HasMaxLength(128);
            builder.Property(a => a.Key).HasMaxLength(50);
            builder.Property(a => a.Value).HasMaxLength(50);
        }
    }
}
