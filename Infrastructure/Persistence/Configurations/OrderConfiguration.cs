using Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderAggregate>
    {
        public void Configure(EntityTypeBuilder<OrderAggregate> builder)
        {
            builder.ToTable("order");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.UserId)
             .IsRequired();

            builder.Property(o => o.AddressId)
                   .IsRequired();

            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int");
            builder.Property(o => o.OrderNumber).HasColumnName("order_number").HasMaxLength(200);
            builder.Property(o => o.TotalAmount).HasColumnName("total_amount").HasColumnType("decimal(18,2)");
            builder.Property(o => o.DiscountAmount).HasColumnName("discount_amount").HasColumnType("decimal(18,2)");
            builder.Property(o => o.OrderDate).HasColumnName("order_date").HasColumnType("date");
            builder.Property(o => o.CustomerName).HasColumnName("customer_name").HasColumnType("varchar(250)");

            builder.HasOne(o => o.User)
               .WithMany(u => u.Orders)
               .HasForeignKey(o => o.UserId);

            builder.HasOne(o => o.Address)
               .WithMany(a => a.Orders)
               .HasForeignKey(o => o.AddressId);

            builder.HasMany(o => o.Products).WithMany(p => p.Orders);
        }
    }
}
