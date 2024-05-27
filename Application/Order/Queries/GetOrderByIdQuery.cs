using Application.Common.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderAggregate>
    {
        public int OrderId { get; set; }

        public GetOrderByIdQuery(int orderId)
        {
            OrderId = orderId;
        }

        public class Handler : IRequestHandler<GetOrderByIdQuery, OrderAggregate>
        {
            private readonly IPizzaAppDbContext _pizzaAppDbContext;

            public Handler(IPizzaAppDbContext pizzaAppDbContext)
            {
                _pizzaAppDbContext = pizzaAppDbContext;
            }

            public async Task<OrderAggregate> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
            {
                var dbQuery = _pizzaAppDbContext.Orders.AsQueryable();

                if (request.OrderId > 0)
                {
                    dbQuery = dbQuery.Where(x => x.Id == request.OrderId);
                }

                var order = await dbQuery.Include(x => x.Products).FirstOrDefaultAsync(cancellationToken);

                if (order is null)
                {
                    throw new Exception("Order not found");
                }

                return order;
            }
        }
    }
}
