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
    public class AddressConfiguration : IEntityTypeConfiguration<AddressAggregate>
    {
        public void Configure(EntityTypeBuilder<AddressAggregate> builder)
        {
            builder.ToTable("address");
            builder.HasKey(a => a.Id);

            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");

            builder.Property(a => a.City)
               .HasMaxLength(100)
               .IsRequired();

            builder.Property(a => a.Street)
               .HasMaxLength(100)
               .IsRequired();

            builder.Property(a => a.UserId)
               .IsRequired();

            builder.HasOne(a => a.User)
               .WithMany(u => u.Addresses)
               .HasForeignKey(a => a.UserId);

            builder.HasMany(a => a.Orders)
               .WithOne(o => o.Address)
               .HasForeignKey(o => o.AddressId);
        }
    }
}
