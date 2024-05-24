using Application.Common.Interfaces;
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
    public class GetProductByIdQuery : IRequest<List<ProductAggregate>>
    {
        public int ProductId { get; set; }

        public GetProductByIdQuery(int productId)
        {
            ProductId = productId;
        }

        public class Handler : IRequestHandler<GetProductByIdQuery, List<ProductAggregate>>
        {
            private readonly IPizzaAppDbContext _pizzaAppDbContext;

            public Handler(IPizzaAppDbContext pizzaAppDbContext)
            {
                _pizzaAppDbContext = pizzaAppDbContext;
            }

            public async Task<List<ProductAggregate>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {
                var dbQuery = _pizzaAppDbContext.Products.AsQueryable();

                if (request.ProductId > 0)
                {
                    dbQuery = dbQuery.Where(x => x.Id == request.ProductId);
                }

                var products = await dbQuery.Include(x => x.Orders).ToListAsync(cancellationToken);

                return products;
            }
        }
    }
}
