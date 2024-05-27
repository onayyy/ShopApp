using Application.Common.Interfaces;
using Application.Product.Queries;
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
    public class GetOrderQuery : IRequest<List<OrderAggregate>>
    {
        public class Handler : IRequestHandler<GetOrderQuery, List<OrderAggregate>>
        {
            private readonly IPizzaAppDbContext _pizzaAppDbContext;

            public Handler(IPizzaAppDbContext pizzaAppDbContext)
            {
                _pizzaAppDbContext = pizzaAppDbContext;
            }

            public async Task<List<OrderAggregate>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
            {
                var dbQuery = _pizzaAppDbContext.Orders.AsQueryable();

                var orders = await dbQuery.Include(x => x.Products).ToListAsync(cancellationToken);

                return orders;
            }
        }
    }
}
