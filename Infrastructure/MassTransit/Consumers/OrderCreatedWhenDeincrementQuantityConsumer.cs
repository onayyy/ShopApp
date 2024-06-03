using Application.Common.Interfaces;
using Domain.Events.Order;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.MassTransit.Consumers
{
    public class OrderCreatedWhenDeincrementQuantityConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IPizzaAppDbContext _dbContext;

        public OrderCreatedWhenDeincrementQuantityConsumer(IPizzaAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var products = await _dbContext.Products.Where(x => context.Message.ProductIds.Contains(x.Id)).ToListAsync();

            foreach (var product in products)
            {
                if (product.Quantity > 0)
                    product.DeincrementQuantity();

                else
                    throw new Exception($"Product {product.Id} is out of stock.");
            }


            _dbContext.Products.UpdateRange(products);
            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }

    }
}
