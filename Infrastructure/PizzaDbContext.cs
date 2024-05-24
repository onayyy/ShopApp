using Application.Common.Interfaces;
using Domain.Model;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class PizzaAppDbContext : DbContext, IPizzaAppDbContext
    {
        public DbSet<ProductAggregate> Products { get; set; }
        public DbSet<OrderAggregate> Orders { get; set; }
        public DbSet<UserAggregate> Users { get; set; }
        public DbSet<AddressAggregate> Address { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new NpgsqlDataSourceBuilder("Server=localhost;Port=5432;Database=pizzaapp;Userid=postgres;Password=2019;Include Error Detail=True;");
            builder.EnableDynamicJson();
            var dataSource = builder.Build();
            optionsBuilder.UseNpgsql(dataSource);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
