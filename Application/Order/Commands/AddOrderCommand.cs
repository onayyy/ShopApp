using Application.Common.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Order.Commands
{
    public class AddOrderCommand : IRequest<OrderAggregate>
    {
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public string CustomerName { get; set; }
        public List<int> ProductIds { get; set; }

        public AddOrderCommand(int userId, int addressId, string customerName, List<int> productIds)
        {
            UserId = userId;
            AddressId = addressId;
            CustomerName = customerName;
            ProductIds = productIds;
        }

        public class Handler : IRequestHandler<AddOrderCommand, OrderAggregate>
        {
            private readonly IPizzaAppDbContext _dbContext;

            public Handler(IPizzaAppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<OrderAggregate> Handle(AddOrderCommand request, CancellationToken cancellationToken)
            {
                var user = await _dbContext.Users.Where(x => x.Id == request.UserId).Include(x => x.Addresses).FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {request.UserId} was not found.");
                }

                var address = user.Addresses.FirstOrDefault(a => a.Id == request.AddressId);

                if (address == null)
                {
                    throw new KeyNotFoundException($"Address with ID {request.AddressId} for user with ID {request.UserId} was not found.");
                }

                if (string.IsNullOrEmpty(request.CustomerName))
                {
                    throw new Exception("CUSTOMER_NAME_CANNOT_BE_EMPTY");
                }

                var invalidProductIds = request.ProductIds.Except(_dbContext.Products.Select(p => p.Id)).ToList();

                if (invalidProductIds.Any())
                {
                    throw new ArgumentException($"Invalid product IDs: {string.Join(", ", invalidProductIds)}");
                }

                var products = await _dbContext.Products.Where(x => request.ProductIds.Contains(x.Id)).ToListAsync();

                foreach (var product in products )
                {
                    if (product.Quantity > 0)
                        product.DeincrementQuantity();

                    else
                        throw new Exception($"Product {product.Id} is out of stock.");
                }

                var totalAmount = products.Sum(x => x.Price);

                var orderNumber = Guid.NewGuid().ToString();

                var order = OrderAggregate.Create(orderNumber, totalAmount, default, request.CustomerName, products, user, address);

                await _dbContext.Orders.AddAsync(order, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return order;
            }
        }

    }
}
