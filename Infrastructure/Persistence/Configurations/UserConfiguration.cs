using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserAggregate>
    {
        public void Configure(EntityTypeBuilder<UserAggregate> builder)
        {
            builder.ToTable("user");
            builder.HasKey(u => u.Id);

            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(u => u.Name).HasColumnName("name").HasColumnType("varchar(250)");
            builder.Property(u => u.Surname).HasColumnName("surname").HasColumnType("varchar(250)");
            builder.Property(u => u.Email).HasColumnName("email").HasColumnType("varchar(100)");
            builder.Property(u => u.Password).HasColumnName("password").HasColumnType("varchar(100)");
            builder.Property(u => u.Gender).HasColumnName("gender").HasColumnType("int");

            builder.HasMany(u => u.Addresses)
               .WithOne(a => a.User)
               .HasForeignKey(a => a.UserId);

            builder.HasMany(u => u.Orders)
              .WithOne(o => o.User)
              .HasForeignKey(o => o.UserId);

        }
    }
}
