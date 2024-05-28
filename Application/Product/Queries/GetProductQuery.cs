using Application.Common.Interfaces;
using Application.Common.Interfaces.RedisCache;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Queries
{
    public class GetProductQuery : IRequest<List<ProductAggregate>>, ICacheableQuery
    {
        public string CacheKey => "GetProduct";

        public double CacheTime => 10;

        public class Handler : IRequestHandler<GetProductQuery, List<ProductAggregate>>
        {
            private readonly IPizzaAppDbContext _pizzaAppDbContext;

            public Handler(IPizzaAppDbContext pizzaAppDbContext)
            {
                _pizzaAppDbContext = pizzaAppDbContext;
            }

            public async Task<List<ProductAggregate>> Handle(GetProductQuery request, CancellationToken cancellationToken)
            {
                var dbQuery = _pizzaAppDbContext.Products.AsQueryable();

                var products = await dbQuery.Include(x => x.Orders).ToListAsync(cancellationToken);

                return products;
            }
        }
    }
}
